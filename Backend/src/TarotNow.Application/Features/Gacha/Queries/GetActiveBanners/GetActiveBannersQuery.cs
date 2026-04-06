using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gacha.Queries.GetActiveBanners;

public class GetActiveBannersQuery : IRequest<List<GachaBannerDto>>
{
    public System.Guid? UserId { get; set; }
}
