using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý item người dùng sở hữu và idempotency thao tác dùng item.
/// </summary>
public sealed partial class UserItemRepository : IUserItemRepository
{
    private const string UseOperationUniqueConstraint = "ix_inventory_item_use_operations_user_id_idempotency_key";

    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo UserItemRepository.
    /// </summary>
    public UserItemRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<UserItem?> GetByUserAndItemDefinitionIdAsync(
        Guid userId,
        Guid itemDefinitionId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<UserItem>()
            .Include(x => x.ItemDefinition)
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.ItemDefinitionId == itemDefinitionId,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserInventoryItemView>> GetUserInventoryAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserItem>()
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Quantity > 0)
            .Join(
                _dbContext.Set<ItemDefinition>().AsNoTracking().Where(x => x.IsActive),
                userItem => userItem.ItemDefinitionId,
                definition => definition.Id,
                (userItem, definition) => new UserInventoryItemView
                {
                    ItemDefinitionId = definition.Id,
                    ItemCode = definition.Code,
                    ItemType = definition.Type,
                    EnhancementType = definition.EnhancementType,
                    Rarity = definition.Rarity,
                    IsConsumable = definition.IsConsumable,
                    IsPermanent = definition.IsPermanent,
                    EffectValue = definition.EffectValue,
                    SuccessRatePercent = definition.SuccessRatePercent,
                    NameVi = definition.NameVi,
                    NameEn = definition.NameEn,
                    NameZh = definition.NameZh,
                    DescriptionVi = definition.DescriptionVi,
                    DescriptionEn = definition.DescriptionEn,
                    DescriptionZh = definition.DescriptionZh,
                    IconUrl = definition.IconUrl,
                    Quantity = userItem.Quantity,
                    AcquiredAtUtc = userItem.AcquiredAtUtc,
                })
            .OrderByDescending(x => x.AcquiredAtUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserItem userItem, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<UserItem>().Update(userItem);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new InvalidOperationException("User item was updated concurrently.", exception);
        }
    }

    /// <inheritdoc />
    public async Task AddAsync(UserItem userItem, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<UserItem>().AddAsync(userItem, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static bool IsUniqueUseOperationViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   UseOperationUniqueConstraint,
                   StringComparison.OrdinalIgnoreCase);
    }
}
