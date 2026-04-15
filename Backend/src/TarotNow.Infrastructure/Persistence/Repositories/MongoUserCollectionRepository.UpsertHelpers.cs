using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial helper dựng filter/update pipeline cho upsert user collection.
public partial class MongoUserCollectionRepository
{
    /// <summary>
    /// Dựng filter theo cặp user-card.
    /// Luồng xử lý: chuẩn hóa userId về string và kết hợp với cardId để định danh bản ghi duy nhất.
    /// </summary>
    private static FilterDefinition<UserCollectionDocument> BuildUserCardFilter(Guid userId, int cardId)
    {
        var userIdString = userId.ToString();
        return Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdString)
               & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);
    }

    /// <summary>
    /// Dựng pipeline update cho thao tác upsert card.
    /// Luồng xử lý: xây biểu thức level động, tạo $set document và bọc thành PipelineUpdateDefinition.
    /// </summary>
    private static PipelineUpdateDefinition<UserCollectionDocument> BuildUpsertUpdate(
        Guid userId,
        int cardId,
        long expToGain,
        string orientation,
        DateTime now)
    {
        var userIdString = userId.ToString();
        var levelExpression = BuildLevelExpression();
        var setDocument = BuildSetDocument(userIdString, cardId, expToGain, orientation, now, levelExpression);
        var pipeline = PipelineDefinition<UserCollectionDocument, UserCollectionDocument>.Create(
            new[] { new BsonDocument("$set", setDocument) });

        return new PipelineUpdateDefinition<UserCollectionDocument>(pipeline);
    }

    /// <summary>
    /// Tạo document $set cho pipeline upsert.
    /// Luồng xử lý: cập nhật metadata thời gian, cộng exp, tăng draw stats và tính level mới.
    /// </summary>
    private static BsonDocument BuildSetDocument(
        string userId,
        int cardId,
        long expToGain,
        string orientation,
        DateTime now,
        BsonDocument levelExpression)
    {
        var drawnStatField = orientation == CardOrientation.Reversed
            ? "stats.times_drawn_reversed"
            : "stats.times_drawn_upright";

        return new BsonDocument
        {
            { "user_id", userId },
            { "card_id", cardId },
            { "is_deleted", false },
            { "created_at", new BsonDocument("$ifNull", new BsonArray { "$created_at", now }) },
            { "updated_at", now },
            { "last_drawn_at", now },
            { "exp", new BsonDocument("$add", new BsonArray
                {
                    new BsonDocument("$ifNull", new BsonArray { "$exp", 0 }),
                    expToGain
                })
            },
            { drawnStatField, new BsonDocument("$add", new BsonArray
                {
                    new BsonDocument("$ifNull", new BsonArray { $"${drawnStatField}", 0 }),
                    1
                })
            },
            { "level", levelExpression }
        };
    }

    /// <summary>
    /// Tạo biểu thức tính level theo tổng số lần draw.
    /// Luồng xử lý: level = 1 + floor(total_draws / 5) theo rule progression hiện tại.
    /// </summary>
    private static BsonDocument BuildLevelExpression()
    {
        return new BsonDocument("$add", new BsonArray
        {
            1,
            new BsonDocument("$floor", new BsonDocument("$divide", new BsonArray { BuildTotalDrawsExpression(), 5 }))
        });
    }

    /// <summary>
    /// Tạo biểu thức tổng số lần draw.
    /// Luồng xử lý: cộng upright + reversed và cộng thêm lượt draw hiện tại trong cùng pipeline.
    /// </summary>
    private static BsonDocument BuildTotalDrawsExpression()
    {
        return new BsonDocument("$add", new BsonArray
        {
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_reversed", 0 }),
            1
        });
    }

    /// <summary>
    /// Map document user collection sang aggregate domain.
    /// Luồng xử lý: chuẩn hóa dữ liệu an toàn (level/exp/copies), sau đó rehydrate từ snapshot.
    /// </summary>
    private static UserCollection MapToEntity(UserCollectionDocument doc)
    {
        Guid.TryParse(doc.UserId, out var userId);
        var totalDraws = doc.Stats.TimesDrawnUpright + doc.Stats.TimesDrawnReversed;
        var copies = Math.Max(totalDraws, 1);
        var level = Math.Max(doc.Level, 1);
        var exp = Math.Max(doc.Exp, 0);
        var lastDrawnAt = doc.LastDrawnAt == default ? doc.UpdatedAt : doc.LastDrawnAt;

        return UserCollection.Rehydrate(new UserCollectionSnapshot
        {
            UserId = userId,
            CardId = doc.CardId,
            Level = level,
            Copies = copies,
            ExpGained = exp,
            LastDrawnAt = lastDrawnAt,
            Atk = doc.Atk,
            Def = doc.Def
        });
        // Fallback copies>=1 tránh vi phạm invariant domain khi dữ liệu cũ thiếu stats.
    }
}
