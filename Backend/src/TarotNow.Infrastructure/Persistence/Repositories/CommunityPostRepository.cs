using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính quản lý CRUD bài viết cộng đồng.
public partial class CommunityPostRepository : ICommunityPostRepository
{
    // Mongo context truy cập collection community_posts.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository bài viết cộng đồng.
    /// Luồng xử lý: nhận MongoDbContext qua DI để dùng chung collection/index đã cấu hình.
    /// </summary>
    public CommunityPostRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tạo mới bài viết cộng đồng.
    /// Luồng xử lý: map DTO sang document, insert Mongo, rồi trả DTO có id vừa sinh.
    /// </summary>
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
        // Gán id phát sinh để caller trả response đồng bộ với dữ liệu đã lưu.
        return post;
    }

    /// <summary>
    /// Lấy bài viết theo id.
    /// Luồng xử lý: validate ObjectId trước, query document và map sang DTO nếu tồn tại.
    /// </summary>
    public async Task<CommunityPostDto?> GetByIdAsync(string postId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(postId, out _))
        {
            return null;
            // Edge case: id sai định dạng thì dừng sớm để tránh query không cần thiết.
        }

        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var doc = await _context.CommunityPosts.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>
    /// Cập nhật nội dung bài viết khi chưa bị xóa.
    /// Luồng xử lý: filter theo id + is_deleted=false, set content và updated_at, trả true nếu có bản ghi đổi.
    /// </summary>
    public async Task<bool> UpdateContentAsync(string postId, string content, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId)
                   & Builders<CommunityPostDocument>.Filter.Eq(x => x.IsDeleted, false);
        // Chặn chỉnh sửa trên bài đã xóa mềm để giữ tính toàn vẹn lịch sử moderation.

        var update = Builders<CommunityPostDocument>.Update
            .Set(x => x.Content, content)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        var result = await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Xóa mềm bài viết cộng đồng.
    /// Luồng xử lý: set cờ is_deleted và metadata xóa, trả true nếu update thành công.
    /// </summary>
    public async Task<bool> SoftDeleteAsync(string postId, string deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedAt, DateTime.UtcNow)
            .Set(x => x.DeletedBy, deletedBy);
        var result = await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
        // Dùng ModifiedCount để phản ánh chính xác có state-change hay không.
    }
}
