using MediatR;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public class StreamReadingCommand : IRequest<StreamReadingResult>
{
    public Guid UserId { get; set; }

    public string ReadingSessionId { get; set; } = string.Empty;

    public string? FollowupQuestion { get; set; }

    public string Language { get; set; } = "vi";
}
