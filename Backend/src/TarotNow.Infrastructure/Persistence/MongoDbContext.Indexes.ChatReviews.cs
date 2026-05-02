using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    /// <summary>
    /// Tạo index cho collection conversation_reviews.
    /// Luồng xử lý: enforce mỗi user chỉ được review một lần trên một conversation, đồng thời tối ưu truy vấn theo reader.
    /// </summary>
    private void EnsureConversationReviewIndexes()
    {
        SafeCreateIndex(ConversationReviews, new CreateIndexModel<ConversationReviewDocument>(
            Builders<ConversationReviewDocument>.IndexKeys
                .Ascending(r => r.ConversationId)
                .Ascending(r => r.UserId),
            new CreateIndexOptions
            {
                Name = "ux_conversationid_userid",
                Unique = true
            }));

        SafeCreateIndex(ConversationReviews, new CreateIndexModel<ConversationReviewDocument>(
            Builders<ConversationReviewDocument>.IndexKeys
                .Ascending(r => r.ReaderId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_readerid_createdat_desc" }));
    }
}
