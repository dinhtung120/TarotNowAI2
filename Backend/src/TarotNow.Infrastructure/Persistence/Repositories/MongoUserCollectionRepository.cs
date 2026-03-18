using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation cho IUserCollectionRepository.
///
/// THAY THẾ UserCollectionRepository (EF Core / PostgreSQL) trước đó.
/// Sử dụng MongoDB upsert pattern thay vì raw SQL INSERT ... ON CONFLICT.
///
/// Tại sao MongoDB phù hợp hơn PostgreSQL cho user_collections?
/// → Schema linh hoạt (stats, customization, ascension_tier are nested objects).
/// → Queries chủ yếu là per-user (shardable by user_id).
/// → Không cần JOIN — mỗi document là self-contained.
/// </summary>
public class MongoUserCollectionRepository : IUserCollectionRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoUserCollectionRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Upsert card: chưa có → insert level 1. Đã có → +EXP, +1 draw count, level up nếu đủ.
    ///
    /// Dùng MongoDB UpdateOneAsync với IsUpsert=true — atomic operation,
    /// không cần application-level lock hay transaction.
    ///
    /// Level up formula: mỗi 5 lần rút trùng = +1 level (giữ nguyên logic cũ).
    /// Lưu ý: $inc là atomic, tránh race condition khi 2 request cùng lúc.
    /// </summary>
    public async Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();
        var now = DateTime.UtcNow;

        // Upsert atomic để tránh race condition read-then-write.
        var filter = Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdStr)
            & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);

        var totalDrawsExpr = new BsonDocument("$add", new BsonArray
        {
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_reversed", 0 }),
            1
        });

        var levelExpr = new BsonDocument("$add", new BsonArray
        {
            1,
            new BsonDocument("$floor", new BsonDocument("$divide", new BsonArray { totalDrawsExpr, 5 }))
        });

        var updatePipeline = new[]
        {
            new BsonDocument("$set", new BsonDocument
            {
                { "user_id", userIdStr },
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
                { "level", levelExpr }
            })
        };
        var pipeline = PipelineDefinition<UserCollectionDocument, UserCollectionDocument>.Create(updatePipeline);
        var update = new PipelineUpdateDefinition<UserCollectionDocument>(pipeline);

        await _mongoContext.UserCollections.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    /// <summary>Lấy toàn bộ bộ sưu tập của user — sắp theo level giảm dần.</summary>
    public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var docs = await _mongoContext.UserCollections
            .Find(u => u.UserId == userIdStr && !u.IsDeleted)
            .SortByDescending(u => u.Level)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToEntity);
    }

    /// <summary>
    /// Map MongoDB document → Domain Entity.
    /// UserCollection entity cần userId (Guid) và cardId (int).
    /// </summary>
    private static UserCollection MapToEntity(UserCollectionDocument doc)
    {
        Guid.TryParse(doc.UserId, out var userId);
        var totalDraws = doc.Stats.TimesDrawnUpright + doc.Stats.TimesDrawnReversed;
        var copies = Math.Max(totalDraws, 1);
        var level = Math.Max(doc.Level, 1);
        var exp = Math.Max(doc.Exp, 0);
        var lastDrawnAt = doc.LastDrawnAt == default ? doc.UpdatedAt : doc.LastDrawnAt;

        return UserCollection.Rehydrate(
            userId: userId,
            cardId: doc.CardId,
            level: level,
            copies: copies,
            expGained: exp,
            lastDrawnAt: lastDrawnAt);
    }
}
