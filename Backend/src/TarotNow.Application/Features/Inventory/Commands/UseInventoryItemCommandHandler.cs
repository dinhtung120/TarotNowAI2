using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.Features.Inventory.Commands;

/// <summary>
/// Handler tiếp nhận command sử dụng item và publish domain event.
/// </summary>
public sealed class UseInventoryItemCommandHandler : IRequestHandler<UseInventoryItemCommand, UseInventoryItemResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler dùng item inventory.
    /// </summary>
    public UseInventoryItemCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command dùng item theo Rule 0: chỉ validate + load + publish event.
    /// </summary>
    public async Task<UseInventoryItemResult> Handle(UseInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var normalizedItemCode = NormalizeItemCode(request.ItemCode);
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        EnsureTargetCardIdIfProvided(request.TargetCardId);

        await _inlineDomainEventDispatcher.PublishAsync(
            BuildItemUsedEvent(
                request.UserId,
                normalizedItemCode,
                request.TargetCardId,
                normalizedIdempotencyKey),
            cancellationToken);

        return BuildAcceptedResult(normalizedItemCode, request.TargetCardId);
    }

    private static ItemUsedDomainEvent BuildItemUsedEvent(
        Guid userId,
        string itemCode,
        int? targetCardId,
        string normalizedIdempotencyKey)
    {
        return new ItemUsedDomainEvent
        {
            UserId = userId,
            ItemCode = itemCode,
            TargetCardId = targetCardId,
            IdempotencyKey = normalizedIdempotencyKey,
        };
    }

    private static UseInventoryItemResult BuildAcceptedResult(string normalizedItemCode, int? targetCardId)
    {
        return new UseInventoryItemResult
        {
            ItemCode = normalizedItemCode,
            TargetCardId = targetCardId,
            IsIdempotentReplay = false,
            Message = InventoryCommandMessages.Accepted,
        };
    }

    private static string NormalizeItemCode(string itemCode)
    {
        if (string.IsNullOrWhiteSpace(itemCode))
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemNotFound, "Item code is required.");
        }

        return itemCode.Trim().ToLowerInvariant();
    }

    private static string NormalizeIdempotencyKey(string idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BusinessRuleException(InventoryErrorCodes.AlreadyProcessed, "Idempotency key is required.");
        }

        return idempotencyKey.Trim();
    }

    private static void EnsureTargetCardIdIfProvided(int? targetCardId)
    {
        if (targetCardId is not null && targetCardId <= 0)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.TargetCardRequired,
                "Target card id must be a positive number.");
        }
    }
}
