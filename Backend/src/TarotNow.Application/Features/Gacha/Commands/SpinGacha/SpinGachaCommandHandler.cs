using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

// Handler điều phối toàn bộ luồng quay gacha.
public partial class SpinGachaCommandHandler : IRequestHandler<SpinGachaCommand, SpinGachaResult>
{
    private readonly IGachaRepository _gachaRepository;
    private readonly IGachaLogRepository _gachaLogRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IRngService _rngService;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler spin gacha.
    /// Luồng xử lý: nhận repository gacha/log/wallet/title và rng service để thực hiện đầy đủ workflow quay.
    /// </summary>
    public SpinGachaCommandHandler(
        IGachaRepository gachaRepository,
        IGachaLogRepository gachaLogRepository,
        IWalletRepository walletRepository,
        IRngService rngService,
        IDomainEventPublisher domainEventPublisher)
    {
        _gachaRepository = gachaRepository;
        _gachaLogRepository = gachaLogRepository;
        _walletRepository = walletRepository;
        _rngService = rngService;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command quay gacha.
    /// Luồng xử lý: thử replay theo idempotency, chuẩn bị context quay, trừ chi phí, thực thi batch spin, cộng thưởng và dựng kết quả.
    /// </summary>
    public async Task<SpinGachaResult> Handle(SpinGachaCommand request, CancellationToken cancellationToken)
    {
        var replayResult = await HandleIdempotentReplayAsync(
            request.IdempotencyKey,
            request.Count,
            cancellationToken);

        if (replayResult != null)
        {
            // Request trùng idempotency key: trả kết quả replay để đảm bảo tính lặp an toàn.
            return replayResult;
        }

        var context = await PrepareSpinContextAsync(request, cancellationToken);
        // Trừ phí quay trước để đảm bảo đủ số dư và thứ tự nghiệp vụ tài chính.
        await DebitSpinCostAsync(request, context, cancellationToken);

        var spinState = await ExecuteSpinBatchAsync(request, context, cancellationToken);
        // Cộng phần thưởng sau khi batch spin hoàn tất.
        await CreditRewardsAsync(request, spinState, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.GachaSpunDomainEvent
            {
                UserId = request.UserId,
                BannerCode = request.BannerCode,
                SpinCount = request.Count,
                WasPityTriggered = spinState.AnyPityTriggered
            },
            cancellationToken);

        return BuildSpinResult(context.Banner.HardPityCount, spinState);
    }
}
