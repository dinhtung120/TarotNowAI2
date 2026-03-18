using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Services;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public class StreamReadingCommand : IRequest<StreamReadingResult>
{
    public Guid UserId { get; set; }
    public string ReadingSessionId { get; set; } = null!;
    
    // Câu hỏi Follow-up (nếu có). 
    // Nếu là null => Gọi Stream cho lần bói gốc 3 lá.
    public string? FollowupQuestion { get; set; }
}

public class StreamReadingResult
{
    public required IAsyncEnumerable<string> Stream { get; set; }
    public required Guid AiRequestId { get; set; }
    public required IAiProvider Provider { get; set; }
}

public class StreamReadingCommandHandler : IRequestHandler<StreamReadingCommand, StreamReadingResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IAiProvider _aiProvider;
    private readonly ICacheService _cacheService;
    private readonly FollowupPricingService _pricingService;

    public StreamReadingCommandHandler(
        IReadingSessionRepository readingRepo,
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        IAiProvider aiProvider,
        ICacheService cacheService)
    {
        _readingRepo = readingRepo;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _aiProvider = aiProvider;
        _cacheService = cacheService;
        _pricingService = new FollowupPricingService(); // Domain service is stateless, can instantiate directly or inject.
    }

    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra phiên trải bài
        var session = await _readingRepo.GetByIdAsync(request.ReadingSessionId, cancellationToken);
        if (session == null) throw new NotFoundException("Reading session not found");
        if (session.UserId != request.UserId.ToString())
            throw new UnauthorizedAccessException("Session not found or access denied");
        if (!session.IsCompleted) throw new BadRequestException("Cannot stream AI interpretation before revealing cards");

        // Guard 1: Daily Quota Cap (Max 3)
        var dailyCount = await _aiRequestRepo.GetDailyAiRequestCountAsync(request.UserId, cancellationToken);
        if (dailyCount >= 3) throw new BadRequestException("Daily AI request quota exceeded");
        
        // Guard 1.5: In-flight Cap (Max 3 concurrent)
        var activeCount = await _aiRequestRepo.GetActiveAiRequestCountAsync(request.UserId, cancellationToken);
        if (activeCount >= 3) throw new BadRequestException("Too many in-flight AI requests");

        // Guard 3: Rate Limiting (Phase 1.5 spec) - Chống spam request AI
        // Giới hạn: tối đa 1 request interpretation/follow-up mỗi 30 giây cho mỗi user.
        // Tại sao cần Guard này? 
        // -> Vì AI Interpretation là tốn phí thật (token OpenAI). User click spam nút bói bài
        //    có thể gây tốn quota hoặc freeze ví liên tục, làm crash bộ đếm.
        var rateLimitKey = $"ratelimit:{request.UserId}:ai_interpret";
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, TimeSpan.FromSeconds(30), cancellationToken);
        if (!isAllowed) 
        {
            throw new BadRequestException("Vui lòng đợi 30 giây giữa các lần yêu cầu AI giải bài.");
        }

        // Guard 1.7: Follow-up Pricing & Hard Cap (Phase 1.5)
        long calculatedCost = 5; // Default reading cost
        
        if (!string.IsNullOrWhiteSpace(request.FollowupQuestion))
        {
            var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(request.ReadingSessionId, cancellationToken);
            
            try 
            {
                // Domain service Calculates free slots and next price tier
                calculatedCost = _pricingService.CalculateNextFollowupCost(session.CardsDrawn ?? "[]", followUpCount);
            }
            catch (InvalidOperationException domainEx) 
            {
                throw new BadRequestException(domainEx.Message); // Re-throw Cap Reached error
            }
        }

        // 2. Tạo Idempotency Key chống Deduplicate
        var idempotencyKey = $"ai_stream_{session.Id}_{request.FollowupQuestion?.GetHashCode() ?? 0}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        // 3. Khởi tạo Entity Theo dõi AI Request (State Machine)
        var aiRequest = new AiRequest
        {
            UserId = request.UserId,
            ReadingSessionRef = session.Id,
            Status = AiRequestStatus.Requested,
            IdempotencyKey = idempotencyKey,
            PromptVersion = "v1.5",
            ChargeDiamond = calculatedCost
        };

        await _aiRequestRepo.AddAsync(aiRequest, cancellationToken);

        // 4. Guard 2: Đóng băng (Freeze) Quota Atomic bằng WalletRepository (Escrow Pattern)
        // Chỉ đóng băng ví thực nếu calculatedCost > 0 (Tức là không được Free Slot bao trọn)
        if (calculatedCost > 0)
        {
            try
            {
                // Tạm đóng băng Diamond vào Escrow để đảm bảo nguyên bản (Atomicity & Refundable)
                await _walletRepo.FreezeAsync(
                    userId: request.UserId,
                    amount: calculatedCost,
                    referenceSource: "AiRequest",
                    referenceId: aiRequest.Id.ToString(),
                    description: string.IsNullOrWhiteSpace(request.FollowupQuestion) ? "Escrow freeze for initial Tarot Reading" : "Escrow freeze for Follow-up Chat",
                    idempotencyKey: $"freeze_{aiRequest.Id}",
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                // Quỹ không đủ hoặc lỗi trừ tiền
                aiRequest.Status = AiRequestStatus.FailedBeforeFirstToken;
                aiRequest.FinishReason = "insufficient_funds_or_error";
                await _aiRequestRepo.UpdateAsync(aiRequest, cancellationToken);

                if (ex is InvalidOperationException)
                {
                    throw new BadRequestException("Not enough balance to perform AI Reading.");
                }

                throw new BadRequestException("Unable to reserve balance for AI Reading. Please try again later.");
            }
        }

        // 5. Build Prompts
        string systemPrompt = "You are a mystical, wise, and empathetic Tarot Reader. Format your response clearly using Markdown.";
        string userPrompt = string.IsNullOrWhiteSpace(request.FollowupQuestion)
            ? $"Interpret this reading for me. Spread Type: {session.SpreadType}. Cards Chosen: {session.CardsDrawn}"
            : $"Based on my previous reading (Spread: {session.SpreadType}, Cards: {session.CardsDrawn}), answer my follow-up question: {request.FollowupQuestion}";

        // 6. Kích hoạt Provider chạy ngầm lấy Luồng Stream
        // Lưu ý: Stream sẽ được Consumer (Controller) tiêu thụ dần,
        // CancellationToken sẽ được truyền từ HttpRequest xuống tận HttpClient của OpenAiProvider.
        var asyncStream = _aiProvider.StreamChatAsync(systemPrompt, userPrompt, cancellationToken);

        // Trả kết nối IAsyncEnumerable ra tận ngoài Controller để Controller Server Sent Events thẳng xuống Browser
        return new StreamReadingResult
        {
            Stream = asyncStream,
            AiRequestId = aiRequest.Id,
            Provider = _aiProvider
        };
    }
}
