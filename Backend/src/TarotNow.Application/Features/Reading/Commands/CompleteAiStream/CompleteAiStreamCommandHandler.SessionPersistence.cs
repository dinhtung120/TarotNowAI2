using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandler
{
    private async Task UpdateReadingSessionContentAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        CancellationToken cancellationToken)
    {
        if (request.FinalStatus != AiStreamFinalStatuses.Completed || string.IsNullOrWhiteSpace(request.FullResponse))
        {
            return;
        }

        var session = await _readingRepo.GetByIdAsync(record.ReadingSessionRef, cancellationToken);
        if (session == null)
        {
            return;
        }

        var updatedSession = BuildUpdatedSession(session, request);
        await _readingRepo.UpdateAsync(updatedSession, cancellationToken);
    }

    private static ReadingSession BuildUpdatedSession(
        ReadingSession session,
        CompleteAiStreamCommand request)
    {
        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            var newFollowups = session.Followups.ToList();
            newFollowups.Add(new ReadingFollowup
            {
                Question = string.IsNullOrWhiteSpace(request.FollowupQuestion)
                    ? "Câu hỏi Follow-up"
                    : request.FollowupQuestion,
                Answer = request.FullResponse!
            });

            return RehydrateSession(session, session.AiSummary, newFollowups);
        }

        return RehydrateSession(session, request.FullResponse!, session.Followups);
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
