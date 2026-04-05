/*
 * ===================================================================
 * FILE: MongoDbContext.Indexes.Community.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence
 * ===================================================================
 * MỤC ĐÍCH:
 *   Đảm bảo các index cho khu vực Community được khởi tạo.
 *   - CommunityPosts: query theo thời gian, tác giả, visibility.
 *   - CommunityReactions: unique (post_id, user_id) tránh like trùng.
 *   - CommunityComments: query theo bài viết, tác giả.
 *   Sử dụng SafeCreateIndex để tránh lỗi xung đột tên index cũ.
 * ===================================================================
 */

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCommunityIndexes()
    {
        // -------------------------------------------------------------
        //  1. Indexes cho bài viết cộng đồng (CommunityPosts)
        // -------------------------------------------------------------

        // Index feed hiển thị chung: Theo thời gian mới nhất + Bỏ qua bài đã Xoá
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Descending(x => x.CreatedAt)
                .Ascending(x => x.IsDeleted),
            new CreateIndexOptions { Name = "idx_createdat_isdeleted" }));

        // Index tìm theo Tác giả (Trang cá nhân của một Reader/User)
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));

        // Index lọc bài public cho feed chính ngoài trang chủ
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.Visibility)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_visibility_isdeleted_createdat" }));

        // -------------------------------------------------------------
        //  2. Indexes cho biểu cảm cộng đồng (CommunityReactions)
        // -------------------------------------------------------------

        // Unique: Một người dùng chỉ được React 1 lần vào 1 bài viết
        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_postid_userid_unique" }));

        // Index để lấy danh sách biểu cảm cho 1 bài viết
        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_createdat" }));

        // -------------------------------------------------------------
        //  3. Indexes cho bình luận cộng đồng (CommunityComments)
        // -------------------------------------------------------------

        // Index chính: Lấy bình luận theo bài viết, bỏ qua đã xoá, sắp xếp theo thời gian
        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_isdeleted_createdat" }));

        // Index tra cứu theo tác giả (dành cho Admin moderation)
        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));
    }
}
