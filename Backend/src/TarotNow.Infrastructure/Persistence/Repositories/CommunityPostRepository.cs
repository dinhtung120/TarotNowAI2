/*
 * ===================================================================
 * FILE: CommunityPostRepository.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Repositories
 * ===================================================================
 * MỤC ĐÍCH:
 *   Triển khai chi tiết Interface ICommunityPostRepository.
 *   Xử lý các thao tác CRUD lên collection "community_posts".
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class CommunityPostRepository : ICommunityPostRepository
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
        if (!ObjectId.TryParse(postId, out var objId)) return null;

        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var doc = await _context.CommunityPosts.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (doc == null) return null;
        return MapToDto(doc);
    }

    public async Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page, int pageSize, string? viewerId = null, string? authorId = null, string? visibility = null, CancellationToken cancellationToken = default)
    {
        var builder = Builders<CommunityPostDocument>.Filter;
        var filter = builder.Eq(x => x.IsDeleted, false);

        if (!string.IsNullOrEmpty(authorId))
        {
            filter &= builder.Eq(x => x.AuthorId, authorId);
        }

        if (!string.IsNullOrEmpty(visibility))
        {
            filter &= builder.Eq(x => x.Visibility, visibility);
            
            // Bảo mật: Nếu xin list bài Private nhưng không phải tác giả -> Từ chối (hoặc filter chặt chẽ)
            if (visibility == PostVisibility.Private && !string.IsNullOrEmpty(viewerId))
            {
                filter &= builder.Eq(x => x.AuthorId, viewerId);
            }
            else if (visibility == PostVisibility.Private && string.IsNullOrEmpty(viewerId))
            {
                // Unauthenticated user requesting Private feed? Block it.
                filter &= builder.Eq(x => x.AuthorId, "BLOCK_UNAUTHENTICATED");
            }
        }
        else 
        {
            // Mặc định Feed chung
            // Hiển thị bài Public, VÀ cả bài Private CỦA CHÍNH MÌNH
            var publicFilter = builder.Eq(x => x.Visibility, PostVisibility.Public);
            if (!string.IsNullOrEmpty(viewerId))
            {
                var myPrivateFilter = builder.And(
                    builder.Eq(x => x.Visibility, PostVisibility.Private),
                    builder.Eq(x => x.AuthorId, viewerId)
                );
                filter &= builder.Or(publicFilter, myPrivateFilter);
            }
            else
            {
                filter &= publicFilter;
            }
        }

        var total = await _context.CommunityPosts.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.CommunityPosts.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(MapToDto), total);
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

    public async Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        
        // Mongo hỗ trợ Inc cho property trong dictionary (lưu dưới dạng object JSON)
        var incReactionsCount = Builders<CommunityPostDocument>.Update.Inc($"reactions_count.{reactionType}", delta);
        var incTotal = Builders<CommunityPostDocument>.Update.Inc(x => x.TotalReactions, delta);
        
        var update = Builders<CommunityPostDocument>.Update.Combine(incReactionsCount, incTotal);

        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update.Inc(x => x.CommentsCount, delta);
        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private CommunityPostDto MapToDto(CommunityPostDocument doc)
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
