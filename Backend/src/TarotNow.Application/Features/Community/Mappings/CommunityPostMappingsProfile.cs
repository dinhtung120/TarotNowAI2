using AutoMapper;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Community.Mappings;

public sealed class CommunityPostMappingsProfile : AutoMapper.Profile
{
    public CommunityPostMappingsProfile()
    {
        CreateMap<CommunityPostDto, CommunityPostFeedItemDto>()
            .ForMember(dest => dest.ViewerReaction, opt => opt.Ignore());
    }
}
