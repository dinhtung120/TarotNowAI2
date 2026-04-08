using AutoMapper;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Community.Mappings;

// Cấu hình mapping DTO cho luồng hiển thị community post.
public sealed class CommunityPostMappingsProfile : AutoMapper.Profile
{
    /// <summary>
    /// Khởi tạo cấu hình mapping CommunityPostDto sang CommunityPostFeedItemDto.
    /// Luồng xử lý: map các trường đồng tên và giữ ViewerReaction để enrich riêng theo user đang xem.
    /// </summary>
    public CommunityPostMappingsProfile()
    {
        CreateMap<CommunityPostDto, CommunityPostFeedItemDto>()
            .ForMember(dest => dest.ViewerReaction, opt => opt.Ignore());
    }
}
