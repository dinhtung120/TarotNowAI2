using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public class SpinGachaCommandHandler : IRequestHandler<SpinGachaCommand, SpinGachaResult>
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
        // ───────────────────────────────────────────────────────────
        // 1. Kiểm tra Idempotency (Hỗ trợ quay x10)
        // ───────────────────────────────────────────────────────────
        var replayResult = await HandleIdempotentReplayAsync(request.IdempotencyKey, request.Count, cancellationToken);
        if (replayResult != null) return replayResult;

        // ───────────────────────────────────────────────────────────
        // 2. Validate Banner
        // ───────────────────────────────────────────────────────────
        var banner = await _gachaRepository.GetActiveBannerAsync(request.BannerCode, cancellationToken);

        if (banner == null)
            throw new BadRequestException("Banner is invalid or expired.");

        var items = await _gachaRepository.GetBannerItemsAsync(banner.Id, cancellationToken);

        if (!items.Any() || !banner.ValidateOddsSum(items))
            throw new InvalidOperationException("Banner items configuration is invalid (sum != 10000).");

        // ───────────────────────────────────────────────────────────
        // 3. Trừ Kim Cương (Debit Bulk) cho tổng số lần quay
        // ───────────────────────────────────────────────────────────
        long totalCostDiamond = (long)banner.CostDiamond * request.Count;

        await _walletRepository.DebitAsync(
            userId: request.UserId,
            currency: CurrencyType.Diamond,
            type: TransactionType.GachaCost,
            amount: totalCostDiamond,
            description: $"Spin Gacha {banner.Code} x{request.Count}",
            idempotencyKey: $"gacha_debit_{request.IdempotencyKey}",
            cancellationToken: cancellationToken);

        // ───────────────────────────────────────────────────────────
        // 4. Bắt đầu vòng lặp xóc RNG và tính Pity
        // ───────────────────────────────────────────────────────────
        int currentPity = await _gachaRepository.GetUserPityCountAsync(request.UserId, banner.Id, cancellationToken);
        
        long totalGoldReward = 0;
        long totalDiamondReward = 0;
        
        var resultItems = new List<SpinGachaItemResult>();
        bool anyPityTriggered = false;

        for (int i = 0; i < request.Count; i++)
        {
            bool shouldTriggerHardPity = banner.PityEnabled && currentPity >= (banner.HardPityCount - 1);
            if (shouldTriggerHardPity) anyPityTriggered = true;

            GachaBannerItem selectedItem;
            string? rngSeed = null;

            if (shouldTriggerHardPity)
            {
                var legendaryItems = items.Where(x => x.Rarity == GachaRarity.Legendary).ToList();
                if (!legendaryItems.Any()) throw new InvalidOperationException("Banner missing legendary items for pity.");

                if (legendaryItems.Count == 1)
                {
                    selectedItem = legendaryItems.First();
                }
                else
                {
                    var legendaryWeighted = legendaryItems.Select(x => new WeightedItem { ItemId = x.Id, WeightBasisPoints = x.WeightBasisPoints });
                    var pityRng = _rngService.WeightedSelect(legendaryWeighted);
                    selectedItem = legendaryItems.First(x => x.Id == pityRng.SelectedItemId);
                    rngSeed = pityRng.RngSeed;
                }
            }
            else
            {
                var weightedItems = items.Select(x => new WeightedItem { ItemId = x.Id, WeightBasisPoints = x.WeightBasisPoints });
                var rngResult = _rngService.WeightedSelect(weightedItems);
                selectedItem = items.First(x => x.Id == rngResult.SelectedItemId);
                rngSeed = rngResult.RngSeed;
            }

            // Ghi nhận Pity mới
            int pityCountAtThisSpin = currentPity + 1;
            int newPityForNextSpin = (selectedItem.Rarity == GachaRarity.Legendary) ? 0 : pityCountAtThisSpin;
            currentPity = newPityForNextSpin;

            // Ghi nhận tổng phần thưởng
            if (selectedItem.RewardType == GachaRewardType.Gold)
                totalGoldReward += long.Parse(selectedItem.RewardValue);
            else if (selectedItem.RewardType == GachaRewardType.Diamond)
                totalDiamondReward += long.Parse(selectedItem.RewardValue);
            else if (selectedItem.RewardType == GachaRewardType.Title)
                await _titleRepository.GrantTitleAsync(request.UserId, selectedItem.RewardValue, cancellationToken);

            // Sinh SQL Log
            string logIdempotencyKey = $"{request.IdempotencyKey}_{i}";
            var log = GachaRewardLog.Create(
                userId: request.UserId,
                bannerId: banner.Id,
                bannerItemId: selectedItem.Id,
                oddsVersion: banner.OddsVersion,
                spentDiamond: banner.CostDiamond, // Cost per spin
                rarity: selectedItem.Rarity,
                rewardType: selectedItem.RewardType,
                rewardValue: selectedItem.RewardValue,
                pityCountAtSpin: pityCountAtThisSpin,
                wasPityTriggered: shouldTriggerHardPity,
                rngSeed: rngSeed,
                idempotencyKey: logIdempotencyKey
            );

            // LogRewardAsync sẽ lưu liền (Unit of Work)
            await _gachaRepository.LogRewardAsync(log, cancellationToken);

            // Log Mongo (Best-effort Analytics)
            try
            {
                await _gachaLogRepository.InsertLogAsync(
                    userId: request.UserId, bannerCode: banner.Code, rarity: selectedItem.Rarity,
                    rewardType: selectedItem.RewardType, rewardValue: selectedItem.RewardValue,
                    spentDiamond: banner.CostDiamond, wasPity: shouldTriggerHardPity,
                    rngSeed: rngSeed, createdAt: log.CreatedAt, ct: cancellationToken);
            }
            catch { }

            // Thêm vào array kết quả client
            resultItems.Add(new SpinGachaItemResult
            {
                Rarity = selectedItem.Rarity,
                RewardType = selectedItem.RewardType,
                RewardValue = selectedItem.RewardValue,
                DisplayNameVi = selectedItem.DisplayNameVi,
                DisplayNameEn = selectedItem.DisplayNameEn,
                DisplayIcon = selectedItem.DisplayIcon
            });
        }

        // ───────────────────────────────────────────────────────────
        // 5. Trả thưởng hàng loạt (Credit Bulk) cho Gold và Diamond
        // ───────────────────────────────────────────────────────────
        if (totalGoldReward > 0)
        {
            await _walletRepository.CreditAsync(
                userId: request.UserId, currency: CurrencyType.Gold, type: TransactionType.GachaReward,
                amount: totalGoldReward, description: $"Gacha Gold Rewards x{request.Count}",
                idempotencyKey: $"gacha_credit_gold_{request.IdempotencyKey}", cancellationToken: cancellationToken);
        }

        if (totalDiamondReward > 0)
        {
            await _walletRepository.CreditAsync(
                userId: request.UserId, currency: CurrencyType.Diamond, type: TransactionType.GachaReward,
                amount: totalDiamondReward, description: $"Gacha Diamond Rewards x{request.Count}",
                idempotencyKey: $"gacha_credit_diamond_{request.IdempotencyKey}", cancellationToken: cancellationToken);
        }

        // ───────────────────────────────────────────────────────────
        // 6. Trả kết quả cho Client
        // ───────────────────────────────────────────────────────────
        return new SpinGachaResult
        {
            Success = true,
            IsIdempotentReplay = false,
            CurrentPityCount = currentPity,
            HardPityThreshold = banner.HardPityCount,
            WasPityTriggered = anyPityTriggered,
            Items = resultItems
        };
    }

    private async Task<SpinGachaResult?> HandleIdempotentReplayAsync(string baseKey, int count, CancellationToken ct)
    {
        var existingLogFirst = await _gachaRepository.GetRewardLogsByIdempotencyKeyAsync($"{baseKey}_0", ct);
        if (existingLogFirst == null || !existingLogFirst.Any()) return null;

        var allExistingLogs = new List<GachaRewardLog>();
        for (int i = 0; i < count; i++)
        {
            var logs = await _gachaRepository.GetRewardLogsByIdempotencyKeyAsync($"{baseKey}_{i}", ct);
            if (logs.Any()) allExistingLogs.AddRange(logs);
        }

        if (!allExistingLogs.Any()) allExistingLogs = existingLogFirst;

        var bannerItemsTemp = await _gachaRepository.GetBannerItemsAsync(allExistingLogs.First().BannerId, ct);
        
        var replayItems = allExistingLogs.Select(log => {
            var item = bannerItemsTemp.FirstOrDefault(x => x.Id == log.BannerItemId);
            return new SpinGachaItemResult
            {
                Rarity = log.Rarity,
                RewardType = log.RewardType,
                RewardValue = log.RewardValue,
                DisplayNameVi = item?.DisplayNameVi ?? string.Empty,
                DisplayNameEn = item?.DisplayNameEn ?? string.Empty,
                DisplayIcon = item?.DisplayIcon
            };
        }).ToList();

        var lastLog = allExistingLogs.OrderByDescending(x => x.CreatedAt).First();

        return new SpinGachaResult
        {
            Success = true,
            IsIdempotentReplay = true,
            CurrentPityCount = lastLog.Rarity == GachaRarity.Legendary ? 0 : lastLog.PityCountAtSpin,
            HardPityThreshold = 90,
            WasPityTriggered = allExistingLogs.Any(x => x.WasPityTriggered),
            Items = replayItems
        };
    }
}
