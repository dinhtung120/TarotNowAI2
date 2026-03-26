namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public class StreamReadingResult
{
    public required IAsyncEnumerable<string> Stream { get; init; }

    public required Guid AiRequestId { get; init; }
}
