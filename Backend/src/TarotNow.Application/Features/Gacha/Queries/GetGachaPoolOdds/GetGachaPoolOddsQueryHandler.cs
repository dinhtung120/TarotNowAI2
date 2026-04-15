using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaPoolOdds;

/// <summary>
/// Handler truy vấn odds của pool gacha.
/// </summary>
public sealed class GetGachaPoolOddsQueryHandler : IRequestHandler<GetGachaPoolOddsQuery, GachaPoolOddsDto>
{
    private readonly IGachaPoolRepository _gachaPoolRepository;
    private readonly IItemDefinitionRepository _itemDefinitionRepository;

    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public GetGachaPoolOddsQueryHandler(
        IGachaPoolRepository gachaPoolRepository,
        IItemDefinitionRepository itemDefinitionRepository)
    {
        _gachaPoolRepository = gachaPoolRepository;
        _itemDefinitionRepository = itemDefinitionRepository;
    }

    /// <summary>
    /// Xử lý query lấy odds.
    /// </summary>
    public async Task<GachaPoolOddsDto> Handle(GetGachaPoolOddsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PoolCode))
        {
            throw new BusinessRuleException(GachaErrorCodes.PoolNotFound, "Pool code is required.");
        }

        var normalizedPoolCode = request.PoolCode.Trim().ToLowerInvariant();
        var pool = await _gachaPoolRepository.GetActivePoolByCodeAsync(normalizedPoolCode, cancellationToken);
        if (pool is null)
        {
            throw new BusinessRuleException(GachaErrorCodes.PoolNotFound, $"Pool '{normalizedPoolCode}' not found.");
        }

        var rewards = await _gachaPoolRepository.GetActiveRewardRatesAsync(pool.Id, cancellationToken);
        if (rewards.Count == 0)
        {
            throw new BusinessRuleException(GachaErrorCodes.InvalidPoolConfiguration, "Pool reward configuration is empty.");
        }

        var itemDefinitionMap = await LoadItemDefinitionMapAsync(rewards, cancellationToken);

        return new GachaPoolOddsDto
        {
            PoolCode = pool.Code,
            OddsVersion = pool.OddsVersion,
            Rewards = rewards.Select(r => new GachaPoolRewardRateDto
            {
                Kind = r.RewardKind,
                Rarity = r.Rarity,
                Currency = r.Currency,
                Amount = r.Amount,
                ItemDefinitionId = r.ItemDefinitionId,
                ItemCode = ResolveItemCode(r.ItemDefinitionId, itemDefinitionMap),
                QuantityGranted = r.QuantityGranted,
                ProbabilityBasisPoints = r.ProbabilityBasisPoints,
                ProbabilityPercent = r.ProbabilityBasisPoints / 100d,
                IconUrl = r.IconUrl,
                NameVi = r.NameVi,
                NameEn = r.NameEn,
                NameZh = r.NameZh,
            }).ToList(),
        };
    }

    private async Task<Dictionary<Guid, string>> LoadItemDefinitionMapAsync(
        IReadOnlyList<Domain.Entities.GachaPoolRewardRate> rewards,
        CancellationToken cancellationToken)
    {
        var itemDefinitionIds = rewards
            .Where(x => x.ItemDefinitionId.HasValue)
            .Select(x => x.ItemDefinitionId!.Value)
            .Distinct()
            .ToList();
        if (itemDefinitionIds.Count == 0)
        {
            return new Dictionary<Guid, string>();
        }

        var result = new Dictionary<Guid, string>(itemDefinitionIds.Count);
        foreach (var itemDefinitionId in itemDefinitionIds)
        {
            var itemDefinition = await _itemDefinitionRepository.GetByIdAsync(itemDefinitionId, cancellationToken);
            if (itemDefinition is null)
            {
                continue;
            }

            result[itemDefinitionId] = itemDefinition.Code;
        }

        return result;
    }

    private static string? ResolveItemCode(Guid? itemDefinitionId, IReadOnlyDictionary<Guid, string> itemDefinitionMap)
    {
        if (!itemDefinitionId.HasValue)
        {
            return null;
        }

        return itemDefinitionMap.TryGetValue(itemDefinitionId.Value, out var itemCode)
            ? itemCode
            : null;
    }
}
