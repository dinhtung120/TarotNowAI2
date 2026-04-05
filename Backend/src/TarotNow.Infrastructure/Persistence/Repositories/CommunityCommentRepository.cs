using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class CommunityCommentRepository : ICommunityCommentRepository
{
    private readonly MongoDbContext _context;

    public CommunityCommentRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityCommentDto> AddCommentAsync(CommunityCommentDto comment, CancellationToken cancellationToken = default)
    {
        var doc = new CommunityCommentDocument
        {
            PostId = comment.PostId,
            AuthorId = comment.AuthorId,
            AuthorDisplayName = comment.AuthorDisplayName,
            AuthorAvatarUrl = comment.AuthorAvatarUrl,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            IsDeleted = false
        };

        await _context.CommunityComments.InsertOneAsync(doc, new InsertOneOptions(), cancellationToken);
        comment.Id = doc.Id;
        return comment;
    }

    public async Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> GetByPostIdAsync(string postId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.PostId, postId) 
                   & Builders<CommunityCommentDocument>.Filter.Eq(x => x.IsDeleted, false);

        var total = await _context.CommunityComments.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.CommunityComments.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = docs.Select(doc => new CommunityCommentDto
        {
            Id = doc.Id,
            PostId = doc.PostId,
            AuthorId = doc.AuthorId,
            AuthorDisplayName = doc.AuthorDisplayName,
            AuthorAvatarUrl = doc.AuthorAvatarUrl,
            Content = doc.Content,
            CreatedAt = doc.CreatedAt
        });

        return (dtos, total);
    }

    public async Task<CommunityCommentDto?> GetByIdAsync(string commentId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(commentId, out var objId)) return null;

        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.Id, commentId);
        var doc = await _context.CommunityComments.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (doc == null) return null;
        
        return new CommunityCommentDto
        {
            Id = doc.Id,
            PostId = doc.PostId,
            AuthorId = doc.AuthorId,
            AuthorDisplayName = doc.AuthorDisplayName,
            AuthorAvatarUrl = doc.AuthorAvatarUrl,
            Content = doc.Content,
            CreatedAt = doc.CreatedAt
        };
    }

    public async Task<bool> SoftDeleteAsync(string commentId, string deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.Id, commentId);
        var update = Builders<CommunityCommentDocument>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedAt, DateTime.UtcNow)
            .Set(x => x.DeletedBy, deletedBy);

        var result = await _context.CommunityComments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}
