/*
 * FILE: MongoUserCollectionRepository.cs
 * MỤC ĐÍCH: Repository quản lý bộ sưu tập lá bài từ MongoDB (collection "user_collections").
 *
 *   CÁC CHỨC NĂNG:
 *   → UpsertCardAsync: ATOMIC upsert — chưa có lá → tạo mới, đã có → tăng EXP + level up
 *   → GetUserCollectionAsync: lấy toàn bộ bộ sưu tập của User
 *
 *   TẠI SAO MONGODB PHÙ HỢP HƠN POSTGRESQL?
 *   → Schema linh hoạt (stats, customization, ascension_tier là nested objects)
 *   → Query chủ yếu per-user (shardable theo user_id)
 *   → Không cần JOIN — mỗi document self-contained
 *
 *   UPSERT PATTERN:
 *   Dùng MongoDB Pipeline Update ($set với biểu thức) + IsUpsert=true.
 *   Thay thế PostgreSQL INSERT ... ON CONFLICT (upsert) trước đó.
 *   Atomic: $inc tránh race condition khi 2 request cùng lúc rút trùng lá.
 */

using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IUserCollectionRepository — bộ sưu tập lá bài trong MongoDB.
/// </summary>
public class MongoUserCollectionRepository : IUserCollectionRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoUserCollectionRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// ATOMIC UPSERT: Thêm hoặc cập nhật lá bài trong bộ sưu tập.
    ///
    /// KHI CHƯA CÓ LÁ (insert):
    ///   → Tạo document mới: level=1, exp=expToGain, times_drawn_upright=1
    ///
    /// KHI ĐÃ CÓ LÁ (update):
    ///   → exp += expToGain (cộng dồn)
    ///   → times_drawn_upright += 1 (đếm lần rút)
    ///   → level = 1 + floor(totalDraws / 5) (tự tính level mới)
    ///
    /// TẠI SAO DÙNG PIPELINE UPDATE THAY VÌ $INC THƯỜNG?
    ///   → Cần tính level dựa trên totalDraws (biểu thức phụ thuộc nhiều trường).
    ///   → Pipeline update ($set với expressions) cho phép tính toán phức tạp trong 1 operation.
    ///   → Atomic: MongoDB đảm bảo không có race condition giữa read và write.
    /// </summary>
    public async Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();
        var now = DateTime.UtcNow;

        // Filter: tìm document theo userId + cardId (unique index đảm bảo tối đa 1 kết quả)
        var filter = Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdStr)
            & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);

        // ===== Biểu thức tính totalDraws =====
        // totalDraws = times_drawn_upright + times_drawn_reversed + 1 (lần rút hiện tại)
        // $ifNull: nếu trường chưa tồn tại (insert lần đầu) → mặc định 0
        var totalDrawsExpr = new BsonDocument("$add", new BsonArray
        {
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
            new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_reversed", 0 }),
            1
        });

        // ===== Biểu thức tính level =====
        // level = 1 + floor(totalDraws / 5)
        // Ví dụ: 1 lần rút → level 1, 5 lần → level 2, 10 lần → level 3
        var levelExpr = new BsonDocument("$add", new BsonArray
        {
            1,
            new BsonDocument("$floor", new BsonDocument("$divide", new BsonArray { totalDrawsExpr, 5 }))
        });

        // ===== Pipeline update: set tất cả trường trong 1 lệnh =====
        var updatePipeline = new[]
        {
            new BsonDocument("$set", new BsonDocument
            {
                { "user_id", userIdStr },
                { "card_id", cardId },
                { "is_deleted", false },
                // created_at: giữ nguyên nếu đã có, set now nếu insert mới
                { "created_at", new BsonDocument("$ifNull", new BsonArray { "$created_at", now }) },
                { "updated_at", now },
                { "last_drawn_at", now },
                // exp: cộng dồn (exp hiện tại + expToGain)
                { "exp", new BsonDocument("$add", new BsonArray
                    {
                        new BsonDocument("$ifNull", new BsonArray { "$exp", 0 }),
                        expToGain
                    })
                },
                // times_drawn_upright: +1 mỗi lần rút (mặc định upright)
                { "stats.times_drawn_upright", new BsonDocument("$add", new BsonArray
                    {
                        new BsonDocument("$ifNull", new BsonArray { "$stats.times_drawn_upright", 0 }),
                        1
                    })
                },
                // level: tính tự động từ totalDraws
                { "level", levelExpr }
            })
        };
        var pipeline = PipelineDefinition<UserCollectionDocument, UserCollectionDocument>.Create(updatePipeline);
        var update = new PipelineUpdateDefinition<UserCollectionDocument>(pipeline);

        // IsUpsert = true: chưa có → insert, đã có → update
        await _mongoContext.UserCollections.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    /// <summary>
    /// Lấy toàn bộ bộ sưu tập của User, sắp theo level giảm dần (lá mạnh nhất trước).
    /// Chỉ lấy chưa xóa (IsDeleted = false).
    /// </summary>
    public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var docs = await _mongoContext.UserCollections
            .Find(u => u.UserId == userIdStr && !u.IsDeleted)
            .SortByDescending(u => u.Level) // Lá level cao nhất hiện trước
            .ToListAsync(cancellationToken);

        return docs.Select(MapToEntity);
    }

    // ==================== MAPPING ====================

    /// <summary>
    /// Map MongoDB Document → Domain Entity.
    /// Parse userId (string → Guid), tính copies từ totalDraws, đảm bảo level ≥ 1.
    /// Dùng Entity.Rehydrate() factory method — reconstruct entity từ dữ liệu đã lưu.
    /// </summary>
    private static UserCollection MapToEntity(UserCollectionDocument doc)
    {
        Guid.TryParse(doc.UserId, out var userId);
        var totalDraws = doc.Stats.TimesDrawnUpright + doc.Stats.TimesDrawnReversed;
        var copies = Math.Max(totalDraws, 1);   // Tối thiểu 1 copy
        var level = Math.Max(doc.Level, 1);      // Tối thiểu level 1
        var exp = Math.Max(doc.Exp, 0);          // Không âm
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
