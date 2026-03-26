using MediatR;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public class CompleteAiStreamCommand : IRequest<bool>
{
    public Guid AiRequestId { get; set; }

    public Guid UserId { get; set; }

    public string FinalStatus { get; set; } = AiStreamFinalStatuses.Completed;

    public string? ErrorMessage { get; set; }

    public bool IsClientDisconnect { get; set; }

    public DateTimeOffset? FirstTokenAt { get; set; }

    public int OutputTokens { get; set; }

    public int LatencyMs { get; set; }

    public string? FullResponse { get; set; }

    public string? FollowupQuestion { get; set; }
}
