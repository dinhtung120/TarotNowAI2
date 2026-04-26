using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Services;

public sealed partial class AiStreamSseOrchestrator
{
    private async Task StreamAndFinalizeAsync(
        StreamReadingResult streamResult,
        AiStreamOrchestrationRequest request,
        HttpContext httpContext,
        CancellationToken requestToken)
    {
        var state = new StreamExecutionState();
        var completionContext = new StreamCompletionContext(
            streamResult.AiRequestId,
            request.UserId,
            request.SessionId,
            state,
            streamResult.EstimatedInputTokens);

        try
        {
            await WriteStreamAsync(httpContext.Response, streamResult.Stream, state, requestToken);
            await WriteDoneEventAsync(httpContext.Response, requestToken);

            await CompleteStreamAsync(new StreamCompletionDispatch(
                streamResult.AiRequestId,
                request.UserId,
                request.FollowUpQuestion,
                state,
                streamResult.EstimatedInputTokens,
                AiStreamFinalStatuses.Completed,
                ErrorMessage: null,
                IsClientDisconnect: false,
                CancellationToken.None));
        }
        catch (OperationCanceledException ex)
        {
            await HandleCanceledStreamAsync(ex, completionContext, httpContext, requestToken);
        }
        catch (Exception ex)
        {
            await HandleFailedStreamAsync(ex, completionContext, httpContext.Response, CancellationToken.None);
        }
    }

    private async Task WriteStreamAsync(
        HttpResponse response,
        IAsyncEnumerable<string> stream,
        StreamExecutionState state,
        CancellationToken cancellationToken)
    {
        await foreach (var chunk in stream.WithCancellation(cancellationToken))
        {
            if (state.FirstTokenAt is null)
            {
                state.FirstTokenAt = DateTimeOffset.UtcNow;
            }

            state.FullResponseBuilder.Append(chunk);
            state.OutputTokens += EstimateTokenCount(chunk);

            var sanitizedChunk = chunk.Replace("\n", "\\n");
            await WriteServerEventAsync(response, sanitizedChunk, cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }
    }

    private Task WriteDoneEventAsync(HttpResponse response, CancellationToken cancellationToken)
    {
        return WriteServerEventAsync(response, "[DONE]", cancellationToken);
    }

    private static Task WriteServerEventAsync(HttpResponse response, string payload, CancellationToken cancellationToken)
    {
        return response.WriteAsync($"data: {payload}\n\n", cancellationToken);
    }

    private sealed class StreamExecutionState
    {
        public DateTimeOffset? FirstTokenAt { get; set; }

        public int OutputTokens { get; set; }

        public System.Text.StringBuilder FullResponseBuilder { get; } = new();
    }

    private readonly record struct StreamCompletionContext(
        Guid AiRequestId,
        Guid UserId,
        string SessionId,
        StreamExecutionState State,
        int InputTokens);

    private static int EstimateTokenCount(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return 0;
        }

        var normalizedLength = content.Trim().Length;
        return Math.Max(1, (int)Math.Ceiling(normalizedLength / 4d));
    }
}
