using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace TarotNow.Infrastructure.Services.Ai;

public partial class OpenAiProvider
{
    public async IAsyncEnumerable<string> StreamChatAsync(
        string systemPrompt,
        string userPrompt,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var requestBody = CreateRequestBody(systemPrompt, userPrompt);

        using var response = await SendWithRetryAsync(requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(responseStream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            var chunkContent = TryReadChunkContent(line);
            if (chunkContent == StreamChunkDone)
            {
                yield break;
            }

            if (chunkContent != null)
            {
                yield return chunkContent;
            }
        }
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(object requestBody, CancellationToken cancellationToken)
    {
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
                {
                    Content = JsonContent.Create(requestBody)
                };

                return await _httpClient.SendAsync(
                    requestMessage,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);
            }
            catch (Exception ex) when (IsRetryable(ex, cancellationToken) && attempt < _maxRetries)
            {
                var delayMs = 200 * (attempt + 1);
                _logger.LogWarning(
                    ex,
                    "OpenAI request failed. Retrying attempt {Attempt}/{MaxRetries} after {DelayMs}ms.",
                    attempt + 1,
                    _maxRetries,
                    delayMs);

                await Task.Delay(delayMs, cancellationToken);
            }
        }
    }

    private static bool IsRetryable(Exception ex, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        return ex is HttpRequestException or TaskCanceledException;
    }

    private static readonly string StreamChunkDone = "__DONE__";

    private object CreateRequestBody(string systemPrompt, string userPrompt)
    {
        return new
        {
            model = _modelName,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            temperature = 0.7,
            stream = true
        };
    }

    private static string? TryReadChunkContent(string? line)
    {
        if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ", StringComparison.Ordinal))
        {
            return null;
        }

        var data = line[6..].Trim();
        if (data == "[DONE]")
        {
            return StreamChunkDone;
        }

        return JsonNode.Parse(data)?["choices"]?[0]?["delta"]?["content"]?.GetValue<string>();
    }
}
