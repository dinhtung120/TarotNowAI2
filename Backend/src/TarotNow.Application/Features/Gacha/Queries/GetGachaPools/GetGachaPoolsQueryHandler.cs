using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetGachaPools;

/// <summary>
/// Handler truy vấn danh sách pool gacha.
/// </summary>
public sealed class GetGachaPoolsQueryHandler : IRequestHandler<GetGachaPoolsQuery, IReadOnlyList<GachaPoolDto>>
{
    private readonly IGachaPoolRepository _gachaPoolRepository;

    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public GetGachaPoolsQueryHandler(IGachaPoolRepository gachaPoolRepository)
    {
        _gachaPoolRepository = gachaPoolRepository;
    }

    /// <summary>
    /// Xử lý query lấy pool gacha.
    /// </summary>
    public async Task<IReadOnlyList<GachaPoolDto>> Handle(GetGachaPoolsQuery request, CancellationToken cancellationToken)
    {
        var pools = await _gachaPoolRepository.GetActivePoolsAsync(cancellationToken);
        if (pools.Count == 0)
        {
            return Array.Empty<GachaPoolDto>();
        }

        var result = new List<GachaPoolDto>(pools.Count);
        foreach (var pool in pools)
        {
            var pity = 0;
            if (request.UserId.HasValue)
            {
                pity = await _gachaPoolRepository.GetUserCurrentPityCountAsync(
                    request.UserId.Value,
                    pool.Id,
                    cancellationToken);
            }

            result.Add(new GachaPoolDto
            {
                Code = pool.Code,
                PoolType = pool.PoolType,
                NameVi = pool.NameVi,
                NameEn = pool.NameEn,
                NameZh = pool.NameZh,
                DescriptionVi = pool.DescriptionVi,
                DescriptionEn = pool.DescriptionEn,
                DescriptionZh = pool.DescriptionZh,
                CostCurrency = pool.CostCurrency,
                CostAmount = pool.CostAmount,
                OddsVersion = pool.OddsVersion,
                UserCurrentPity = pity,
                PityEnabled = pool.PityEnabled,
                HardPityCount = pool.HardPityCount,
            });
        }

        return result;
    }
}
