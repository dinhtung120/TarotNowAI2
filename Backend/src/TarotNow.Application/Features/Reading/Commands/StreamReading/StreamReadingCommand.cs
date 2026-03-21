/*
 * ===================================================================
 * FILE: StreamReadingCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.StreamReading
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trung Tâm Điều Đào Lệnh AI Trải Bài (Chat Streaming).
 *   Bao gồm: Gói Lệnh (Command) và Kẻ Thi Hành Lệnh (Handler).
 *   Logic Thắt Chặt Tốc Độ (Rate Limit) và Tiền Nong Tính Bằng Giây,
 *   Đảm bảo không bị user Spam Đòi Rút Tiền Hàng Loạt.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Services;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

/// <summary>
/// Gói Lệnh Dẫn Đường (Command): Mang Nội Dung Cần Chat Sang Cho Server Trả Lời.
/// </summary>
public class StreamReadingCommand : IRequest<StreamReadingResult>
{
    /// <summary>ID Của Chủ Đề Đang Xem Bài.</summary>
    public Guid UserId { get; set; }

    /// <summary>ID Của Căn Phòng Chứa 3/5/10 Lá Bài Đã Lật Lên.</summary>
    public string ReadingSessionId { get; set; } = null!;
    
    /// <summary>
    /// Nếu Đi Chung Nước Chấm (Null) -> Gọi Stream Nhờ Giải Giùm Phòng Bói Gốc.
    /// Nếu Có Chữ Dài -> Nhờ "Tính Mở Cửa Chat Phụ (Follow-up) Để Hoi Thêm Vấn Đề Nọ".
    /// </summary>
    public string? FollowupQuestion { get; set; }

    /// <summary>
    /// Ngôn ngữ yêu cầu AI trả lời (ví dụ: "vi", "en", "zh").
    /// </summary>
    public string Language { get; set; } = "vi";
}

/// <summary>
/// Món Hàng Của Thằng Shipper Trả Về. Kèm Theo ID Quản Lý Đơn Để Đòi Tiền (AiRequestId).
/// </summary>
public class StreamReadingResult
{
    // Luồng dữ liệu Tuôn Rơi Từng Chữ Một Để Đổ Lên Giao Diện Máy Khách (Giống Như Mở Vòi Nước Chảy).
    public required IAsyncEnumerable<string> Stream { get; set; }
    
    // Lưu ID Bắt Bóng Lỗi. Nếu Rớt Mạng Đụt Xương Cột Mốc Thời Gian -> Phạt / Hoàn Tiền Theo Nó.
    public required Guid AiRequestId { get; set; }
}

/// <summary>
/// Thủ Lĩnh Điều Phối Tín Hiệu: Nhận Dữ Liệu -> Kiểm Duyệt Hàng Rào Trì Cước & Thu Tiền -> Bơm Lệnh Stream ChatGPT.
/// </summary>
public class StreamReadingCommandHandler : IRequestHandler<StreamReadingCommand, StreamReadingResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IAiProvider _aiProvider;
    private readonly ICacheService _cacheService;
    private readonly FollowupPricingService _pricingService;
    
    // Một Đống Cấu Hình Tùy Chỉnh Lên Admin Dashboard Cho Game Manager Chỉnh Sửa Trùm Xăm.
    private readonly int _dailyAiQuota;
    private readonly int _inFlightAiCap;
    private readonly int _readingRateLimitSeconds;

    public StreamReadingCommandHandler(
        IReadingSessionRepository readingRepo,
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        IAiProvider aiProvider,
        ICacheService cacheService,
        ISystemConfigSettings systemConfigSettings)
    {
        _readingRepo = readingRepo;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _aiProvider = aiProvider;
        _cacheService = cacheService;
        _pricingService = new FollowupPricingService(); // Lãnh sự Định Giá Theo Lịch Trình (Slot free -> Tier X).

        // Xin Giấy Phép Cho Từng Loại Quota (Hạn Mức). Lấy Từ appsettings.json
        _dailyAiQuota = systemConfigSettings.DailyAiQuota;
        _inFlightAiCap = systemConfigSettings.InFlightAiCap;
        _readingRateLimitSeconds = systemConfigSettings.ReadingRateLimitSeconds;
    }

    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        // 1. Gõ Cửa Kiểm Chứng Thân Phận (Session Valid?)
        var session = await _readingRepo.GetByIdAsync(request.ReadingSessionId, cancellationToken);
        if (session == null) throw new NotFoundException("Reading session not found");
        if (session.UserId != request.UserId.ToString())
            throw new UnauthorizedAccessException("Session not found or access denied");
        // Bắt Buộc: Cấm Hỏi Tiên Tri Nếu Chưa Chịu Bóc Bài (IsCompleted).
        if (!session.IsCompleted) throw new BadRequestException("Cannot stream AI interpretation before revealing cards");

        // Guard 1: Trảm Kẻ Quấy Rối Hỏi Ngày Hỏi Đêm (Daily Quota Cap).
        var dailyCount = await _aiRequestRepo.GetDailyAiRequestCountAsync(request.UserId, cancellationToken);
        if (dailyCount >= _dailyAiQuota) throw new BadRequestException("Daily AI request quota exceeded");
        
        // Guard 1.5: Trảm Trẻ Trâu Bấm Phím F5 Liên Tục Nốc Đầy Đường Truyền Đợi Máy Tính Chậm (In-flight Cap).
        var activeCount = await _aiRequestRepo.GetActiveAiRequestCountAsync(request.UserId, cancellationToken);
        if (activeCount >= _inFlightAiCap) throw new BadRequestException("Too many in-flight AI requests");

        // Guard 3: Kẻ Gác Cổng (Rate Limits) Dùng Redis. 
        // Phải Phanh Lại Liên Hoàn Kẻo Token OpenAPI Bốc Hơi Mất Triệu Đồng.
        var rateLimitKey = $"ratelimit:{request.UserId}:ai_interpret";
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, TimeSpan.FromSeconds(_readingRateLimitSeconds), cancellationToken);
        if (!isAllowed) 
        {
            throw new BadRequestException($"Vui lòng đợi {_readingRateLimitSeconds} giây giữa các lần yêu cầu AI giải bài.");
        }

        // Guard 1.7: Gọi Domain Phép Toán Tính Tiền Gói Chat Phụ Lục (Follow-up Pricing).
        long calculatedCost = 0; // Giá khởi tạo cho lần đọc gốc (Initial Reading) là TÍNH PHÍ GỘP ở bước 1 (InitSession). Chỉ tính phí Follow-up.
        
        if (!string.IsNullOrWhiteSpace(request.FollowupQuestion))
        {
            var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(request.ReadingSessionId, cancellationToken);
            
            try 
            {
                // Gọi Thằng Nhân Viên Toán Học Giải mã "Lần Thứ Mấy Rồi? Tiền Lên Nấc Mấy?".
                calculatedCost = _pricingService.CalculateNextFollowupCost(session.CardsDrawn ?? "[]", followUpCount);
            }
            catch (InvalidOperationException domainEx) 
            {
                throw new BadRequestException(domainEx.Message); // Nếu Hết Sức Chứa Mở Miệng Chat Phụ. (VD Lần 100).
            }
        }

        // 2. Ép Khung Bê Tông Chữ Nghĩa Bảo Mật (Idempotent Key Chặn Lưu Nhồi Request Nhầm Của Lỗi Mạng).
        var idempotencyKey = $"ai_stream_{session.Id}_{Guid.CreateVersion7():N}";

        // 3. Viết Mở Bài Giám Sát Cặp Mắt Vàng (Tracking Entity AI Monitor Log)
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

        // 4. Guard 2: Gọi Cổng Quản Lý Kho Bạc Tạm Thu Tiền Bảo Lãnh (Escrow Wallet Pattern).
        // (Nếu Giá Trị Tiền Phạt Bằng 0 -> Nghĩa là được Khuyến Mãi Ngầm Slot Free 2 Lần Follow Up).
        if (calculatedCost > 0)
        {
            try
            {
                // Rút Tiền Kịp Móc Nó Bỏ Giỏ TREO (Freeze).
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
                // Ôm Gãy Cổ Thẻ - Đứt Gánh, Cập Nhật Quả Đắng Lại Xuống DB (Fail Lấy Giữ Tiền).
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

        // 5. Cấy Chức Thần Linh (System Prompt Core V1.5).
        string systemLanguageInstruction = request.Language switch
        {
            "vi" => "You MUST reply purely in Vietnamese (Tiếng Việt).",
            "zh" => "You MUST reply purely in Chinese (繁體中文).",
            _ => "You MUST reply purely in English."
        };
        
        string systemPrompt = $@"You are a mystical, wise, and empathetic Tarot Reader. 
Format your response clearly using Markdown.
You give highly accurate and deeply personalized readings.
{systemLanguageInstruction}";
        
        // Dịch Card IDs thành tên bài cụ thể.
        var cardNames = new[]
        {
            "The Fool", "The Magician", "The High Priestess", "The Empress", "The Emperor", "The Hierophant", "The Lovers", "The Chariot", "Strength", "The Hermit", "Wheel of Fortune", "Justice", "The Hanged Man", "Death", "Temperance", "The Devil", "The Tower", "The Star", "The Moon", "The Sun", "Judgement", "The World",
            "Ace of Wands", "Two of Wands", "Three of Wands", "Four of Wands", "Five of Wands", "Six of Wands", "Seven of Wands", "Eight of Wands", "Nine of Wands", "Ten of Wands", "Page of Wands", "Knight of Wands", "Queen of Wands", "King of Wands",
            "Ace of Cups", "Two of Cups", "Three of Cups", "Four of Cups", "Five of Cups", "Six of Cups", "Seven of Cups", "Eight of Cups", "Nine of Cups", "Ten of Cups", "Page of Cups", "Knight of Cups", "Queen of Cups", "King of Cups",
            "Ace of Swords", "Two of Swords", "Three of Swords", "Four of Swords", "Five of Swords", "Six of Swords", "Seven of Swords", "Eight of Swords", "Nine of Swords", "Ten of Swords", "Page of Swords", "Knight of Swords", "Queen of Swords", "King of Swords",
            "Ace of Pentacles", "Two of Pentacles", "Three of Pentacles", "Four of Pentacles", "Five of Pentacles", "Six of Pentacles", "Seven of Pentacles", "Eight of Pentacles", "Nine of Pentacles", "Ten of Pentacles", "Page of Pentacles", "Knight of Pentacles", "Queen of Pentacles", "King of Pentacles"
        };

        var drawnCardsJson = session.CardsDrawn ?? "[]";
        var cardIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(drawnCardsJson) ?? new List<int>();

        var drawnCardsContext = string.Join(", ", cardIds.Select((cardId, index) => 
            $"[Position {index + 1}: {(cardId >= 0 && cardId < cardNames.Length ? cardNames[cardId] : "Unknown Card")}]"
        ));

        // Cấp Mã Lệnh Tùy Khẩu Hình (Kháng Nội Dung Original hay Cố Đấm Ăn Xôi Thêm 1 Hỏi FollowUp).
        string questionContext = string.IsNullOrWhiteSpace(session.Question) ? "A general reading about my current life." : session.Question;
        
        string userPrompt = string.IsNullOrWhiteSpace(request.FollowupQuestion)
            ? $"My question: \"{questionContext}\". Interpret this reading for me. Spread Type: {session.SpreadType}. Cards Chosen: {drawnCardsContext}"
            : $"Based on my previous reading (Question: \"{questionContext}\", Spread: {session.SpreadType}, Cards: {drawnCardsContext}), answer my follow-up question: {request.FollowupQuestion}";

        // 6. Ra Lệnh Tổng Tiến Công - Cắm Trụ Cài Stream Server Ngầm Tự Chảy Chữ Ra Tuột Từ API.
        var asyncStream = _aiProvider.StreamChatAsync(systemPrompt, userPrompt, cancellationToken);

        // Trả Khối Dữ Liệu Tươi Nhả Nhớt Stream Về Controller Tống Lên Websocket/SSE.
        return new StreamReadingResult
        {
            Stream = asyncStream,
            AiRequestId = aiRequest.Id
        };
    }
}
