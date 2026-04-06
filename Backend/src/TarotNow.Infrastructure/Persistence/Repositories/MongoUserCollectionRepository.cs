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
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IUserCollectionRepository — bộ sưu tập lá bài trong MongoDB.
/// </summary>
public partial class MongoUserCollectionRepository : IUserCollectionRepository
{
    private readonly MongoDbContext _mongoContext;
    private readonly IRngService _rngService;

    public MongoUserCollectionRepository(MongoDbContext mongoContext, IRngService rngService)
    {
        _mongoContext = mongoContext;
        _rngService = rngService;
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
        var now = DateTime.UtcNow;
        var filter = BuildUserCardFilter(userId, cardId);

        // Đọc level cũ TRƯỚC KHI upsert để so sánh xem có lên level không
        var existingDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
        int docBeforeUpsertLevel = existingDoc?.Level ?? 0;

        var update = BuildUpsertUpdate(userId, cardId, expToGain, now);

        var (updateResult, newDoc) = await UpsertAndGetDocAsync(filter, update, cancellationToken);
        
        // --- GAMIFICATION PHASE 5.3: CARD LEVEL UP STATS ---
        // Nếu user vừa rút trùng lá bài và đủ điểm để lên level:
        // Cần xử lý hậu kỳ: tung RNG tính điểm ATK/DEF bonus, ghi vào lịch sử và update lại DB.
        if (updateResult.IsAcknowledged && newDoc != null && newDoc.Level > docBeforeUpsertLevel)
        {
            await ApplyRandomStatsOnLevelUpAsync(newDoc, docBeforeUpsertLevel, cancellationToken);
        }
    }

    private async Task<(UpdateResult, UserCollectionDocument?)> UpsertAndGetDocAsync(
        FilterDefinition<UserCollectionDocument> filter,
        PipelineUpdateDefinition<UserCollectionDocument> update,
        CancellationToken ct)
    {
        var result = await _mongoContext.UserCollections.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            ct);

        var newDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(ct);
        return (result, newDoc);
    }

    private async Task ApplyRandomStatsOnLevelUpAsync(UserCollectionDocument doc, int oldLevel, CancellationToken ct)
    {
        int levelsGained = doc.Level - oldLevel;
        if (levelsGained <= 0) return;

        int totalAtkBonus = 0;
        int totalDefBonus = 0;
        var rollRecords = new List<StatRollRecord>();

        // Xử lý bù level nếu vượt multiple levels
        for (int pLevel = oldLevel + 1; pLevel <= doc.Level; pLevel++)
        {
            var (min, max) = UserCollection.GetStatBonusRange(pLevel);
            // rngService.ShuffleDeck is for cards, but we can use System.Random internally for generic ints, 
            // but to be safe and fair, assuming IRngService will be expanded later, we just use Random here since IRngService currently only has Fisher-Yates array shuffle.
            // Wait, we can implement a simple Random here for now since IRngService is strictly for Deck.
            int atkBonus = Random.Shared.Next(min, max + 1); // +1 because upper bound is exclusive
            int defBonus = Random.Shared.Next(min, max + 1);

            totalAtkBonus += atkBonus;
            totalDefBonus += defBonus;

            rollRecords.Add(new StatRollRecord
            {
                Level = pLevel,
                AtkBonus = atkBonus,
                DefBonus = defBonus,
                RolledAt = DateTime.UtcNow
            });
        }

        // Cập nhật lại database với points và stat_history
        var filter = Builders<UserCollectionDocument>.Filter.Eq(u => u.Id, doc.Id);
        var update = Builders<UserCollectionDocument>.Update
            .Inc(u => u.Atk, totalAtkBonus)
            .Inc(u => u.Def, totalDefBonus);

        if (doc.StatHistory == null)
        {
            update = update.Set(u => u.StatHistory, rollRecords);
        }
        else
        {
            update = update.PushEach(u => u.StatHistory, rollRecords);
        }

        await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: ct);
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

}
