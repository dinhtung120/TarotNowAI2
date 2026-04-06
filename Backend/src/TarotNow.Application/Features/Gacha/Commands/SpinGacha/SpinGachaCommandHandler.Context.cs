using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private async Task<SpinExecutionContext> PrepareSpinContextAsync(
        SpinGachaCommand request,
        CancellationToken cancellationToken)
    {
        var banner = await _gachaRepository.GetActiveBannerAsync(request.BannerCode, cancellationToken);
        if (banner == null)
        {
            throw new BadRequestException("Banner is invalid or expired.");
        }

        var items = await _gachaRepository.GetBannerItemsAsync(banner.Id, cancellationToken);
        if (!items.Any() || !banner.ValidateOddsSum(items))
        {
            throw new InvalidOperationException("Banner items configuration is invalid (sum != 10000).");
        }

        var currentPity = await _gachaRepository.GetUserPityCountAsync(request.UserId, banner.Id, cancellationToken);
        var totalCostDiamond = (long)banner.CostDiamond * request.Count;

        return new SpinExecutionContext(banner, items, currentPity, totalCostDiamond);
    }

    private Task DebitSpinCostAsync(
        SpinGachaCommand request,
        SpinExecutionContext context,
        CancellationToken cancellationToken)
    {
        return _walletRepository.DebitAsync(
            userId: request.UserId,
            currency: CurrencyType.Diamond,
            type: TransactionType.GachaCost,
            amount: context.TotalCostDiamond,
            description: $"Spin Gacha {context.Banner.Code} x{request.Count}",
            idempotencyKey: $"gacha_debit_{request.IdempotencyKey}",
            cancellationToken: cancellationToken);
    }

    private async Task CreditRewardsAsync(
        SpinGachaCommand request,
        SpinBatchState state,
        CancellationToken cancellationToken)
    {
        await CreditRewardAsync(
            request,
            CurrencyType.Gold,
            state.TotalGoldReward,
            "gold",
            cancellationToken);

        await CreditRewardAsync(
            request,
            CurrencyType.Diamond,
            state.TotalDiamondReward,
            "diamond",
            cancellationToken);
    }

    private Task CreditRewardAsync(
        SpinGachaCommand request,
        string currency,
        long amount,
        string suffix,
        CancellationToken cancellationToken)
    {
        if (amount <= 0)
        {
            return Task.CompletedTask;
        }

        return _walletRepository.CreditAsync(
            userId: request.UserId,
            currency: currency,
            type: TransactionType.GachaReward,
            amount: amount,
            description: $"Gacha {currency} Rewards x{request.Count}",
            idempotencyKey: $"gacha_credit_{suffix}_{request.IdempotencyKey}",
            cancellationToken: cancellationToken);
    }

    private static SpinGachaResult BuildSpinResult(int hardPityThreshold, SpinBatchState state)
    {
        return new SpinGachaResult
        {
            Success = true,
            IsIdempotentReplay = false,
            CurrentPityCount = state.CurrentPity,
            HardPityThreshold = hardPityThreshold,
            WasPityTriggered = state.AnyPityTriggered,
            Items = state.Items
        };
    }
}
