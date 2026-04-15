using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý consume item inventory theo idempotency.
/// </summary>
public sealed partial class UserItemRepository
{
    /// <inheritdoc />
    public async Task<InventoryItemConsumeResult> TryConsumeWithIdempotencyAsync(
        InventoryItemConsumeRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateConsumeRequest(request);

        var operation = BuildUseOperation(request);
        var operationCreated = await TryCreateUseOperationAsync(operation, cancellationToken);
        if (operationCreated == false)
        {
            return InventoryItemConsumeResult.AlreadyProcessed;
        }

        if (request.IsConsumable == false)
        {
            return await HandleNonConsumableOwnershipAsync(request, operation, cancellationToken);
        }

        var consumeQuantity = Math.Max(1, request.ConsumeQuantity);
        var consumed = await TryConsumeQuantityAsync(request.UserId, request.ItemDefinitionId, consumeQuantity, cancellationToken);
        if (consumed)
        {
            return InventoryItemConsumeResult.Consumed;
        }

        return await ResolveConsumeFailureAsync(request, operation, cancellationToken);
    }

    private static void ValidateConsumeRequest(InventoryItemConsumeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(request));
        }

        if (request.ItemDefinitionId == Guid.Empty)
        {
            throw new ArgumentException("ItemDefinitionId is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.ItemCode))
        {
            throw new ArgumentException("ItemCode is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            throw new ArgumentException("IdempotencyKey is required.", nameof(request));
        }
    }

    private static InventoryItemUseOperation BuildUseOperation(InventoryItemConsumeRequest request)
    {
        return new InventoryItemUseOperation(
            request.UserId,
            request.IdempotencyKey.Trim(),
            request.ItemCode.Trim().ToLowerInvariant(),
            request.TargetCardId);
    }

    private async Task<bool> TryCreateUseOperationAsync(
        InventoryItemUseOperation operation,
        CancellationToken cancellationToken)
    {
        _dbContext.Set<InventoryItemUseOperation>().Add(operation);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (IsUniqueUseOperationViolation(exception))
        {
            _dbContext.Entry(operation).State = EntityState.Detached;
            return false;
        }
    }

    private async Task<InventoryItemConsumeResult> HandleNonConsumableOwnershipAsync(
        InventoryItemConsumeRequest request,
        InventoryItemUseOperation operation,
        CancellationToken cancellationToken)
    {
        var ownsItem = await UserOwnsItemAsync(request.UserId, request.ItemDefinitionId, cancellationToken);
        if (ownsItem)
        {
            return InventoryItemConsumeResult.Consumed;
        }

        await RemoveUseOperationAsync(operation, cancellationToken);
        return InventoryItemConsumeResult.ItemNotOwned;
    }

    private async Task<bool> TryConsumeQuantityAsync(
        Guid userId,
        Guid itemDefinitionId,
        int consumeQuantity,
        CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;
        var affectedRows = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE user_items
             SET quantity = quantity - {consumeQuantity},
                 updated_at_utc = {nowUtc}
             WHERE user_id = {userId}
               AND item_definition_id = {itemDefinitionId}
               AND quantity >= {consumeQuantity};
             """,
            cancellationToken);
        return affectedRows > 0;
    }

    private async Task<InventoryItemConsumeResult> ResolveConsumeFailureAsync(
        InventoryItemConsumeRequest request,
        InventoryItemUseOperation operation,
        CancellationToken cancellationToken)
    {
        await RemoveUseOperationAsync(operation, cancellationToken);

        var userOwnsItem = await UserOwnsItemAsync(request.UserId, request.ItemDefinitionId, cancellationToken);
        return userOwnsItem
            ? InventoryItemConsumeResult.OutOfStock
            : InventoryItemConsumeResult.ItemNotOwned;
    }

    private Task<bool> UserOwnsItemAsync(Guid userId, Guid itemDefinitionId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserItem>()
            .AsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.ItemDefinitionId == itemDefinitionId, cancellationToken);
    }

    private async Task RemoveUseOperationAsync(
        InventoryItemUseOperation operation,
        CancellationToken cancellationToken)
    {
        _dbContext.Set<InventoryItemUseOperation>().Remove(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
