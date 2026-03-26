using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    private async Task<StreamReadingResult?> TryStartStreamAsync(
        Guid userId,
        string sessionId,
        string? followUpQuestion,
        string? language,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _mediator.Send(new StreamReadingCommand
            {
                UserId = userId,
                ReadingSessionId = sessionId,
                FollowupQuestion = followUpQuestion,
                Language = language ?? "vi"
            }, cancellationToken);
        }
        catch (BadRequestException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteServerEventAsync(ex.Message, cancellationToken);
            return null;
        }
        catch (NotFoundException ex)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            await WriteServerEventAsync(ex.Message, cancellationToken);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize AI stream for session {SessionId}.", sessionId);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync("Unable to start AI stream. Please try again later.", cancellationToken);
            return null;
        }
    }

    private static void ConfigureSseHeaders(HttpResponse response)
    {
        response.Headers.Append("Content-Type", "text/event-stream");
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("Connection", "keep-alive");
    }

    private async Task StreamAndFinalizeAsync(
        StreamReadingResult result,
        Guid userId,
        string sessionId,
        string? followUpQuestion,
        CancellationToken requestToken)
    {
        var state = new StreamExecutionState();
        var completionContext = new StreamCompletionContext(
            result.AiRequestId,
            userId,
            sessionId,
            state);

        try
        {
            await WriteStreamAsync(result.Stream, state, requestToken);
            await WriteDoneEventAsync(requestToken);

            await CompleteStreamAsync(new StreamCompletionDispatch(
                result.AiRequestId,
                userId,
                followUpQuestion,
                state,
                AiStreamFinalStatuses.Completed,
                ErrorMessage: null,
                IsClientDisconnect: false,
                CancellationToken.None));
        }
        catch (OperationCanceledException ex)
        {
            await HandleCanceledStreamAsync(ex, completionContext, requestToken);
        }
        catch (Exception ex)
        {
            await HandleFailedStreamAsync(ex, completionContext, CancellationToken.None);
        }
    }
}
