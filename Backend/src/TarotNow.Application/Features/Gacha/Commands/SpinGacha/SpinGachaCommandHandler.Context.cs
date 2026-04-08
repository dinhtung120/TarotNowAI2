using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Chuẩn bị context cho batch quay gacha.
    /// Luồng xử lý: tải banner active, tải item banner + validate odds sum, lấy pity hiện tại của user và tính tổng chi phí quay.
    /// </summary>
    private async Task<SpinExecutionContext> PrepareSpinContextAsync(
        SpinGachaCommand request,
        CancellationToken cancellationToken)
    {
        var banner = await _gachaRepository.GetActiveBannerAsync(request.BannerCode, cancellationToken);
        if (banner == null)
        {
            // Banner không hợp lệ/hết hạn thì từ chối quay.
            throw new BadRequestException("Banner is invalid or expired.");
        }

        var items = await _gachaRepository.GetBannerItemsAsync(banner.Id, cancellationToken);
        if (!items.Any() || !banner.ValidateOddsSum(items))
        {
            // Cấu hình odds sai gây mất công bằng quay nên phải chặn toàn bộ request.
            throw new InvalidOperationException("Banner items configuration is invalid (sum != 10000).");
        }

        var currentPity = await _gachaRepository.GetUserPityCountAsync(request.UserId, banner.Id, cancellationToken);
        var totalCostDiamond = (long)banner.CostDiamond * request.Count;

        return new SpinExecutionContext(banner, items, currentPity, totalCostDiamond);
    }

    /// <summary>
    /// Trừ chi phí quay gacha từ ví diamond của user.
    /// Luồng xử lý: gọi wallet debit với idempotency key ổn định theo request.
    /// </summary>
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

    /// <summary>
    /// Cộng phần thưởng dạng tiền tệ sau khi kết thúc batch quay.
    /// Luồng xử lý: cộng gold và diamond tổng hợp qua hai lệnh credit riêng.
    /// </summary>
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

    /// <summary>
    /// Cộng một loại phần thưởng tiền tệ cụ thể vào ví user.
    /// Luồng xử lý: bỏ qua amount <= 0; nếu có giá trị thì gọi wallet credit với idempotency key theo suffix.
    /// </summary>
    private Task CreditRewardAsync(
        SpinGachaCommand request,
        string currency,
        long amount,
        string suffix,
        CancellationToken cancellationToken)
    {
        if (amount <= 0)
        {
            // Không phát sinh thưởng thì bỏ qua gọi ví để tránh transaction thừa.
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

    /// <summary>
    /// Dựng kết quả trả về cho batch quay thành công.
    /// Luồng xử lý: map pity trạng thái, cờ trigger pity và danh sách item nhận được.
    /// </summary>
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
