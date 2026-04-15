using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler write-side cho init reading session.
/// </summary>
public sealed class ReadingSessionInitRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionInitRequestedDomainEvent>
{
    private readonly IReadingSessionRepository _readingSessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler init-requested.
    /// </summary>
    public ReadingSessionInitRequestedDomainEventHandler(
        IReadingSessionRepository readingSessionRepository,
        IUserRepository userRepository,
        ISystemConfigSettings systemConfigSettings,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readingSessionRepository = readingSessionRepository;
        _userRepository = userRepository;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionInitRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(domainEvent.UserId, cancellationToken);
        await EnsureDailyCardLimitAsync(domainEvent, cancellationToken);

        var currency = NormalizeCurrency(domainEvent.Currency);
        var (costGold, costDiamond) = ResolveBasePricing(domainEvent.SpreadType, currency);
        var amountCharged = costGold > 0 ? costGold : costDiamond;
        var session = new ReadingSession(
            domainEvent.UserId.ToString(),
            domainEvent.SpreadType,
            domainEvent.Question,
            currency,
            amountCharged);

        await _readingSessionRepository.CreateAsync(session, cancellationToken);

        domainEvent.CostGold = costGold;
        domainEvent.CostDiamond = costDiamond;
        domainEvent.AmountCharged = amountCharged;
        domainEvent.CurrencyUsed = currency;
        domainEvent.SessionId = session.Id;
        domainEvent.Session = session;
    }

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
    }

    private async Task EnsureDailyCardLimitAsync(
        ReadingSessionInitRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        if (domainEvent.SpreadType != SpreadType.Daily1Card)
        {
            return;
        }

        var alreadyDrawn = await _readingSessionRepository.HasDrawnDailyCardAsync(
            domainEvent.UserId,
            DateTime.UtcNow,
            cancellationToken);
        if (alreadyDrawn)
        {
            throw new BadRequestException("You have already drawn your free daily card today. Please try other spreads.");
        }
    }

    private (long CostGold, long CostDiamond) ResolveBasePricing(string spreadType, string currency)
    {
        return spreadType switch
        {
            SpreadType.Spread3Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread3GoldCost,
                _systemConfigSettings.Spread3DiamondCost),
            SpreadType.Spread5Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread5GoldCost,
                _systemConfigSettings.Spread5DiamondCost),
            SpreadType.Spread10Cards => GetConfiguredPrice(
                currency,
                _systemConfigSettings.Spread10GoldCost,
                _systemConfigSettings.Spread10DiamondCost),
            _ => (0L, 0L)
        };
    }

    private static (long CostGold, long CostDiamond) GetConfiguredPrice(
        string currency,
        long goldPrice,
        long diamondPrice)
    {
        return currency == CurrencyType.Diamond
            ? (0, diamondPrice)
            : (goldPrice, 0);
    }

    private static string NormalizeCurrency(string? currency)
    {
        var normalized = currency?.Trim().ToLowerInvariant();
        return normalized == CurrencyType.Diamond ? CurrencyType.Diamond : CurrencyType.Gold;
    }
}
