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

public class MongoAchievementRepository : IAchievementRepository
{
    private readonly MongoDbContext _context;

    public MongoAchievementRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<AchievementDefinitionDto>> GetAllAchievementsAsync(CancellationToken ct)
    {
        var docs = await _context.Achievements.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    public async Task<AchievementDefinitionDto?> GetByCodeAsync(string achievementCode, CancellationToken ct)
    {
        var doc = await _context.Achievements.Find(a => a.Code == achievementCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

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

        await _context.Achievements.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task DeleteAchievementDefinitionAsync(string achievementCode, CancellationToken ct)
    {
        await _context.Achievements.DeleteOneAsync(a => a.Code == achievementCode, ct);
    }

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

    public async Task<bool> HasUnlockedAsync(Guid userId, string achievementCode, CancellationToken ct)
    {
        var filter = Builders<UserAchievementDocument>.Filter.Eq(a => a.UserId, userId)
                     & Builders<UserAchievementDocument>.Filter.Eq(a => a.AchievementCode, achievementCode);

        var count = await _context.UserAchievements.CountDocumentsAsync(filter, cancellationToken: ct);
        return count > 0;
    }

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
            
        }
    }

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
