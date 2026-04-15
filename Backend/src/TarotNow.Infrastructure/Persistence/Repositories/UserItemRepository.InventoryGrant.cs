using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý cấp item reward cho user.
/// </summary>
public sealed partial class UserItemRepository
{
    /// <inheritdoc />
    public async Task GrantItemByCodeAsync(
        Guid userId,
        string itemCode,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        ValidateGrantRequest(userId, itemCode, quantity);

        var definition = await GetActiveDefinitionByCodeAsync(itemCode, cancellationToken);
        var userItem = await GetExistingUserItemAsync(userId, definition.Id, cancellationToken);
        if (userItem is null)
        {
            await AddNewUserItemAsync(userId, definition.Id, quantity, cancellationToken);
            return;
        }

        userItem.IncreaseQuantity(quantity);
        _dbContext.Set<UserItem>().Update(userItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateGrantRequest(Guid userId, string itemCode, int quantity)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(itemCode))
        {
            throw new ArgumentException("Item code is required.", nameof(itemCode));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }
    }

    private async Task<ItemDefinition> GetActiveDefinitionByCodeAsync(string itemCode, CancellationToken cancellationToken)
    {
        var normalizedItemCode = itemCode.Trim().ToLowerInvariant();
        var definition = await _dbContext.Set<ItemDefinition>()
            .FirstOrDefaultAsync(
                x => x.Code == normalizedItemCode && x.IsActive,
                cancellationToken);
        if (definition is null)
        {
            throw new InvalidOperationException($"Item definition '{normalizedItemCode}' was not found.");
        }

        return definition;
    }

    private Task<UserItem?> GetExistingUserItemAsync(
        Guid userId,
        Guid itemDefinitionId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserItem>()
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.ItemDefinitionId == itemDefinitionId,
                cancellationToken);
    }

    private async Task AddNewUserItemAsync(
        Guid userId,
        Guid itemDefinitionId,
        int quantity,
        CancellationToken cancellationToken)
    {
        var userItem = new UserItem(userId, itemDefinitionId, quantity);
        await _dbContext.Set<UserItem>().AddAsync(userItem, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
