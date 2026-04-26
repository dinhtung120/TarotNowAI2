using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReadingSessionRevealRequestedDomainEventHandler
{
    private const int TarotDeckSize = 78;

    private async Task<bool> TryHandleCompletedSessionReplayAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        ReadingSession session,
        ReadingRevealSagaState saga,
        CancellationToken cancellationToken)
    {
        if (!session.IsCompleted)
        {
            return false;
        }

        var completedCards = ReadingDrawnCardCodec.Parse(session.CardsDrawn);
        await EnsureCompletedSagaSnapshotAsync(saga, session.CardsDrawn, cancellationToken);
        domainEvent.RevealedCards = completedCards;
        domainEvent.IsIdempotentReplay = true;
        return true;
    }

    private async Task EnsureCompletedSagaSnapshotAsync(
        ReadingRevealSagaState saga,
        string? sessionCardsJson,
        CancellationToken cancellationToken)
    {
        if (saga.IsTerminal())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(saga.RevealedCardsJson) && !string.IsNullOrWhiteSpace(sessionCardsJson))
        {
            saga.SetRevealedCards(sessionCardsJson!, DateTime.UtcNow);
        }

        saga.MarkSessionCompleted(DateTime.UtcNow);
        saga.MarkRevealedEventPublished(DateTime.UtcNow);
        saga.MarkCompleted(DateTime.UtcNow);
        await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);
    }

    private async Task<IReadOnlyList<ReadingDrawnCard>> EnsureRevealedCardsAsync(
        ReadingRevealSagaState saga,
        ReadingSession session,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(saga.RevealedCardsJson))
        {
            return ReadingDrawnCardCodec.Parse(saga.RevealedCardsJson);
        }

        var revealedCards = DrawCards(session.SpreadType);
        saga.SetRevealedCards(ReadingDrawnCardCodec.Serialize(revealedCards), DateTime.UtcNow);
        await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);
        return revealedCards;
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
