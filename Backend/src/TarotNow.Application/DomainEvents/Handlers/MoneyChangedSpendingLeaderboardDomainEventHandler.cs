using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler cập nhật leaderboard chi tiêu từ MoneyChangedDomainEvent.
/// </summary>
public sealed class MoneyChangedSpendingLeaderboardDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MoneyChangedDomainEvent>
{
    private readonly ILeaderboardRepository _leaderboardRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật leaderboard chi tiêu.
    /// </summary>
    public MoneyChangedSpendingLeaderboardDomainEventHandler(
        ILeaderboardRepository leaderboardRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _leaderboardRepository = leaderboardRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        MoneyChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var spendingAmount = ResolveSpendingAmount(domainEvent);
        if (spendingAmount <= 0)
        {
            return;
        }

        var track = ResolveTrack(domainEvent.Currency);
        if (track == null)
        {
            return;
        }

        var dailyKey = PeriodKeyHelper.GetPeriodKey("daily");
        var monthlyKey = PeriodKeyHelper.GetPeriodKey("monthly");
        await _leaderboardRepository.IncrementScoreAsync(domainEvent.UserId, track, dailyKey, spendingAmount, cancellationToken);
        await _leaderboardRepository.IncrementScoreAsync(domainEvent.UserId, track, monthlyKey, spendingAmount, cancellationToken);
        await _leaderboardRepository.IncrementScoreAsync(domainEvent.UserId, track, "all", spendingAmount, cancellationToken);
    }

    private static long ResolveSpendingAmount(MoneyChangedDomainEvent domainEvent)
    {
        if (domainEvent.DeltaAmount >= 0)
        {
            return 0;
        }

        if (string.Equals(domainEvent.ChangeType, TransactionType.EscrowFreeze, StringComparison.OrdinalIgnoreCase)
            || string.Equals(domainEvent.ChangeType, "freeze", StringComparison.OrdinalIgnoreCase))
        {
            return 0;
        }

        return Math.Abs(domainEvent.DeltaAmount);
    }

    private static string? ResolveTrack(string currency)
    {
        var normalizedCurrency = currency.Trim().ToLowerInvariant();
        return normalizedCurrency switch
        {
            CurrencyType.Gold => "spent_gold",
            CurrencyType.Diamond => "spent_diamond",
            _ => null
        };
    }
}
