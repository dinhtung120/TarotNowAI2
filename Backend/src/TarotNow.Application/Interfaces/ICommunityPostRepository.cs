

using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICommunityPostRepository
{
        Task<CommunityPostDto> CreateAsync(CommunityPostDto post, CancellationToken cancellationToken = default);

        Task<CommunityPostDto?> GetByIdAsync(string postId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page, int pageSize, string? viewerId = null, string? authorId = null, string? visibility = null, CancellationToken cancellationToken = default);

        Task<bool> UpdateContentAsync(string postId, string content, CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(string postId, string deletedBy, CancellationToken cancellationToken = default);

        Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default);

        Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default);
}
