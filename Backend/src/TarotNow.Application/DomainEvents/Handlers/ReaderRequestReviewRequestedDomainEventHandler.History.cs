using TarotNow.Application.Common;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
{
    private static void AppendReviewHistory(
        ReaderRequestDto readerRequest,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        string resultingStatus,
        DateTime reviewedAtUtc)
    {
        readerRequest.ReviewHistory ??= [];
        readerRequest.ReviewHistory.Add(new ReaderRequestReviewHistoryEntryDto
        {
            Action = domainEvent.Action,
            Status = resultingStatus,
            ReviewedBy = domainEvent.AdminId.ToString(),
            AdminNote = domainEvent.AdminNote,
            ReviewedAt = reviewedAtUtc
        });
    }
}
