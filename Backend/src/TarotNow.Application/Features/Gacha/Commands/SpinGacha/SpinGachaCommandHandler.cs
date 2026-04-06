using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler : IRequestHandler<SpinGachaCommand, SpinGachaResult>
{
    private readonly IGachaRepository _gachaRepository;
    private readonly IGachaLogRepository _gachaLogRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITitleRepository _titleRepository;
    private readonly IRngService _rngService;

    public SpinGachaCommandHandler(
        IGachaRepository gachaRepository,
        IGachaLogRepository gachaLogRepository,
        IWalletRepository walletRepository,
        ITitleRepository titleRepository,
        IRngService rngService)
    {
        _gachaRepository = gachaRepository;
        _gachaLogRepository = gachaLogRepository;
        _walletRepository = walletRepository;
        _titleRepository = titleRepository;
        _rngService = rngService;
    }

    public async Task<SpinGachaResult> Handle(SpinGachaCommand request, CancellationToken cancellationToken)
    {
        var replayResult = await HandleIdempotentReplayAsync(
            request.IdempotencyKey,
            request.Count,
            cancellationToken);

        if (replayResult != null)
        {
            return replayResult;
        }

        var context = await PrepareSpinContextAsync(request, cancellationToken);
        await DebitSpinCostAsync(request, context, cancellationToken);

        var spinState = await ExecuteSpinBatchAsync(request, context, cancellationToken);
        await CreditRewardsAsync(request, spinState, cancellationToken);

        return BuildSpinResult(context.Banner.HardPityCount, spinState);
    }
}
