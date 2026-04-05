using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICommunityCommentRepository
{
    Task<CommunityCommentDto> AddCommentAsync(CommunityCommentDto comment, CancellationToken cancellationToken = default);
    Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> GetByPostIdAsync(string postId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<CommunityCommentDto?> GetByIdAsync(string commentId, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(string commentId, string deletedBy, CancellationToken cancellationToken = default);
}
