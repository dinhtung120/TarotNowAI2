using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetBannerOdds;

public class GetBannerOddsQuery : IRequest<GachaBannerOddsDto>
{
    public string BannerCode { get; set; } = string.Empty;
    
    public GetBannerOddsQuery(string bannerCode)
    {
        BannerCode = bannerCode;
    }
}
