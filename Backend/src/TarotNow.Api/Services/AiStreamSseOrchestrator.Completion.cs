using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Api.Services;

public sealed partial class AiStreamSseOrchestrator
{
    private async Task HandleCanceledStreamAsync(
        OperationCanceledException exception,
        StreamCompletionContext context,
        HttpContext httpContext,
        CancellationToken requestToken)
    {
        var finalStatus = context.State.OutputTokens > 0
            ? AiStreamFinalStatuses.FailedAfterFirstToken
            : AiStreamFinalStatuses.FailedBeforeFirstToken;

        var clientDisconnected = requestToken.IsCancellationRequested
                                 || httpContext.RequestAborted.IsCancellationRequested;
        var finishReason = clientDisconnected
            ? "Client disconnected"
            : "Upstream timeout/cancellation";

        await CompleteStreamAsync(new StreamCompletionDispatch(
            context.AiRequestId,
            context.UserId,
            FollowUpQuestion: null,
            context.State,
            finalStatus,
            finishReason,
            clientDisconnected,
            CancellationToken.None));

        if (!clientDisconnected)
        {
            _logger.LogWarning(
                exception,
                "AI stream canceled by upstream for session {SessionId}, request {AiRequestId}.",
                context.SessionId,
                context.AiRequestId);
        }
    }

    private async Task HandleFailedStreamAsync(
        Exception exception,
        StreamCompletionContext context,
        HttpResponse response,
        CancellationToken cancellationToken)
    {
        var finalStatus = context.State.OutputTokens > 0
            ? AiStreamFinalStatuses.FailedAfterFirstToken
            : AiStreamFinalStatuses.FailedBeforeFirstToken;

        await CompleteStreamAsync(new StreamCompletionDispatch(
            context.AiRequestId,
            context.UserId,
            FollowUpQuestion: null,
            context.State,
            finalStatus,
            "stream_runtime_error",
            IsClientDisconnect: false,
            cancellationToken));

        _logger.LogError(
            exception,
            "AI stream runtime error for session {SessionId}, request {AiRequestId}.",
            context.SessionId,
            context.AiRequestId);

        if (!response.HasStarted)
        {
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync(response, "Stream Error: Unable to process AI stream. Please try again.", CancellationToken.None);
        }
    }

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

    private readonly record struct StreamCompletionDispatch(
        Guid AiRequestId,
        Guid UserId,
        string? FollowUpQuestion,
        StreamExecutionState State,
        string FinalStatus,
        string? ErrorMessage,
        bool IsClientDisconnect,
        CancellationToken CancellationToken);
}
