using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository thao tác comment cộng đồng trên MongoDB.
public class CommunityCommentRepository : ICommunityCommentRepository
{
    // Mongo context truy cập collection community_comments.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository comment cộng đồng.
    /// Luồng xử lý: nhận MongoDbContext qua DI để dùng chung cấu hình collection/index hiện tại.
    /// </summary>
    public CommunityCommentRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới comment vào bài viết cộng đồng.
    /// Luồng xử lý: map DTO sang document, insert Mongo, sau đó trả DTO kèm id vừa phát sinh.
    /// </summary>
    public async Task<CommunityCommentDto> AddCommentAsync(CommunityCommentDto comment, CancellationToken cancellationToken = default)
    {
        var doc = new CommunityCommentDocument
        {
            PostId = comment.PostId,
            AuthorId = comment.AuthorId,
            AuthorDisplayName = comment.AuthorDisplayName,
            AuthorAvatarUrl = comment.AuthorAvatarUrl,
            Content = comment.Content,
            MediaAttachStatus = comment.MediaAttachStatus,
            MediaAttachLastError = comment.MediaAttachLastError,
            CreatedAt = comment.CreatedAt,
            IsDeleted = false
        };

        await _context.CommunityComments.InsertOneAsync(doc, new InsertOneOptions(), cancellationToken);
        comment.Id = doc.Id;
        // Gán ngược id Mongo để lớp trên có thể trả về client ngay trong cùng request.
        return comment;
    }

    /// <summary>
    /// Lấy danh sách comment theo bài viết có phân trang.
    /// Luồng xử lý: lọc theo post + chưa xóa, đếm tổng, lấy page theo created_at desc rồi map sang DTO.
    /// </summary>
    public async Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> GetByPostIdAsync(string postId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.PostId, postId)
                   & Builders<CommunityCommentDocument>.Filter.Eq(x => x.IsDeleted, false);
        // Luôn loại comment soft-delete để nhất quán với feed cộng đồng.

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
            MediaAttachStatus = doc.MediaAttachStatus,
            MediaAttachLastError = doc.MediaAttachLastError,
            CreatedAt = doc.CreatedAt
        });

        return (dtos, total);
    }

    /// <summary>
    /// Lấy một comment theo id.
    /// Luồng xử lý: validate định dạng ObjectId, query document và map DTO nếu tồn tại.
    /// </summary>
    public async Task<CommunityCommentDto?> GetByIdAsync(string commentId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(commentId, out _)) return null;
        // Edge case: id không đúng chuẩn ObjectId thì trả null ngay để tránh query vô nghĩa.

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
            MediaAttachStatus = doc.MediaAttachStatus,
            MediaAttachLastError = doc.MediaAttachLastError,
            CreatedAt = doc.CreatedAt
        };
    }

    /// <summary>
    /// Xóa mềm comment.
    /// Luồng xử lý: set cờ is_deleted và metadata deleted_at/deleted_by, trả về true nếu có bản ghi được cập nhật.
    /// </summary>
    public async Task<bool> SoftDeleteAsync(string commentId, string deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.Id, commentId);
        var update = Builders<CommunityCommentDocument>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedAt, DateTime.UtcNow)
            .Set(x => x.DeletedBy, deletedBy);

        var result = await _context.CommunityComments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
        // Chỉ báo thành công khi Mongo thực sự cập nhật được bản ghi đích.
    }

    /// <summary>
    /// Cập nhật trạng thái attach media cho bình luận.
    /// Luồng xử lý: validate status hợp lệ, rồi set trạng thái/lỗi theo comment id.
    /// </summary>
    public async Task UpdateMediaAttachStatusAsync(
        string commentId,
        string status,
        string? lastError = null,
        CancellationToken cancellationToken = default)
    {
        if (!MediaUploadConstants.IsCommunityEntityMediaAttachStatus(status))
        {
            throw new ArgumentException("Community media attach status không hợp lệ.", nameof(status));
        }

        var normalizedStatus = status.Trim().ToLowerInvariant();
        var filter = Builders<CommunityCommentDocument>.Filter.Eq(x => x.Id, commentId);
        var update = Builders<CommunityCommentDocument>.Update
            .Set(x => x.MediaAttachStatus, normalizedStatus)
            .Set(x => x.MediaAttachLastError, string.IsNullOrWhiteSpace(lastError) ? null : lastError.Trim());

        await _context.CommunityComments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}
