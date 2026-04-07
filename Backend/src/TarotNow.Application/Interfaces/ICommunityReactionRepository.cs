

using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICommunityReactionRepository
{
        Task<bool> AddOrIgnoreAsync(CommunityReactionDto reaction, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(string postId, string userId, CancellationToken cancellationToken = default);

        Task<CommunityReactionDto?> GetAsync(string postId, string userId, CancellationToken cancellationToken = default);

        Task<bool> UpdateTypeAsync(string postId, string userId, string newType, CancellationToken cancellationToken = default);

        Task<Dictionary<string, string>> GetUserReactionsForPostsAsync(string userId, IEnumerable<string> postIds, CancellationToken cancellationToken = default);
}
