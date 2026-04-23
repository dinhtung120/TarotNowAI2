using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository thao tác bảng system_configs.
/// </summary>
public sealed class SystemConfigRepository : ISystemConfigRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SystemConfigRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SystemConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SystemConfigs
            .AsNoTracking()
            .OrderBy(x => x.Key)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var normalizedKey = SystemConfigRegistry.NormalizeKey(key);
        return await _dbContext.SystemConfigs
            .FirstOrDefaultAsync(x => x.Key == normalizedKey, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SystemConfig> UpsertAsync(
        string key,
        string value,
        string valueKind,
        string? description,
        Guid? updatedBy,
        CancellationToken cancellationToken = default)
    {
        var normalizedKey = SystemConfigRegistry.NormalizeKey(key);
        var normalizedKind = SystemConfigRegistry.NormalizeValueKind(valueKind);

        var entity = await _dbContext.SystemConfigs
            .FirstOrDefaultAsync(x => x.Key == normalizedKey, cancellationToken);

        if (entity is null)
        {
            entity = new SystemConfig(normalizedKey, value, normalizedKind, description, updatedBy);
            _dbContext.SystemConfigs.Add(entity);
        }
        else
        {
            entity.Update(value, normalizedKind, description, updatedBy);
            _dbContext.SystemConfigs.Update(entity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }
}
