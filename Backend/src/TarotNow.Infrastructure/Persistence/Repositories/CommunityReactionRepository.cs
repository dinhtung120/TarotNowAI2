

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class CommunityReactionRepository : ICommunityReactionRepository
{
    private readonly MongoDbContext _context;

    public CommunityReactionRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddOrIgnoreAsync(CommunityReactionDto reaction, CancellationToken cancellationToken = default)
    {
        
        
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, reaction.PostId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, reaction.UserId)
        );

        var update = Builders<CommunityReactionDocument>.Update
            .SetOnInsert(x => x.PostId, reaction.PostId)
            .SetOnInsert(x => x.UserId, reaction.UserId)
            .SetOnInsert(x => x.Type, reaction.Type)
            .SetOnInsert(x => x.CreatedAt, reaction.CreatedAt);

        var options = new UpdateOptions { IsUpsert = true };

        var result = await _context.CommunityReactions.UpdateOneAsync(filter, update, options, cancellationToken);
        
        
        
        return result.UpsertedId != null;
    }

    public async Task<bool> RemoveAsync(string postId, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, postId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId)
        );

        var result = await _context.CommunityReactions.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<CommunityReactionDto?> GetAsync(string postId, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, postId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId)
        );

        var doc = await _context.CommunityReactions.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (doc == null) return null;

        return new CommunityReactionDto
        {
            Id = doc.Id,
            PostId = doc.PostId,
            UserId = doc.UserId,
            Type = doc.Type,
            CreatedAt = doc.CreatedAt
        };
    }

    public async Task<bool> UpdateTypeAsync(string postId, string userId, string newType, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, postId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId)
        );

        var update = Builders<CommunityReactionDocument>.Update
            .Set(x => x.Type, newType); 

        var result = await _context.CommunityReactions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<Dictionary<string, string>> GetUserReactionsForPostsAsync(string userId, IEnumerable<string> postIds, CancellationToken cancellationToken = default)
    {
        var postIdsList = postIds.Distinct().ToList();
        if (!postIdsList.Any()) return new Dictionary<string, string>();

        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<CommunityReactionDocument>.Filter.In(x => x.PostId, postIdsList)
        );

        
        var projection = Builders<CommunityReactionDocument>.Projection
            .Include(x => x.PostId)
            .Include(x => x.Type);

        var docs = await _context.CommunityReactions
            .Find(filter)
            .Project<CommunityReactionDocument>(projection)
            .ToListAsync(cancellationToken);

        
        return docs.ToDictionary(k => k.PostId, v => v.Type);
    }
}
