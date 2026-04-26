using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Services;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// Handler điều phối luồng stream reading: validate session/quota/rate-limit, tạo request, freeze escrow và trả stream AI.
public partial class StreamReadingCommandExecutor : ICommandExecutionExecutor<StreamReadingCommand, StreamReadingResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IAiProvider _aiProvider;
    private readonly ICacheService _cacheService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly FollowupPricingService _pricingService;
    private readonly int _dailyAiQuota;
    private readonly int _inFlightAiCap;
    private readonly int _readingRateLimitSeconds;
    private readonly int _aiQuotaReservationLeaseSeconds;
    private readonly string _aiPromptVersion;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler stream reading.
    /// Luồng xử lý: nhận repository/service cho validation, pricing, wallet escrow, ai streaming và thông báo số dư.
    /// </summary>
    public StreamReadingCommandExecutor(
        IReadingSessionRepository readingRepo,
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        IAiProvider aiProvider,
        ICacheService cacheService,
        ITransactionCoordinator transactionCoordinator,
        FollowupPricingService pricingService,
        ISystemConfigSettings systemConfigSettings,
        IDomainEventPublisher domainEventPublisher)
    {
        _readingRepo = readingRepo;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _aiProvider = aiProvider;
        _cacheService = cacheService;
        _transactionCoordinator = transactionCoordinator;
        _pricingService = pricingService;
        _dailyAiQuota = systemConfigSettings.DailyAiQuota;
        _inFlightAiCap = systemConfigSettings.InFlightAiCap;
        _readingRateLimitSeconds = systemConfigSettings.ReadingRateLimitSeconds;
        _aiQuotaReservationLeaseSeconds = systemConfigSettings.OperationalAiQuotaReservationLeaseSeconds;
        _aiPromptVersion = systemConfigSettings.OperationalAiPromptVersion;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command stream reading.
    /// Luồng xử lý: validate điều kiện nghiệp vụ, tính chi phí, tạo AI request, freeze escrow, build prompt và trả stream kết quả.
    /// </summary>
    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        var session = await ValidateSessionAsync(request, cancellationToken);
        var readingSessionRef = ParseReadingSessionRefOrThrow(session.Id);
        await EnsureRateLimitAsync(request.UserId, cancellationToken);
        // Chuỗi validation chặn sớm các trường hợp không hợp lệ trước khi tạo request/billing.

        var calculatedCost = await CalculateCostAsync(request, session, readingSessionRef, cancellationToken);
        var normalizedIdempotencyKey = ResolveIdempotencyKeyForRequest(request, calculatedCost);
        var aiRequest = await ReserveAndCreateAiRequestAsync(
            request,
            readingSessionRef,
            calculatedCost,
            normalizedIdempotencyKey,
            cancellationToken);

        var prompts = StreamReadingPromptFactory.Build(session, request.FollowupQuestion, request.Language);
        var stream = _aiProvider.StreamChatAsync(prompts.SystemPrompt, prompts.UserPrompt, cancellationToken);
        var estimatedInputTokens = EstimateTokenCount(prompts.SystemPrompt) + EstimateTokenCount(prompts.UserPrompt);

        if (calculatedCost > 0)
        {
            await _domainEventPublisher.PublishAsync(
                new Domain.Events.MoneyChangedDomainEvent
                {
                    UserId = request.UserId,
                    Currency = Domain.Enums.CurrencyType.Diamond,
                    ChangeType = Domain.Enums.TransactionType.EscrowFreeze,
                    DeltaAmount = -calculatedCost,
                    ReferenceId = aiRequest.Id.ToString()
                },
                cancellationToken);
            // Sau khi freeze escrow, publish event để luồng realtime xử lý cập nhật ví.
        }

        return new StreamReadingResult
        {
            Stream = stream,
            AiRequestId = aiRequest.Id,
            EstimatedInputTokens = estimatedInputTokens
        };
    }

    private static int EstimateTokenCount(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return 0;
        }

        var normalizedLength = content.Trim().Length;
        return Math.Max(1, (int)Math.Ceiling(normalizedLength / 4d));
    }
}
