

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý reaction của người dùng trên bài viết cộng đồng.
public class CommunityReactionRepository : ICommunityReactionRepository
{
    // Mongo context truy cập collection community_reactions.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository reaction cộng đồng.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu Mongo.
    /// </summary>
    public CommunityReactionRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm reaction mới theo cơ chế idempotent.
    /// Luồng xử lý: upsert theo cặp postId-userId với SetOnInsert, trả true khi thực sự tạo mới.
    /// </summary>
    public async Task<bool> AddOrIgnoreAsync(CommunityReactionDto reaction, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, reaction.PostId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, reaction.UserId)
        );
        // Business rule: mỗi user chỉ có một reaction trên một bài.

        var update = Builders<CommunityReactionDocument>.Update
            .SetOnInsert(x => x.PostId, reaction.PostId)
            .SetOnInsert(x => x.UserId, reaction.UserId)
            .SetOnInsert(x => x.Type, reaction.Type)
            .SetOnInsert(x => x.CreatedAt, reaction.CreatedAt);

        var options = new UpdateOptions { IsUpsert = true };

        var result = await _context.CommunityReactions.UpdateOneAsync(filter, update, options, cancellationToken);
        // UpsertedId khác null nghĩa là bản ghi vừa được tạo, còn null là đã tồn tại và bị ignore.
        return result.UpsertedId != null;
    }

    /// <summary>
    /// Xóa reaction của user trên một bài viết.
    /// Luồng xử lý: delete theo cặp postId-userId và trả true nếu có bản ghi bị xóa.
    /// </summary>
    public async Task<bool> RemoveAsync(string postId, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, postId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId)
        );

        var result = await _context.CommunityReactions.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Lấy reaction hiện tại của user trên bài viết.
    /// Luồng xử lý: query theo postId-userId rồi map document sang DTO.
    /// </summary>
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

    /// <summary>
    /// Cập nhật loại reaction của user.
    /// Luồng xử lý: tìm bản ghi theo postId-userId và set type mới, trả true nếu có thay đổi.
    /// </summary>
    public async Task<bool> UpdateTypeAsync(string postId, string userId, string newType, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.PostId, postId),
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId)
        );

        var update = Builders<CommunityReactionDocument>.Update
            .Set(x => x.Type, newType);
        // Chỉ cập nhật type, không thay đổi CreatedAt để giữ mốc phản ứng ban đầu.

        var result = await _context.CommunityReactions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Lấy map reaction của một user cho nhiều bài viết.
    /// Luồng xử lý: chuẩn hóa danh sách postIds, query theo user+postIds, project tối thiểu và chuyển thành dictionary.
    /// </summary>
    public async Task<Dictionary<string, string>> GetUserReactionsForPostsAsync(string userId, IEnumerable<string> postIds, CancellationToken cancellationToken = default)
    {
        var postIdsList = postIds.Distinct().ToList();
        if (!postIdsList.Any()) return new Dictionary<string, string>();
        // Edge case: danh sách rỗng thì trả ngay để tránh query Mongo không cần thiết.

        var filter = Builders<CommunityReactionDocument>.Filter.And(
            Builders<CommunityReactionDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<CommunityReactionDocument>.Filter.In(x => x.PostId, postIdsList)
        );

        var projection = Builders<CommunityReactionDocument>.Projection
            .Include(x => x.PostId)
            .Include(x => x.Type);
        // Chỉ lấy hai trường cần dùng để giảm payload IO.

        var docs = await _context.CommunityReactions
            .Find(filter)
            .Project<CommunityReactionDocument>(projection)
            .ToListAsync(cancellationToken);

        return docs.ToDictionary(k => k.PostId, v => v.Type);
    }
}
