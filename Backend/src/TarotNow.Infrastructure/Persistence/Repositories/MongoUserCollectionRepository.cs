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
        var now = DateTime.UtcNow;
        var filter = BuildUserCardFilter(userId, cardId);
        var update = BuildUpsertUpdate(userId, cardId, expToGain, now);

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

}
