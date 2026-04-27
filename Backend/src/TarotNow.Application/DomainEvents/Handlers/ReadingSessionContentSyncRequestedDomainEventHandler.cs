using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;
using System;
using System.Linq;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler đồng bộ nội dung ReadingSession ở Mongo sau khi stream hoàn tất.
/// </summary>
public sealed class ReadingSessionContentSyncRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionContentSyncRequestedDomainEvent>
{
    private readonly IReadingSessionRepository _readingSessionRepository;

    public ReadingSessionContentSyncRequestedDomainEventHandler(
        IReadingSessionRepository readingSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readingSessionRepository = readingSessionRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionContentSyncRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var session = await _readingSessionRepository.GetByIdAsync(domainEvent.SessionId, cancellationToken);
        if (session is null)
        {
            return;
        }

        var updatedSession = BuildUpdatedSession(session, domainEvent);
        await _readingSessionRepository.UpdateAsync(updatedSession, cancellationToken);
    }

    private static ReadingSession BuildUpdatedSession(
        ReadingSession session,
        ReadingSessionContentSyncRequestedDomainEvent domainEvent)
    {
        var aiRequestId = domainEvent.AiRequestId.ToString("D");
        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            if (session.Followups.Any(f => string.Equals(f.AiRequestId, aiRequestId, StringComparison.OrdinalIgnoreCase)))
            {
                return session;
            }

            var newFollowups = session.Followups.ToList();
            newFollowups.Add(new ReadingFollowup
            {
                Question = string.IsNullOrWhiteSpace(domainEvent.FollowupQuestion)
                    ? "Câu hỏi Follow-up"
                    : domainEvent.FollowupQuestion,
                Answer = domainEvent.FullResponse,
                AiRequestId = aiRequestId
            });

            return RehydrateSession(session, session.AiSummary, newFollowups);
        }

        return RehydrateSession(session, domainEvent.FullResponse, session.Followups);
    }

    private static ReadingSession RehydrateSession(
        ReadingSession session,
        string aiSummary,
        IEnumerable<ReadingFollowup> followups)
    {
        return ReadingSession.Rehydrate(new ReadingSessionSnapshot
        {
            Id = session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            CardsDrawn = session.CardsDrawn,
            CurrencyUsed = session.CurrencyUsed,
            AmountCharged = session.AmountCharged,
            IsCompleted = session.IsCompleted,
            CreatedAt = session.CreatedAt,
            CompletedAt = session.CompletedAt,
            AiSummary = aiSummary,
            Followups = followups.ToList()
        });
    }
}
