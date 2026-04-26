using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using System.Linq;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho các collection chat (conversation/message/report).
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho toàn bộ cụm chat.
    /// Luồng xử lý: gọi lần lượt từng nhóm index conversation, message và report.
    /// </summary>
    private void EnsureChatCollectionIndexes()
    {
        EnsureConversationIndexes();
        EnsureChatMessageIndexes();
        EnsureReportIndexes();
    }

    /// <summary>
    /// Tạo index cho collection conversations.
    /// Luồng xử lý: tối ưu inbox theo user và reader với bộ lọc is_deleted + status + updated_at.
    /// </summary>
    private void EnsureConversationIndexes()
    {
        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.UserId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_userid_status_updatedat" }));
        // Index này phục vụ luồng user xem danh sách conversation theo trạng thái mới cập nhật.

        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.ReaderId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_readerid_status_updatedat" }));
        // Luồng reader dashboard dùng cùng pattern lọc nhưng theo reader_id.
    }

    /// <summary>
    /// Tạo index cho collection chat_messages.
    /// Luồng xử lý: tối ưu phân trang theo conversation và truy vấn lịch sử gửi theo sender.
    /// </summary>
    private void EnsureChatMessageIndexes()
    {
        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Ascending(m => m.IsDeleted)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_isdeleted_createdat_desc" }));
        // Tổ hợp này bám đúng điều kiện lấy message timeline trong hội thoại.

        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.SenderId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));
        // Hỗ trợ tra soát lịch sử gửi của một user trong các luồng moderation/audit.

        EnsureSystemEventUniquenessIndex();
    }

    private void EnsureSystemEventUniquenessIndex()
    {
        var systemEventFilter = new BsonDocument(
            "system_event_key",
            new BsonDocument
            {
                { "$type", "string" },
                { "$gt", string.Empty }
            });
        var model = new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Ascending(m => m.SystemEventKey),
            new CreateIndexOptions<ChatMessageDocument>
            {
                Name = "ux_conversationid_systemeventkey",
                Unique = true,
                PartialFilterExpression = systemEventFilter
            });

        try
        {
            SafeCreateIndex(ChatMessages, model);
        }
        catch (MongoCommandException ex) when (IsDuplicateKeyConflict(ex))
        {
            var normalizedCount = NormalizeDuplicateSystemEventKeys();
            _logger.LogWarning(
                ex,
                "[MongoDB] Found duplicate chat system_event_key records while creating '{IndexName}'. Normalized={NormalizedCount}, collection={Collection}. Retrying index creation.",
                model.Options.Name,
                normalizedCount,
                ChatMessages.CollectionNamespace.CollectionName);
            SafeCreateIndex(ChatMessages, model);
        }
        // Chặn tạo trùng system message khi outbox retry trên cùng business event.
    }

    private long NormalizeDuplicateSystemEventKeys()
    {
        long normalized = 0;
        var now = DateTime.UtcNow;
        foreach (var group in ListDuplicateSystemEventGroups())
        {
            normalized += NormalizeDuplicateSystemEventGroup(group, now);
        }

        return normalized;
    }

    private List<BsonDocument> ListDuplicateSystemEventGroups()
    {
        return ChatMessages.Aggregate<BsonDocument>(new[]
        {
            new BsonDocument("$match", new BsonDocument("$and", new BsonArray
            {
                new BsonDocument("system_event_key", new BsonDocument("$type", "string")),
                new BsonDocument("system_event_key", new BsonDocument("$ne", string.Empty))
            })),
            new BsonDocument("$sort", new BsonDocument
            {
                { "created_at", 1 },
                { "_id", 1 }
            }),
            new BsonDocument("$group", new BsonDocument
            {
                {
                    "_id", new BsonDocument
                    {
                        { "conversation_id", "$conversation_id" },
                        { "system_event_key", "$system_event_key" }
                    }
                },
                { "ids", new BsonDocument("$push", "$_id") },
                { "count", new BsonDocument("$sum", 1) }
            }),
            new BsonDocument("$match", new BsonDocument("count", new BsonDocument("$gt", 1)))
        }).ToList();
    }

    private long NormalizeDuplicateSystemEventGroup(BsonDocument group, DateTime now)
    {
        if (!group.TryGetValue("ids", out var idsValue) || idsValue is not BsonArray ids || ids.Count <= 1)
        {
            return 0;
        }

        var duplicatedIds = new BsonArray(ids.Skip(1));
        if (duplicatedIds.Count == 0)
        {
            return 0;
        }

        var result = ChatMessages.UpdateMany(
            new BsonDocument("_id", new BsonDocument("$in", duplicatedIds)),
            new BsonDocument("$set", new BsonDocument
            {
                { "system_event_key", BsonNull.Value },
                { "updated_at", now }
            }));
        return result.ModifiedCount;
    }

    /// <summary>
    /// Tạo index cho collection reports.
    /// Luồng xử lý: tối ưu hàng chờ xử lý report theo status và truy vấn report theo target object.
    /// </summary>
    private void EnsureReportIndexes()
    {
        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));
        // Admin thường lọc theo status và ưu tiên report mới nhất.

        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Target.Type)
                .Ascending(r => r.Target.Id)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_targettype_targetid_createdat_desc" }));
        // Cần index target để gom tất cả report của cùng post/comment/message khi điều tra.
    }
}
