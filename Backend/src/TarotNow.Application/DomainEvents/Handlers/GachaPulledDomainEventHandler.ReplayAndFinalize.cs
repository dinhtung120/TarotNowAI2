using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class GachaPulledDomainEventHandler
{
    private async Task PopulateReplayResultAsync(
        GachaPulledDomainEvent domainEvent,
        GachaPullOperation operation,
        CancellationToken cancellationToken)
    {
        domainEvent.IsIdempotentReplay = true;
        domainEvent.OperationId = operation.Id;
        if (!operation.IsCompleted)
        {
            domainEvent.IsProcessingReplay = true;
            return;
        }

        var rewardLogs = await _gachaPoolRepository.GetRewardLogsByOperationIdAsync(operation.Id, cancellationToken);
        domainEvent.CurrentPityCount = operation.CurrentPityCount;
        domainEvent.HardPityThreshold = operation.HardPityThreshold;
        domainEvent.WasPityTriggered = operation.WasPityTriggered;
        domainEvent.Rewards = rewardLogs.Select(MapSnapshot).ToList();
    }

    private async Task DebitPullCostAsync(PullExecutionContext context, CancellationToken cancellationToken)
    {
        var totalCost = checked(context.Pool.CostAmount * context.DomainEvent.Count);

        try
        {
            await _walletRepository.DebitAsync(
                context.DomainEvent.UserId,
                context.Pool.CostCurrency,
                TransactionType.GachaCost,
                totalCost,
                description: $"Gacha pull {context.Pool.Code} x{context.DomainEvent.Count}",
                idempotencyKey: $"gacha_pull_debit_{context.DomainEvent.IdempotencyKey}",
                cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            throw new BusinessRuleException(
                GachaErrorCodes.InsufficientBalance,
                "Insufficient balance to pull gacha.",
                exception);
        }

        var moneyChanged = new MoneyChangeRequest(
            context.DomainEvent.UserId,
            context.Pool.CostCurrency,
            TransactionType.GachaCost,
            -totalCost,
            context.DomainEvent.IdempotencyKey);
        await PublishMoneyChangedAsync(moneyChanged, cancellationToken);
    }

    private async Task FinalizeOperationAsync(
        PullExecutionContext context,
        IReadOnlyList<GachaPullRewardLog> rewardLogs,
        bool wasPityReset,
        CancellationToken cancellationToken)
    {
        await _gachaPoolRepository.AddPullRewardLogsAsync(rewardLogs, cancellationToken);
        await _gachaPoolRepository.AddHistoryEntryAsync(
            new GachaHistoryEntry(
                context.Operation.Id,
                context.DomainEvent.UserId,
                context.Pool.Id,
                context.Pool.Code,
                context.DomainEvent.Count,
                context.PityBefore,
                context.UserPity.PullCount,
                wasPityReset),
            cancellationToken);
        await _gachaPoolRepository.MarkPullOperationCompletedAsync(context.Operation, cancellationToken);

        context.DomainEvent.IsIdempotentReplay = false;
        context.DomainEvent.Rewards = rewardLogs.Select(MapSnapshot).ToList();

        await _inlineDomainEventDispatcher.PublishAsync(
            new GachaPullCompletedDomainEvent
            {
                UserId = context.DomainEvent.UserId,
                PoolCode = context.Pool.Code,
                PullCount = context.DomainEvent.Count,
                WasPityTriggered = context.DomainEvent.WasPityTriggered,
            },
            cancellationToken);
    }

    private async Task PublishMoneyChangedAsync(MoneyChangeRequest request, CancellationToken cancellationToken)
    {
        await _inlineDomainEventDispatcher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = request.Currency,
                ChangeType = request.ChangeType,
                DeltaAmount = request.DeltaAmount,
                ReferenceId = request.ReferenceId,
            },
            cancellationToken);
    }

    private static GachaPullRewardSnapshot MapSnapshot(GachaPullRewardLog log)
    {
        return new GachaPullRewardSnapshot
        {
            Kind = log.RewardKind,
            Rarity = log.Rarity,
            Currency = log.Currency,
            Amount = log.Amount,
            ItemDefinitionId = log.ItemDefinitionId,
            ItemCode = log.ItemCode,
            QuantityGranted = log.QuantityGranted,
            IconUrl = log.IconUrl,
            NameVi = log.NameVi,
            NameEn = log.NameEn,
            NameZh = log.NameZh,
        };
    }

    private sealed record MoneyChangeRequest(
        Guid UserId,
        string Currency,
        string ChangeType,
        long DeltaAmount,
        string ReferenceId);
}
