using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;
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

/// <summary>
/// Handler write-side cho reveal reading session.
/// </summary>
public sealed class ReadingSessionRevealRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionRevealRequestedDomainEvent>
{
    private const int TarotDeckSize = 78;
    private const long ExpPerCard = 1;

    private readonly IReadingSessionRepository _readingSessionRepository;
    private readonly IUserCollectionRepository _userCollectionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IRngService _rngService;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler reveal-requested.
    /// </summary>
    public ReadingSessionRevealRequestedDomainEventHandler(
        IReadingSessionRepository readingSessionRepository,
        IUserCollectionRepository userCollectionRepository,
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        IRngService rngService,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readingSessionRepository = readingSessionRepository;
        _userCollectionRepository = userCollectionRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _rngService = rngService;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var session = await GetSessionAsync(domainEvent, cancellationToken);
        if (session.IsCompleted)
        {
            domainEvent.RevealedCards = ReadingDrawnCardCodec.Parse(session.CardsDrawn);
            domainEvent.IsIdempotentReplay = true;
            return;
        }

        var revealedCards = DrawCards(session.SpreadType);
        await ChargeReadingAsync(domainEvent.UserId, session, cancellationToken);
        await UpdateCollectionAndUserExpAsync(domainEvent.UserId, session, revealedCards, cancellationToken);

        session.CompleteSession(ReadingDrawnCardCodec.Serialize(revealedCards));
        await _readingSessionRepository.UpdateAsync(session, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new ReadingSessionRevealedDomainEvent
            {
                UserId = domainEvent.UserId,
                SessionId = session.Id,
                Language = domainEvent.Language,
                RevealedCards = revealedCards
            },
            cancellationToken);

        domainEvent.RevealedCards = revealedCards;
        domainEvent.IsIdempotentReplay = false;
    }

    private async Task<ReadingSession> GetSessionAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var session = await _readingSessionRepository.GetByIdAsync(domainEvent.SessionId, cancellationToken)
            ?? throw new NotFoundException("Session not found");

        if (session.UserId != domainEvent.UserId.ToString())
        {
            throw new UnauthorizedAccessException("Reading session not found or access denied");
        }

