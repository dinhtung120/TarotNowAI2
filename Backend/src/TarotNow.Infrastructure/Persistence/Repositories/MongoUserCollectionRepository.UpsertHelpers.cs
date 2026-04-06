using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoUserCollectionRepository
{
    private static FilterDefinition<UserCollectionDocument> BuildUserCardFilter(Guid userId, int cardId)
    {
        var userIdString = userId.ToString();
        return Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdString)
               & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);
    }

    private static PipelineUpdateDefinition<UserCollectionDocument> BuildUpsertUpdate(
        Guid userId,
        int cardId,
        long expToGain,
        DateTime now)
    {
        var userIdString = userId.ToString();
        var levelExpression = BuildLevelExpression();
        var setDocument = BuildSetDocument(userIdString, cardId, expToGain, now, levelExpression);
        var pipeline = PipelineDefinition<UserCollectionDocument, UserCollectionDocument>.Create(
            new[] { new BsonDocument("$set", setDocument) });

        return new PipelineUpdateDefinition<UserCollectionDocument>(pipeline);
    }

    private static BsonDocument BuildSetDocument(
        string userId,
        int cardId,
        long expToGain,
        DateTime now,
        BsonDocument levelExpression)
    {
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
            { "stats.times_drawn_upright", new BsonDocument("$add", new BsonArray
                {
                    new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
                    1
                })
            },
            { "level", levelExpression }
        };
    }

    private static BsonDocument BuildLevelExpression()
    {
        return new BsonDocument("$add", new BsonArray
        {
            1,
            new BsonDocument("$floor", new BsonDocument("$divide", new BsonArray { BuildTotalDrawsExpression(), 5 }))
        });
    }

    private static BsonDocument BuildTotalDrawsExpression()
    {
        return new BsonDocument("$add", new BsonArray
        {
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_reversed", 0 }),
            1
        });
    }

    private static UserCollection MapToEntity(UserCollectionDocument doc)
    {
        Guid.TryParse(doc.UserId, out var userId);
        var totalDraws = doc.Stats.TimesDrawnUpright + doc.Stats.TimesDrawnReversed;
        var copies = Math.Max(totalDraws, 1);
        var level = Math.Max(doc.Level, 1);
        var exp = Math.Max(doc.Exp, 0);
        var lastDrawnAt = doc.LastDrawnAt == default ? doc.UpdatedAt : doc.LastDrawnAt;

        /*
         * ÁNH XẠ (MAPPING) DỮ LIỆU TỪ MONGODB DOCUMENT SANG DOMAIN ENTITY.
         * Ở đây ta sử dụng UserCollectionSnapshot để giữ tính linh hoạt và bảo vệ các Domain Rule
         * trong constructor của thực thể UserCollection.
         * Lỗi hiển thị ATK/DEF = 0 do trước đây thiếu việc map 2 thuộc tính Atk và Def này.
         */
        return UserCollection.Rehydrate(new UserCollectionSnapshot
        {
            UserId = userId,
            CardId = doc.CardId,
            Level = level,
            Copies = copies,
            ExpGained = exp,
            LastDrawnAt = lastDrawnAt,
            Atk = doc.Atk, // Map chỉ số tấn công từ MongoDB Document sang Domain Entity Snapshot.
            Def = doc.Def  // Map chỉ số phòng thủ từ MongoDB Document sang Domain Entity Snapshot.
        });
    }
}
