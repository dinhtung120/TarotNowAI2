

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính quản lý bộ sưu tập thẻ bài của user.
public partial class MongoUserCollectionRepository : IUserCollectionRepository
{
    // Mongo context truy cập collection user_collections.
    private readonly MongoDbContext _mongoContext;
    // RNG service dùng cho roll chỉ số khi level up.
    private readonly IRngService _rngService;

    /// <summary>
    /// Khởi tạo repository user collection.
    /// Luồng xử lý: nhận MongoDbContext và RNG service từ DI.
    /// </summary>
    public MongoUserCollectionRepository(MongoDbContext mongoContext, IRngService rngService)
    {
        _mongoContext = mongoContext;
        _rngService = rngService;
    }

    /// <summary>
    /// Upsert thẻ bài cho user khi draw/nhận exp.
    /// Luồng xử lý: lấy level trước upsert, chạy pipeline upsert, rồi áp dụng random stats nếu có level tăng.
    /// </summary>
    public async Task UpsertCardAsync(
        Guid userId,
        int cardId,
        decimal expToGain,
        string orientation = CardOrientation.Upright,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = BuildUserCardFilter(userId, cardId);
        var normalizedOrientation = ReadingDrawnCardCodec.NormalizeOrientation(orientation);

        var existingDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
        int docBeforeUpsertLevel = existingDoc?.Level ?? 0;
        // Cần snapshot level trước để xác định chính xác số cấp vừa tăng sau upsert.

        var update = BuildUpsertUpdate(userId, cardId, expToGain, normalizedOrientation, now);

        var (updateResult, newDoc) = await UpsertAndGetDocAsync(filter, update, cancellationToken);
        if (updateResult.IsAcknowledged && newDoc != null && newDoc.Level > docBeforeUpsertLevel)
        {
            await ApplyRandomStatsOnLevelUpAsync(newDoc, docBeforeUpsertLevel, cancellationToken);
            // Chỉ roll stats khi level thực sự tăng để tránh cộng chỉ số sai.
        }
    }

    /// <summary>
    /// Thực thi upsert và đọc lại document mới nhất.
    /// Luồng xử lý: UpdateOneAsync IsUpsert=true rồi query lại theo cùng filter.
    /// </summary>
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

    /// <summary>
    /// Áp dụng roll chỉ số ngẫu nhiên cho các cấp mới vừa đạt.
    /// Luồng xử lý: duyệt từng level mới, tính bonus atk/def, lưu lịch sử roll và cập nhật cộng dồn vào document.
    /// </summary>
    private async Task ApplyRandomStatsOnLevelUpAsync(UserCollectionDocument doc, int oldLevel, CancellationToken ct)
    {
        int levelsGained = doc.Level - oldLevel;
        if (levelsGained <= 0) return;
        // Edge case: không tăng cấp thì không thực hiện roll.

        decimal totalAtkBonus = 0m;
        decimal totalDefBonus = 0m;
        var rollRecords = new List<StatRollRecord>();

        for (int pLevel = oldLevel + 1; pLevel <= doc.Level; pLevel++)
        {
            var (min, max) = UserCollection.GetStatBonusRange(pLevel);
            // Mỗi cấp có range bonus riêng theo rule domain progression.

            var atkBonus = (decimal)Random.Shared.Next(min, max + 1);
            var defBonus = (decimal)Random.Shared.Next(min, max + 1);

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
        // Cập nhật atomically để giữ đồng bộ giữa chỉ số hiện tại và lịch sử roll.
    }

    /// <summary>
    /// Lấy bộ sưu tập thẻ của user.
    /// Luồng xử lý: lọc theo userId + chưa xóa, sort level giảm dần và map sang aggregate domain.
    /// </summary>
    public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var docs = await _mongoContext.UserCollections
            .Find(u => u.UserId == userIdStr && !u.IsDeleted)
            .SortByDescending(u => u.Level)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToEntity);
    }
}
