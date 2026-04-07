using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

public interface ITitleRepository
{
    Task<List<TitleDefinitionDto>> GetAllTitlesAsync(CancellationToken ct);
    Task<TitleDefinitionDto?> GetByCodeAsync(string titleCode, CancellationToken ct);
    Task<List<UserTitleDto>> GetUserTitlesAsync(Guid userId, CancellationToken ct);
    Task<bool> OwnsTitleAsync(Guid userId, string titleCode, CancellationToken ct);
    Task GrantTitleAsync(Guid userId, string titleCode, CancellationToken ct);
    
    
    Task UpsertTitleDefinitionAsync(TitleDefinitionDto title, CancellationToken ct);
    Task DeleteTitleDefinitionAsync(string titleCode, CancellationToken ct);
}
