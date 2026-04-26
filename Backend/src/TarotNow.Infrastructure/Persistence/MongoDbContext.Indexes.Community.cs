

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho các collection community.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho toàn bộ cụm community.
    /// Luồng xử lý: tách theo post, reaction, comment để áp dụng đúng truy vấn từng collection.
    /// </summary>
    private void EnsureCommunityIndexes()
    {
        EnsureCommunityPostIndexes();
        EnsureCommunityReactionIndexes();
        EnsureCommunityCommentIndexes();
    }

    /// <summary>
    /// Tạo index cho community_posts.
    /// Luồng xử lý: tối ưu feed công khai theo thời gian, feed theo tác giả và lọc visibility.
    /// </summary>
    private void EnsureCommunityPostIndexes()
    {
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Descending(x => x.CreatedAt)
                .Ascending(x => x.IsDeleted),
            new CreateIndexOptions { Name = "idx_createdat_isdeleted" }));
        // Feed mặc định ưu tiên bài mới và loại bản ghi đã soft-delete.

        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));
        // Hồ sơ tác giả cần truy vấn nhanh các bài theo thời gian đăng.

        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.Visibility)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_visibility_isdeleted_createdat" }));
        // Kết hợp visibility để hỗ trợ phân quyền bài public/private trong feed.

        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.MediaAttachStatus)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_media_attach_status_createdat" }));
        // Hỗ trợ monitor/reconcile các post còn pending/failed media attach.
    }

    /// <summary>
    /// Tạo index cho community_reactions.
    /// Luồng xử lý: chặn duplicate reaction theo user-post và tối ưu thống kê reaction theo bài.
    /// </summary>
    private void EnsureCommunityReactionIndexes()
    {
        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_postid_userid_unique" }));
        // Business rule: một user chỉ được một reaction hiệu lực trên một bài tại một thời điểm.

        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_createdat" }));
        // Hữu ích khi render danh sách reaction gần đây của một bài.
    }

    /// <summary>
    /// Tạo index cho community_comments.
    /// Luồng xử lý: tối ưu lấy comment theo post và tra lịch sử comment theo tác giả.
    /// </summary>
    private void EnsureCommunityCommentIndexes()
    {
        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_isdeleted_createdat" }));
        // Phù hợp luồng tải comment của bài với lọc soft-delete.

        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));
        // Cần cho trang hồ sơ và moderation theo tác giả.

        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.MediaAttachStatus)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_comment_media_attach_status_createdat" }));
        // Hỗ trợ monitor/reconcile comment còn pending/failed media attach.
    }
}
