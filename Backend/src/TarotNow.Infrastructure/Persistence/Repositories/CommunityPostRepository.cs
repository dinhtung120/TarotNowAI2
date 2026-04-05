using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CommunityPostRepository : ICommunityPostRepository
{
    private readonly MongoDbContext _context;

    public CommunityPostRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityPostDto> CreateAsync(CommunityPostDto post, CancellationToken cancellationToken = default)
    {
        var doc = new CommunityPostDocument
        {
            AuthorId = post.AuthorId,
            AuthorDisplayName = post.AuthorDisplayName,
            AuthorAvatarUrl = post.AuthorAvatarUrl,
            Content = post.Content,
            Visibility = post.Visibility,
            CreatedAt = post.CreatedAt,
            ReactionsCount = post.ReactionsCount,
            TotalReactions = post.TotalReactions,
            IsDeleted = false
        };

        await _context.CommunityPosts.InsertOneAsync(doc, new InsertOneOptions(), cancellationToken);
        post.Id = doc.Id;
        return post;
    }

    public async Task<CommunityPostDto?> GetByIdAsync(string postId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(postId, out _))
        {
            return null;
        }

        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var doc = await _context.CommunityPosts.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : MapToDto(doc);
    }

    public async Task<bool> UpdateContentAsync(string postId, string content, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId)
                   & Builders<CommunityPostDocument>.Filter.Eq(x => x.IsDeleted, false);
        var update = Builders<CommunityPostDocument>.Update
            .Set(x => x.Content, content)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        var result = await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> SoftDeleteAsync(string postId, string deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedAt, DateTime.UtcNow)
            .Set(x => x.DeletedBy, deletedBy);
        var result = await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}
