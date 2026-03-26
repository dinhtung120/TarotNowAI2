namespace TarotNow.Api.Controllers;

public partial class AiController
{
    private async Task WriteStreamAsync(
        IAsyncEnumerable<string> stream,
        StreamExecutionState state,
        CancellationToken cancellationToken)
    {
        await foreach (var chunk in stream.WithCancellation(cancellationToken))
        {
            if (state.FirstTokenAt == null)
            {
                state.FirstTokenAt = DateTimeOffset.UtcNow;
            }

            state.FullResponseBuilder.Append(chunk);
            state.OutputTokens++;

            var sanitizedChunk = chunk.Replace("\n", "\\n");
            await WriteServerEventAsync(sanitizedChunk, cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    private Task WriteDoneEventAsync(CancellationToken cancellationToken)
    {
        return WriteServerEventAsync("[DONE]", cancellationToken);
    }

    private Task WriteServerEventAsync(string payload, CancellationToken cancellationToken)
    {
        return Response.WriteAsync($"data: {payload}\n\n", cancellationToken);
    }

    private sealed class StreamExecutionState
    {
        public DateTimeOffset? FirstTokenAt { get; set; }

        public int OutputTokens { get; set; }

        public System.Text.StringBuilder FullResponseBuilder { get; } = new();
    }
}
