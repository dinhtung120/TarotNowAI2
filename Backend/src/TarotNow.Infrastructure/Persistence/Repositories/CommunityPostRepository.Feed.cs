using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý truy vấn feed bài viết cộng đồng.
public partial class CommunityPostRepository
{
    /// <summary>
    /// Lấy feed bài viết có phân trang và lọc theo ngữ cảnh người xem.
    /// Luồng xử lý: dựng filter tổng hợp, đếm tổng bản ghi, lấy page theo created_at desc rồi map DTO.
    /// </summary>
    public async Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page,
        int pageSize,
        string? viewerId = null,
        string? authorId = null,
        string? visibility = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildFeedFilter(viewerId, authorId, visibility);
        var skip = Math.Max(page - 1, 0) * pageSize;
        var total = await _context.CommunityPosts.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.CommunityPosts.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(MapToDto), total);
    }

    /// <summary>
    /// Dựng filter feed theo tác giả, visibility và quyền truy cập của người xem.
    /// Luồng xử lý: luôn loại bản ghi đã xóa mềm, sau đó áp lọc author và visibility tùy đầu vào.
    /// </summary>
    private static FilterDefinition<CommunityPostDocument> BuildFeedFilter(string? viewerId, string? authorId, string? visibility)
    {
        var builder = Builders<CommunityPostDocument>.Filter;
        var filter = builder.Eq(x => x.IsDeleted, false);
        // Mọi feed chỉ hiển thị bài chưa bị xóa mềm.

        if (!string.IsNullOrWhiteSpace(authorId))
        {
            filter &= builder.Eq(x => x.AuthorId, authorId);
            // Khi lọc theo tác giả, giới hạn phạm vi trước để giảm scan cho các bước sau.
        }

        if (!string.IsNullOrWhiteSpace(visibility))
        {
            return filter & BuildVisibilityFilter(builder, visibility, viewerId);
            // Có visibility cụ thể thì ưu tiên áp rule explicit theo yêu cầu caller.
        }

        return filter & BuildDefaultVisibilityFilter(builder, viewerId);
        // Không truyền visibility thì dùng rule mặc định: public + private của chính viewer.
    }

    /// <summary>
    /// Dựng filter visibility theo yêu cầu explicit.
    /// Luồng xử lý: visibility khác private thì filter trực tiếp; private thì ràng buộc quyền xem theo viewer.
    /// </summary>
    private static FilterDefinition<CommunityPostDocument> BuildVisibilityFilter(
        FilterDefinitionBuilder<CommunityPostDocument> builder,
        string visibility,
        string? viewerId)
    {
        if (!string.Equals(visibility, PostVisibility.Private, StringComparison.Ordinal))
        {
            return builder.Eq(x => x.Visibility, visibility);
        }

        if (string.IsNullOrWhiteSpace(viewerId))
        {
            return builder.And(
                builder.Eq(x => x.Visibility, PostVisibility.Private),
                builder.Eq(x => x.AuthorId, "BLOCK_UNAUTHENTICATED"));
            // Edge case: user chưa đăng nhập không được thấy private post nào, dùng điều kiện impossible để chặn tuyệt đối.
        }

        return builder.And(
            builder.Eq(x => x.Visibility, PostVisibility.Private),
            builder.Eq(x => x.AuthorId, viewerId));
        // Private post chỉ hiển thị cho chính tác giả.
    }

    /// <summary>
    /// Dựng filter visibility mặc định khi caller không truyền visibility.
    /// Luồng xử lý: khách ẩn danh chỉ thấy public; user đăng nhập thấy thêm private của chính họ.
    /// </summary>
    private static FilterDefinition<CommunityPostDocument> BuildDefaultVisibilityFilter(
        FilterDefinitionBuilder<CommunityPostDocument> builder,
        string? viewerId)
    {
        var publicFilter = builder.Eq(x => x.Visibility, PostVisibility.Public);
        if (string.IsNullOrWhiteSpace(viewerId))
        {
            return publicFilter;
            // Edge case: không có viewerId thì chỉ trả dữ liệu công khai.
        }

        var privateMine = builder.And(
            builder.Eq(x => x.Visibility, PostVisibility.Private),
            builder.Eq(x => x.AuthorId, viewerId));
        return builder.Or(publicFilter, privateMine);
        // Kết hợp OR để user vừa thấy feed công khai, vừa thấy bài private của chính mình.
    }
}
