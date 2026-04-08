using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository thao tác định nghĩa achievement và trạng thái unlock của user trên Mongo.
public class MongoAchievementRepository : IAchievementRepository
{
    // Mongo context truy cập collections achievements và user_achievements.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository achievement.
    /// Luồng xử lý: nhận MongoDbContext từ DI để dùng chung cấu hình kết nối/index.
    /// </summary>
    public MongoAchievementRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy toàn bộ achievement definitions.
    /// Luồng xử lý: đọc tất cả documents rồi map sang DTO phục vụ tầng Application.
    /// </summary>
    public async Task<List<AchievementDefinitionDto>> GetAllAchievementsAsync(CancellationToken ct)
    {
        var docs = await _context.Achievements.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    /// <summary>
    /// Lấy achievement definition theo mã code.
    /// Luồng xử lý: query theo code và map DTO nếu tồn tại.
    /// </summary>
    public async Task<AchievementDefinitionDto?> GetByCodeAsync(string achievementCode, CancellationToken ct)
    {
        var doc = await _context.Achievements.Find(a => a.Code == achievementCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

    /// <summary>
    /// Upsert định nghĩa achievement.
    /// Luồng xử lý: update theo code, nếu chưa có thì insert mới và gán CreatedAt tại lần tạo đầu.
    /// </summary>
    public async Task UpsertAchievementDefinitionAsync(AchievementDefinitionDto achievement, CancellationToken ct)
    {
        var filter = Builders<AchievementDefinitionDocument>.Filter.Eq(a => a.Code, achievement.Code);
        var update = Builders<AchievementDefinitionDocument>.Update
            .Set(a => a.TitleVi, achievement.TitleVi)
            .Set(a => a.TitleEn, achievement.TitleEn)
            .Set(a => a.TitleZh, achievement.TitleZh)
            .Set(a => a.DescriptionVi, achievement.DescriptionVi)
            .Set(a => a.DescriptionEn, achievement.DescriptionEn)
            .Set(a => a.DescriptionZh, achievement.DescriptionZh)
            .Set(a => a.Icon, achievement.Icon)
            .Set(a => a.GrantsTitleCode, achievement.GrantsTitleCode)
            .Set(a => a.IsActive, achievement.IsActive)
            .SetOnInsert(a => a.CreatedAt, DateTime.UtcNow);
        // SetOnInsert giữ nguyên mốc tạo cũ khi update lại achievement hiện hữu.

        await _context.Achievements.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    /// <summary>
    /// Xóa definition achievement theo code.
    /// Luồng xử lý: delete trực tiếp một bản ghi khớp code.
    /// </summary>
    public async Task DeleteAchievementDefinitionAsync(string achievementCode, CancellationToken ct)
    {
        await _context.Achievements.DeleteOneAsync(a => a.Code == achievementCode, ct);
    }

    /// <summary>
    /// Lấy danh sách achievement đã unlock của user.
    /// Luồng xử lý: lọc theo userId và map về DTO rút gọn.
    /// </summary>
    public async Task<List<UserAchievementDto>> GetUserAchievementsAsync(Guid userId, CancellationToken ct)
    {
        var filter = Builders<UserAchievementDocument>.Filter.Eq(a => a.UserId, userId);
        var docs = await _context.UserAchievements.Find(filter).ToListAsync(ct);

        return docs.Select(d => new UserAchievementDto
        {
            AchievementCode = d.AchievementCode,
            UnlockedAt = d.UnlockedAt
        }).ToList();
    }

    /// <summary>
    /// Kiểm tra user đã unlock achievement cụ thể hay chưa.
    /// Luồng xử lý: đếm documents theo cặp userId-achievementCode và trả true nếu lớn hơn 0.
    /// </summary>
    public async Task<bool> HasUnlockedAsync(Guid userId, string achievementCode, CancellationToken ct)
    {
        var filter = Builders<UserAchievementDocument>.Filter.Eq(a => a.UserId, userId)
                     & Builders<UserAchievementDocument>.Filter.Eq(a => a.AchievementCode, achievementCode);

        var count = await _context.UserAchievements.CountDocumentsAsync(filter, cancellationToken: ct);
        return count > 0;
    }

    /// <summary>
    /// Ghi nhận user unlock achievement.
    /// Luồng xử lý: insert document mới; nếu trùng unique key thì bỏ qua vì xem như đã unlock trước đó.
    /// </summary>
    public async Task UnlockAsync(Guid userId, string achievementCode, CancellationToken ct)
    {
        try
        {
            var doc = new UserAchievementDocument
            {
                UserId = userId,
                AchievementCode = achievementCode,
                UnlockedAt = DateTime.UtcNow
            };
            await _context.UserAchievements.InsertOneAsync(doc, cancellationToken: ct);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Edge case idempotency: unlock lặp lại do retry thì coi là thành công logic và không ném lỗi.
        }
    }

    /// <summary>
    /// Map document định nghĩa achievement sang DTO.
    /// Luồng xử lý: chuyển toàn bộ trường hiển thị và cấu hình active/title grant.
    /// </summary>
    private AchievementDefinitionDto MapDefinition(AchievementDefinitionDocument doc)
    {
        return new AchievementDefinitionDto
        {
            Code = doc.Code,
            TitleVi = doc.TitleVi,
            TitleEn = doc.TitleEn,
            TitleZh = doc.TitleZh,
            DescriptionVi = doc.DescriptionVi,
            DescriptionEn = doc.DescriptionEn,
            DescriptionZh = doc.DescriptionZh,
            Icon = doc.Icon,
            GrantsTitleCode = doc.GrantsTitleCode,
            IsActive = doc.IsActive
        };
    }
}
