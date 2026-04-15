using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository truy cập master data item definitions.
/// </summary>
public sealed class ItemDefinitionRepository : IItemDefinitionRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo ItemDefinitionRepository.
    /// </summary>
    public ItemDefinitionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<ItemDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToLowerInvariant();
        return _dbContext.Set<ItemDefinition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == normalizedCode, cancellationToken);
    }

    /// <inheritdoc />
    public Task<ItemDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<ItemDefinition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ItemDefinition>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ItemDefinition>()
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
    }
}
