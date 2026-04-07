using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

public interface IQuestRepository
{
    
    Task<List<QuestDefinitionDto>> GetActiveQuestsAsync(string questType, CancellationToken ct);
    Task<QuestDefinitionDto?> GetQuestByCodeAsync(string questCode, CancellationToken ct);
    Task<List<QuestDefinitionDto>> GetAllQuestsAsync(CancellationToken ct); 
    Task UpsertQuestDefinitionAsync(QuestDefinitionDto quest, CancellationToken ct);
    Task DeleteQuestDefinitionAsync(string questCode, CancellationToken ct);
    
    
    Task<QuestProgressDto?> GetProgressAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);
    Task<List<QuestProgressDto>> GetAllProgressAsync(Guid userId, string questType, string periodKey, CancellationToken ct);
    Task UpsertProgressAsync(QuestProgressUpsertRequest request, CancellationToken ct);
    Task MarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);
    Task<bool> TryMarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);
    Task RevertClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);
}

public sealed record QuestProgressUpsertRequest(
    Guid UserId,
    string QuestCode,
    string PeriodKey,
    int Target,
    int IncrementBy);
