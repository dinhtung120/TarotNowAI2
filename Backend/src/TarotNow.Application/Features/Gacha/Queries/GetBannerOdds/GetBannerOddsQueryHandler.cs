using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetBannerOdds;

public class GetBannerOddsQueryHandler : IRequestHandler<GetBannerOddsQuery, GachaBannerOddsDto>
{
    private readonly IGachaRepository _gachaRepository;

    public GetBannerOddsQueryHandler(IGachaRepository gachaRepository)
    {
        _gachaRepository = gachaRepository;
    }

    public async Task<GachaBannerOddsDto> Handle(GetBannerOddsQuery request, CancellationToken cancellationToken)
    {
        var banner = await _gachaRepository.GetActiveBannerAsync(request.BannerCode, cancellationToken);
        if (banner == null)
            throw new NotFoundException($"Banner with code {request.BannerCode} not found or inactive.");

        var items = await _gachaRepository.GetBannerItemsAsync(banner.Id, cancellationToken);

        return new GachaBannerOddsDto
        {
            BannerCode = banner.Code,
            OddsVersion = banner.OddsVersion,
            Items = items.Select(i => new GachaBannerItemDto
            {
                Rarity = i.Rarity,
                RewardType = i.RewardType,
                RewardValue = i.RewardValue,
                WeightBasisPoints = i.WeightBasisPoints,
                DisplayNameVi = i.DisplayNameVi,
                DisplayNameEn = i.DisplayNameEn,
                DisplayIcon = i.DisplayIcon
            }).ToList()
        };
    }
}
