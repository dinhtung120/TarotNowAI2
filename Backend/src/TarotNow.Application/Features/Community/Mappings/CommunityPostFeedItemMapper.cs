using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Community.Mappings;

/// <summary>
/// Mapper thủ công thống nhất cho payload hiển thị community post.
/// </summary>
public static class CommunityPostFeedItemMapper
{
    /// <summary>
    /// Map CommunityPostDto sang CommunityPostFeedItemDto.
    /// Luồng xử lý: copy các trường hiển thị thuần, phần ViewerReaction được enrich ở caller.
    /// </summary>
    public static CommunityPostFeedItemDto Map(CommunityPostDto source)
    {
        return new CommunityPostFeedItemDto
        {
            Id = source.Id,
            AuthorId = source.AuthorId,
            AuthorDisplayName = source.AuthorDisplayName,
            AuthorAvatarUrl = source.AuthorAvatarUrl,
            Content = source.Content,
            Visibility = source.Visibility,
            ReactionsCount = source.ReactionsCount,
            TotalReactions = source.TotalReactions,
            CommentsCount = source.CommentsCount,
            IsDeleted = source.IsDeleted,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
            ViewerReaction = null
        };
    }
}
