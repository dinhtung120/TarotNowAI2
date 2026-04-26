using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Services;

/// <summary>
/// Service điều phối vòng đời SSE stream cho AI reading.
/// </summary>
public sealed partial class AiStreamSseOrchestrator : IAiStreamSseOrchestrator
{
    private readonly IMediator _mediator;
    private readonly ILogger<AiStreamSseOrchestrator> _logger;

    /// <summary>
    /// Khởi tạo orchestrator cho AI SSE.
    /// </summary>
    public AiStreamSseOrchestrator(
        IMediator mediator,
        ILogger<AiStreamSseOrchestrator> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(
        HttpContext httpContext,
        AiStreamOrchestrationRequest request,
        CancellationToken cancellationToken)
    {
        var streamResult = await TryStartStreamAsync(
            request,
            httpContext.Response,
            cancellationToken);
        if (streamResult is null)
        {
            return;
        }

        ConfigureSseHeaders(httpContext.Response);
        await StreamAndFinalizeAsync(
            streamResult,
            request,
            httpContext,
            cancellationToken);
    }

    private async Task<StreamReadingResult?> TryStartStreamAsync(
        AiStreamOrchestrationRequest request,
        HttpResponse response,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _mediator.Send(new StreamReadingCommand
            {
                UserId = request.UserId,
                ReadingSessionId = request.SessionId,
                FollowupQuestion = request.FollowUpQuestion,
                Language = request.Language ?? "vi",
                IdempotencyKey = request.IdempotencyKey
            }, cancellationToken);
        }
        catch (BadRequestException ex)
        {
            _logger.LogInformation(ex, "Rejected AI stream request for session {SessionId}.", request.SessionId);
            response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteServerEventAsync(response, "AI stream request is invalid.", cancellationToken);
            return null;
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "AI stream session not found {SessionId}.", request.SessionId);
            response.StatusCode = StatusCodes.Status404NotFound;
            await WriteServerEventAsync(response, "Reading session was not found.", cancellationToken);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize AI stream for session {SessionId}.", request.SessionId);
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync(response, "Unable to start AI stream. Please try again later.", cancellationToken);
            return null;
        }
    }

    private static void ConfigureSseHeaders(HttpResponse response)
    {
        response.Headers.Append("Content-Type", "text/event-stream");
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("Connection", "keep-alive");
    }
}
