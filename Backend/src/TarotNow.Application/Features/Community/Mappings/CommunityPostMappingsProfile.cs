using AutoMapper;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Community.Mappings;

/// <summary>
/// AutoMapper profile for Community feature DTO transformations.
/// </summary>
public sealed class CommunityPostMappingsProfile : AutoMapper.Profile
{
    public CommunityPostMappingsProfile()
    {
        CreateMap<CommunityPostDto, CommunityPostFeedItemDto>()
            .ForMember(dest => dest.ViewerReaction, opt => opt.Ignore());
    }
}
