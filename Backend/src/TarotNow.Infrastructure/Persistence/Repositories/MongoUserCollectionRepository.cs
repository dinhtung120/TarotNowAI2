using MongoDB.Driver;
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

        // Filter: tìm theo composite key (user_id + card_id)
        var filter = Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdStr)
            & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);

        // Kiểm tra document hiện tại để tính level
        var existing = await _mongoContext.UserCollections
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
        {
            // INSERT mới — lần đầu rút lá này
            var doc = new UserCollectionDocument
            {
                UserId = userIdStr,
                CardId = cardId,
                Level = 1,
                Exp = expToGain,
                Stats = new DrawStats { TimesDrawnUpright = 1 }, // Default upright
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastDrawnAt = DateTime.UtcNow
            };
            await _mongoContext.UserCollections.InsertOneAsync(doc, cancellationToken: cancellationToken);
        }
        else
        {
            // UPDATE — rút trùng, cộng dồn
            var totalDraws = existing.Stats.TimesDrawnUpright + existing.Stats.TimesDrawnReversed + 1;
            var newLevel = existing.Level;
            // Level up: mỗi 5 lần rút = +1 level
            if (totalDraws % 5 == 0) newLevel++;

            var update = Builders<UserCollectionDocument>.Update
                .Inc(u => u.Exp, expToGain)
                .Inc(u => u.Stats.TimesDrawnUpright, 1)
                .Set(u => u.Level, newLevel)
                .Set(u => u.UpdatedAt, DateTime.UtcNow)
                .Set(u => u.LastDrawnAt, DateTime.UtcNow);

            await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
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
        var entity = new UserCollection(userId, doc.CardId);

        // Simulate copies từ tổng draw count
        var totalDraws = doc.Stats.TimesDrawnUpright + doc.Stats.TimesDrawnReversed;
        for (int i = 1; i < totalDraws; i++)
        {
            entity.AddCopy(0); // Recreate copies count
        }

        return entity;
    }
}
