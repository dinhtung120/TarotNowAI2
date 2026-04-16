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

        var domainEvent = BuildItemUsedEvent(
            request.UserId,
            normalizedItemCode,
            request.Quantity,
            request.TargetCardId,
            normalizedIdempotencyKey);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return BuildResult(domainEvent);
    }

    private static ItemUsedDomainEvent BuildItemUsedEvent(
        Guid userId,
        string itemCode,
        int quantity,
        int? targetCardId,
        string normalizedIdempotencyKey)
    {
        return new ItemUsedDomainEvent
        {
            UserId = userId,
            ItemCode = itemCode,
            Quantity = quantity,
            TargetCardId = targetCardId,
            IdempotencyKey = normalizedIdempotencyKey,
        };
    }

    private static UseInventoryItemResult BuildResult(ItemUsedDomainEvent domainEvent)
    {
        return new UseInventoryItemResult
        {
            ItemCode = domainEvent.ItemCode,
            TargetCardId = domainEvent.TargetCardId,
            IsIdempotentReplay = domainEvent.IsIdempotentReplay,
            Message = domainEvent.IsIdempotentReplay
                ? InventoryCommandMessages.Replayed
                : InventoryCommandMessages.Accepted,
            EffectSummaries = domainEvent.EffectSummaries.Select(MapEffectSummary).Where(x => x != null).Cast<UseInventoryItemEffectSummary>().ToList(),
        };
    }

    private static UseInventoryItemEffectSummary? MapEffectSummary(InventoryItemEffectSummary? effectSummary)
    {
        if (effectSummary is null)
        {
            return null;
        }

        return new UseInventoryItemEffectSummary
        {
            EffectType = effectSummary.EffectType,
            RolledValue = effectSummary.RolledValue,
            CardId = effectSummary.CardId,
            BeforeValue = effectSummary.BeforeValue,
            AfterValue = effectSummary.AfterValue,
            Before = MapCardSnapshot(effectSummary.Before),
            After = MapCardSnapshot(effectSummary.After),
        };
    }

    private static UseInventoryCardStatSnapshot? MapCardSnapshot(InventoryCardStatSnapshot? snapshot)
    {
        if (snapshot is null)
        {
            return null;
        }

        return new UseInventoryCardStatSnapshot
        {
            Level = snapshot.Level,
            CurrentExp = snapshot.CurrentExp,
            ExpToNextLevel = snapshot.ExpToNextLevel,
            BaseAtk = snapshot.BaseAtk,
            BaseDef = snapshot.BaseDef,
            BonusAtkPercent = snapshot.BonusAtkPercent,
            BonusDefPercent = snapshot.BonusDefPercent,
            TotalAtk = snapshot.TotalAtk,
            TotalDef = snapshot.TotalDef,
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
        if (targetCardId is not null && targetCardId < 0)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.TargetCardRequired,
                "Target card id must be greater than or equal to 0.");
        }
    }
}