        return session;
    }

    private IReadOnlyList<ReadingDrawnCard> DrawCards(string spreadType)
    {
        var cardsToDraw = ResolveCardsToDraw(spreadType);
        var shuffledDeck = _rngService.ShuffleDeck(TarotDeckSize);
        var revealedCards = new List<ReadingDrawnCard>(cardsToDraw);

        for (var i = 0; i < cardsToDraw; i++)
        {
            revealedCards.Add(new ReadingDrawnCard
            {
                CardId = shuffledDeck[i],
                Position = i,
                Orientation = _rngService.NextBoolean() ? CardOrientation.Reversed : CardOrientation.Upright
            });
        }

        return revealedCards;
    }

    private static int ResolveCardsToDraw(string spreadType)
    {
        return spreadType switch
        {
            SpreadType.Daily1Card => 1,
            SpreadType.Spread3Cards => 3,
            SpreadType.Spread5Cards => 5,
            SpreadType.Spread10Cards => 10,
            _ => throw new BadRequestException($"Invalid spread type: {spreadType}")
        };
    }

    private async Task ChargeReadingAsync(Guid userId, ReadingSession session, CancellationToken cancellationToken)
    {
        var amount = Math.Max(session.AmountCharged, 0);
        var currency = NormalizeCurrency(session.CurrencyUsed);
        if (amount <= 0)
        {
            return;
        }

        var changeType = currency == CurrencyType.Gold
            ? TransactionType.ReadingCostGold
            : TransactionType.ReadingCostDiamond;

        try
        {
            var operationResult = await _walletRepository.DebitWithResultAsync(
                userId,
                currency,
                changeType,
                amount,
                "Reading",
                session.Id,
                $"Tarot reading reveal charge ({session.SpreadType})",
                metadataJson: null,
                idempotencyKey: $"reading_reveal_charge_{session.Id}",
                cancellationToken: cancellationToken);

            if (!operationResult.Executed)
            {
                return;
            }

            await _domainEventPublisher.PublishAsync(
                new MoneyChangedDomainEvent
                {
                    UserId = userId,
                    Currency = currency,
                    ChangeType = changeType,
                    DeltaAmount = -amount,
                    ReferenceId = session.Id
                },
                cancellationToken);
        }
        catch (InvalidOperationException)
        {
            throw new BadRequestException("Not enough balance to reveal this reading session.");
        }
    }

    private async Task UpdateCollectionAndUserExpAsync(
        Guid userId,
        ReadingSession session,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        var expToGrantPerCard = ResolveExpToGrant(session) * ExpPerCard;
        foreach (var card in revealedCards)
        {
            await _userCollectionRepository.UpsertCardAsync(
                userId,
                card.CardId,
                expToGrantPerCard,
                card.Orientation,
                cancellationToken);
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        user.AddExp(revealedCards.Count * expToGrantPerCard);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    private static string NormalizeCurrency(string? currency)
    {
        return string.Equals(currency, CurrencyType.Diamond, StringComparison.OrdinalIgnoreCase)
            ? CurrencyType.Diamond
            : CurrencyType.Gold;
    }

    private static long ResolveExpToGrant(ReadingSession session)
    {
        var usesDiamond = string.Equals(
            session.CurrencyUsed,
            CurrencyType.Diamond,
            StringComparison.OrdinalIgnoreCase);

        return session.SpreadType != SpreadType.Daily1Card && usesDiamond ? 2 : 1;
    }
}

/// <summary>
/// Handler hậu reveal để precompute AI ở nền.
/// </summary>
public sealed class ReadingSessionRevealedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionRevealedDomainEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ReadingSessionRevealedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler session-revealed.
    /// </summary>
    public ReadingSessionRevealedDomainEventHandler(
        IMediator mediator,
        ILogger<ReadingSessionRevealedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionRevealedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        StreamReadingResult? streamResult;
        try
        {
            streamResult = await _mediator.Send(
                new StreamReadingCommand
                {
                    UserId = domainEvent.UserId,
                    ReadingSessionId = domainEvent.SessionId,
                    FollowupQuestion = null,
                    Language = NormalizeLanguage(domainEvent.Language)
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to start background AI precompute. SessionId={SessionId} UserId={UserId}",
                domainEvent.SessionId,
                domainEvent.UserId);
            return;
        }

        var firstTokenAt = default(DateTimeOffset?);
        var outputTokens = 0;
        var fullResponseBuilder = new System.Text.StringBuilder();
        var finalStatus = AiStreamFinalStatuses.Completed;
        var errorMessage = default(string);

        try
        {
            await foreach (var chunk in streamResult.Stream.WithCancellation(cancellationToken))
            {
                firstTokenAt ??= DateTimeOffset.UtcNow;
                fullResponseBuilder.Append(chunk);
                outputTokens++;
            }
        }
        catch (Exception ex)
        {
            finalStatus = firstTokenAt.HasValue
                ? AiStreamFinalStatuses.FailedAfterFirstToken
                : AiStreamFinalStatuses.FailedBeforeFirstToken;
            errorMessage = ex.Message;
            _logger.LogWarning(
                ex,
                "Background AI precompute stream failed. SessionId={SessionId} UserId={UserId}",
                domainEvent.SessionId,
                domainEvent.UserId);
        }

        await _mediator.Send(
            new CompleteAiStreamCommand
            {
                AiRequestId = streamResult.AiRequestId,
                UserId = domainEvent.UserId,
                FinalStatus = finalStatus,
                ErrorMessage = errorMessage,
                IsClientDisconnect = false,
                FirstTokenAt = firstTokenAt,
                OutputTokens = outputTokens,
                LatencyMs = 0,
                FullResponse = fullResponseBuilder.ToString(),
                FollowupQuestion = null
            },
            cancellationToken);
    }

    private static string NormalizeLanguage(string? language)
    {
        return language?.Trim().ToLowerInvariant() switch
        {
            "en" => "en",
            "zh" => "zh",
            _ => "vi"
        };
    }
}
