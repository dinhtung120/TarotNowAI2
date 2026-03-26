using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public static class AiStreamFinalStatuses
{
    public static readonly string Completed = AiRequestStatus.Completed;
    public static readonly string FailedBeforeFirstToken = AiRequestStatus.FailedBeforeFirstToken;
    public static readonly string FailedAfterFirstToken = AiRequestStatus.FailedAfterFirstToken;

    public static bool IsSupported(string? status)
    {
        return string.Equals(status, Completed, StringComparison.Ordinal)
               || string.Equals(status, FailedBeforeFirstToken, StringComparison.Ordinal)
               || string.Equals(status, FailedAfterFirstToken, StringComparison.Ordinal);
    }
}
