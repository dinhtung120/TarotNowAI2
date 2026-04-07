using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

public interface IAchievementRepository
{
    
    Task<List<AchievementDefinitionDto>> GetAllAchievementsAsync(CancellationToken ct);
    Task<AchievementDefinitionDto?> GetByCodeAsync(string achievementCode, CancellationToken ct);
    Task UpsertAchievementDefinitionAsync(AchievementDefinitionDto achievement, CancellationToken ct);
    Task DeleteAchievementDefinitionAsync(string achievementCode, CancellationToken ct);
    
    
    Task<List<UserAchievementDto>> GetUserAchievementsAsync(Guid userId, CancellationToken ct);
    Task<bool> HasUnlockedAsync(Guid userId, string achievementCode, CancellationToken ct);
    Task UnlockAsync(Guid userId, string achievementCode, CancellationToken ct);
}
