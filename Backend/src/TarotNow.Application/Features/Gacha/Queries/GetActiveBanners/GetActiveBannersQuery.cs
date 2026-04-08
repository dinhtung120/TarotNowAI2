using MediatR;
using System.Collections.Generic;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetActiveBanners;

// Query lấy danh sách banner gacha đang active.
public class GetActiveBannersQuery : IRequest<List<GachaBannerDto>>
{
    // UserId tùy chọn để enrich pity count theo từng banner.
    public System.Guid? UserId { get; set; }
}
