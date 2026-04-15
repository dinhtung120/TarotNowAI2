using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler write-side cho reveal reading session.
/// </summary>
public sealed partial class ReadingSessionRevealRequestedDomainEventHandler
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

}
