using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    private readonly record struct StreamCompletionDispatch(
        Guid AiRequestId,
        Guid UserId,
        string? FollowUpQuestion,
        StreamExecutionState State,
        string FinalStatus,
        string? ErrorMessage,
        bool IsClientDisconnect,
        CancellationToken CancellationToken);

    private async Task CompleteStreamAsync(StreamCompletionDispatch completion)
    {
        var latencyMs = completion.State.FirstTokenAt.HasValue
            ? (int)(DateTimeOffset.UtcNow - completion.State.FirstTokenAt.Value).TotalMilliseconds
            : 0;

        await _mediator.Send(new CompleteAiStreamCommand
        {
            AiRequestId = completion.AiRequestId,
            UserId = completion.UserId,
            FinalStatus = completion.FinalStatus,
            ErrorMessage = completion.ErrorMessage,
            IsClientDisconnect = completion.IsClientDisconnect,
            FirstTokenAt = completion.State.FirstTokenAt,
            OutputTokens = completion.State.OutputTokens,
            LatencyMs = latencyMs,
            FullResponse = completion.State.FullResponseBuilder.ToString(),
            FollowupQuestion = completion.FollowUpQuestion
        }, completion.CancellationToken);
    }
}
