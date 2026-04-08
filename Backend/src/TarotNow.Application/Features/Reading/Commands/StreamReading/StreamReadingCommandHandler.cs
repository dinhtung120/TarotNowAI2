using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Services;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// Handler điều phối luồng stream reading: validate session/quota/rate-limit, tạo request, freeze escrow và trả stream AI.
public partial class StreamReadingCommandHandler : IRequestHandler<StreamReadingCommand, StreamReadingResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IAiProvider _aiProvider;
    private readonly ICacheService _cacheService;
    private readonly FollowupPricingService _pricingService;
    private readonly int _dailyAiQuota;
    private readonly int _inFlightAiCap;
    private readonly int _readingRateLimitSeconds;
    private readonly IWalletPushService _walletPushService;
    private readonly IEntitlementService _entitlementService;

    /// <summary>
    /// Khởi tạo handler stream reading.
    /// Luồng xử lý: nhận repository/service cho validation, pricing, wallet escrow, ai streaming và thông báo số dư.
    /// </summary>
    public StreamReadingCommandHandler(
        IReadingSessionRepository readingRepo,
        IAiRequestRepository aiRequestRepo,
        IWalletRepository walletRepo,
        IAiProvider aiProvider,
        ICacheService cacheService,
        FollowupPricingService pricingService,
        ISystemConfigSettings systemConfigSettings,
        IWalletPushService walletPushService,
        IEntitlementService entitlementService)
    {
        _readingRepo = readingRepo;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
        _aiProvider = aiProvider;
        _cacheService = cacheService;
        _pricingService = pricingService;
        _dailyAiQuota = systemConfigSettings.DailyAiQuota;
        _inFlightAiCap = systemConfigSettings.InFlightAiCap;
        _readingRateLimitSeconds = systemConfigSettings.ReadingRateLimitSeconds;
        _walletPushService = walletPushService;
        _entitlementService = entitlementService;
    }

    /// <summary>
    /// Xử lý command stream reading.
    /// Luồng xử lý: validate điều kiện nghiệp vụ, tính chi phí, tạo AI request, freeze escrow, build prompt và trả stream kết quả.
    /// </summary>
    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        var session = await ValidateSessionAsync(request, cancellationToken);
        await EnsureQuotaAsync(request.UserId, cancellationToken);
        await EnsureRateLimitAsync(request.UserId, cancellationToken);
        // Chuỗi validation chặn sớm các trường hợp không hợp lệ trước khi tạo request/billing.

        var calculatedCost = await CalculateCostAsync(request, session, cancellationToken);
        var aiRequest = await CreateAiRequestAsync(request, session, calculatedCost, cancellationToken);

        await FreezeEscrowAsync(request, aiRequest, calculatedCost, cancellationToken);
        // Chỉ khi freeze thành công mới cho phép tạo stream để đảm bảo an toàn tài chính.

        var prompts = StreamReadingPromptFactory.Build(session, request.FollowupQuestion, request.Language);
        var stream = _aiProvider.StreamChatAsync(prompts.SystemPrompt, prompts.UserPrompt, cancellationToken);

        await _walletPushService.PushBalanceChangedAsync(request.UserId, cancellationToken);
        // Chủ động push số dư mới để UI phản ánh thay đổi escrow ngay khi bắt đầu stream.

        return new StreamReadingResult
        {
            Stream = stream,
            AiRequestId = aiRequest.Id
        };
    }
}
