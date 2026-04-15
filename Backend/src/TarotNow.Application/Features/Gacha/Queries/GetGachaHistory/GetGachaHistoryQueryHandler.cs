using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

/// <summary>
/// Handler truy vấn lịch sử gacha.
/// </summary>
public sealed class GetGachaHistoryQueryHandler : IRequestHandler<GetGachaHistoryQuery, GachaHistoryPageDto>
{
    private readonly IGachaPoolRepository _gachaPoolRepository;

    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public GetGachaHistoryQueryHandler(IGachaPoolRepository gachaPoolRepository)
    {
        _gachaPoolRepository = gachaPoolRepository;
    }

    /// <summary>
    /// Xử lý query lịch sử gacha.
    /// </summary>
    public async Task<GachaHistoryPageDto> Handle(GetGachaHistoryQuery request, CancellationToken cancellationToken)
    {
        var normalizedPage = Math.Max(1, request.Page);
        var normalizedPageSize = Math.Clamp(request.PageSize, 1, 100);
        var historyPage = await _gachaPoolRepository.GetUserPullHistoryAsync(
            request.UserId,
            normalizedPage,
            normalizedPageSize,
            cancellationToken);
        if (historyPage.Items.Count == 0)
        {
            return new GachaHistoryPageDto
            {
                Page = historyPage.Page,
                PageSize = historyPage.PageSize,
                TotalCount = historyPage.TotalCount,
                Items = Array.Empty<GachaHistoryEntryDto>(),
            };
        }

        var operationIds = historyPage.Items.Select(x => x.PullOperationId).ToArray();
        var rewardLogs = await _gachaPoolRepository.GetRewardLogsByOperationIdsAsync(operationIds, cancellationToken);
        var rewardsByOperation = rewardLogs
            .GroupBy(x => x.PullOperationId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<GachaHistoryRewardDto>)x
                    .OrderBy(y => y.CreatedAtUtc)
                    .Select(MapHistoryReward)
                    .ToList());

        var items = historyPage.Items.Select(entry => new GachaHistoryEntryDto
        {
            PullOperationId = entry.PullOperationId,
            PoolCode = entry.PoolCode,
            PullCount = entry.PullCount,
            PityBefore = entry.PityBefore,
            PityAfter = entry.PityAfter,
            WasPityReset = entry.WasPityReset,
            CreatedAtUtc = entry.CreatedAtUtc,
            Rewards = rewardsByOperation.TryGetValue(entry.PullOperationId, out var rewards)
                ? rewards
                : Array.Empty<GachaHistoryRewardDto>(),
        }).ToList();

        return new GachaHistoryPageDto
        {
            Page = historyPage.Page,
            PageSize = historyPage.PageSize,
            TotalCount = historyPage.TotalCount,
            Items = items,
        };
    }

    private static GachaHistoryRewardDto MapHistoryReward(Domain.Entities.GachaPullRewardLog rewardLog)
    {
        return new GachaHistoryRewardDto
        {
            Kind = rewardLog.RewardKind,
            Rarity = rewardLog.Rarity,
            Currency = rewardLog.Currency,
            Amount = rewardLog.Amount,
            ItemCode = rewardLog.ItemCode,
            QuantityGranted = rewardLog.QuantityGranted,
            IconUrl = rewardLog.IconUrl,
            NameVi = rewardLog.NameVi,
            NameEn = rewardLog.NameEn,
            NameZh = rewardLog.NameZh,
            IsHardPityReward = rewardLog.IsHardPityReward,
        };
    }
}
