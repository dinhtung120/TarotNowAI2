using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReadingSessionRevealRequestedDomainEventHandler
{
    private const int SagaRepairDelaySeconds = 30;

    private async Task<IReadOnlyList<ReadingDrawnCard>> ExecuteSagaWorkflowAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        ReadingSession session,
        ReadingRevealSagaState saga,
        CancellationToken cancellationToken)
    {
        saga.StartAttempt(DateTime.UtcNow);
        await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);

        var revealedCards = await EnsureRevealedCardsAsync(saga, session, cancellationToken);

        try
        {
            await EnsureChargeStepAsync(domainEvent, session, saga, cancellationToken);
            await EnsureCollectionStepAsync(domainEvent.UserId, session, saga, revealedCards, cancellationToken);
            await EnsureExpStepAsync(domainEvent.UserId, session, saga, revealedCards, cancellationToken);
            await EnsureSessionCompletionStepAsync(session, saga, revealedCards, cancellationToken);
            await EnsureRevealEventStepAsync(domainEvent, session, saga, revealedCards, cancellationToken);

            saga.MarkCompleted(DateTime.UtcNow);
            await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);
            return revealedCards;
        }
        catch (Exception exception)
        {
            await MarkSagaFailedAsync(saga, exception);
            throw new InvalidOperationException($"Reading reveal saga failed for session {session.Id}.", exception);
        }
    }

    private async Task EnsureChargeStepAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        ReadingSession session,
        ReadingRevealSagaState saga,
        CancellationToken cancellationToken)
    {
        if (saga.ChargeDebited)
        {
            return;
        }

        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                var chargeSnapshot = await ChargeReadingAsync(domainEvent.UserId, session, transactionCt);
                if (!chargeSnapshot.Debited)
                {
                    return;
                }

                saga.MarkChargeDebited(
                    chargeSnapshot.Currency,
                    chargeSnapshot.ChangeType,
                    chargeSnapshot.Amount,
                    chargeSnapshot.ReferenceId,
                    DateTime.UtcNow);
                await _readingRevealSagaStateRepository.UpdateAsync(saga, transactionCt);
            },
            cancellationToken);
    }

    private async Task EnsureCollectionStepAsync(
        Guid userId,
        ReadingSession session,
        ReadingRevealSagaState saga,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        if (saga.CollectionApplied)
        {
            return;
        }

        await ApplyCollectionStepAsync(userId, session, revealedCards, cancellationToken);
        saga.MarkCollectionApplied(DateTime.UtcNow);
        await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);
    }

    private async Task EnsureExpStepAsync(
        Guid userId,
        ReadingSession session,
        ReadingRevealSagaState saga,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        if (saga.ExpGranted)
        {
            return;
        }

        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                await GrantUserExpAsync(userId, session, revealedCards, transactionCt);
                saga.MarkExpGranted(DateTime.UtcNow);
                await _readingRevealSagaStateRepository.UpdateAsync(saga, transactionCt);
            },
            cancellationToken);
    }

    private async Task EnsureSessionCompletionStepAsync(
        ReadingSession session,
        ReadingRevealSagaState saga,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        if (saga.SessionCompleted)
        {
            return;
        }

        var revealedCardsJson = saga.RevealedCardsJson ?? ReadingDrawnCardCodec.Serialize(revealedCards);
        session.CompleteSession(revealedCardsJson);
        await _readingSessionRepository.UpdateAsync(session, cancellationToken);

        saga.MarkSessionCompleted(DateTime.UtcNow);
        await _readingRevealSagaStateRepository.UpdateAsync(saga, cancellationToken);
    }

    private async Task EnsureRevealEventStepAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        ReadingSession session,
        ReadingRevealSagaState saga,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        if (saga.RevealedEventPublished)
        {
            return;
        }

        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                await _domainEventPublisher.PublishAsync(
                    new ReadingSessionRevealedDomainEvent
                    {
                        UserId = domainEvent.UserId,
                        SessionId = session.Id,
                        Language = domainEvent.Language,
                        RevealedCards = revealedCards
                    },
                    transactionCt);

                saga.MarkRevealedEventPublished(DateTime.UtcNow);
                await _readingRevealSagaStateRepository.UpdateAsync(saga, transactionCt);
            },
            cancellationToken);
    }

    private async Task MarkSagaFailedAsync(ReadingRevealSagaState saga, Exception exception)
    {
        saga.MarkFailed(
            error: exception.Message,
            nowUtc: DateTime.UtcNow,
            nextRepairAtUtc: DateTime.UtcNow.AddSeconds(SagaRepairDelaySeconds));
        await _readingRevealSagaStateRepository.UpdateAsync(saga, CancellationToken.None);
    }
}
