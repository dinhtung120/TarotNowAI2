using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CommunityPostRepository
{
    public async Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update.Combine(
            Builders<CommunityPostDocument>.Update.Inc($"reactions_count.{reactionType}", delta),
            Builders<CommunityPostDocument>.Update.Inc(x => x.TotalReactions, delta));
        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update.Inc(x => x.CommentsCount, delta);
        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private static CommunityPostDto MapToDto(CommunityPostDocument doc)
    {
        return new CommunityPostDto
        {
            Id = doc.Id,
            AuthorId = doc.AuthorId,
            AuthorDisplayName = doc.AuthorDisplayName,
            AuthorAvatarUrl = doc.AuthorAvatarUrl,
            Content = doc.Content,
            Visibility = doc.Visibility,
            ReactionsCount = doc.ReactionsCount ?? new Dictionary<string, int>(),
            TotalReactions = doc.TotalReactions,
            CommentsCount = doc.CommentsCount,
            IsDeleted = doc.IsDeleted,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
