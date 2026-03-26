using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    private readonly record struct StreamCompletionContext(
        Guid AiRequestId,
        Guid UserId,
        string SessionId,
        StreamExecutionState State);

    private async Task HandleCanceledStreamAsync(
        OperationCanceledException exception,
        StreamCompletionContext context,
        CancellationToken requestToken)
    {
        var finalStatus = context.State.OutputTokens > 0
            ? AiStreamFinalStatuses.FailedAfterFirstToken
            : AiStreamFinalStatuses.FailedBeforeFirstToken;

        var clientDisconnected = requestToken.IsCancellationRequested
                                 || HttpContext.RequestAborted.IsCancellationRequested;
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
            exception.Message,
            IsClientDisconnect: false,
            cancellationToken));

        _logger.LogError(
            exception,
            "AI stream runtime error for session {SessionId}, request {AiRequestId}.",
            context.SessionId,
            context.AiRequestId);

        if (!Response.HasStarted)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync("Stream Error: Unable to process AI stream. Please try again.", CancellationToken.None);
        }
    }
}
