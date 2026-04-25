using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TarotNow.Infrastructure.Services.Ai;

public partial class OpenAiProvider
{
    /// <summary>
    /// Stream phản hồi chat từ OpenAI theo từng chunk text.
    /// Luồng gửi request dạng stream, đọc tuần tự từng dòng SSE và yield nội dung hợp lệ về caller.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(
        string systemPrompt,
        string userPrompt,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // Dựng payload chuẩn để OpenAI trả dữ liệu theo chế độ stream.
        var requestBody = CreateRequestBody(systemPrompt, userPrompt);

        using var response = await SendWithRetryAsync(requestBody, cancellationToken);
        // Fail-fast nếu HTTP không thành công để caller xử lý lỗi sớm.
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(responseStream);

        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line == null)
            {
                // Kết thúc stream từ server thì thoát vòng lặp đọc dữ liệu.
                break;
            }

            var chunkContent = TryReadChunkContent(line);
            if (chunkContent == StreamChunkDone)
            {
                // Nhận tín hiệu [DONE] từ OpenAI thì dừng stream có kiểm soát.
                yield break;
            }

            if (chunkContent != null)
            {
                // Chỉ phát chunk có nội dung để tránh đẩy dòng keep-alive vô nghĩa.
                yield return chunkContent;
            }
        }
    }

    /// <summary>
    /// Gửi request tới OpenAI với cơ chế retry tuyến tính cho lỗi tạm thời.
    /// Luồng retry giới hạn số lần để giảm rủi ro treo request dài.
    /// </summary>
    private async Task<HttpResponseMessage> SendWithRetryAsync(object requestBody, CancellationToken cancellationToken)
    {
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                // Tạo request mới mỗi lần thử lại để tránh tái sử dụng content đã consume.
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
                {
                    Content = JsonContent.Create(requestBody)
                };

                var response = await _httpClient.SendAsync(
                    requestMessage,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                if (IsRetryableStatusCode(response.StatusCode) && attempt < _maxRetries)
                {
                    var delayMs = CalculateRetryDelayMs(attempt);
                    _logger.LogWarning(
                        "OpenAI returned retryable status {StatusCode}. Retrying attempt {Attempt}/{MaxRetries} after {DelayMs}ms.",
                        (int)response.StatusCode,
                        attempt + 1,
                        _maxRetries,
                        delayMs);
                    response.Dispose();
                    await Task.Delay(delayMs, cancellationToken);
                    continue;
                }

                return response;
            }
            catch (Exception ex) when (IsRetryable(ex, cancellationToken) && attempt < _maxRetries)
            {
                var delayMs = CalculateRetryDelayMs(attempt);
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

    /// <summary>
    /// Xác định lỗi có thể retry hay phải dừng ngay.
    /// Luồng ưu tiên dừng khi request đã bị hủy để không retry vô ích.
    /// </summary>
    private static bool IsRetryable(Exception ex, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        return ex is HttpRequestException or TaskCanceledException;
    }

    private static bool IsRetryableStatusCode(HttpStatusCode statusCode)
    {
        var code = (int)statusCode;
        return code == 408 || code == 429 || code >= 500;
    }

    private int CalculateRetryDelayMs(int attempt)
    {
        var exponential = _streamingRetryBaseDelayMs * (int)Math.Pow(2, attempt);
        var jitter = Random.Shared.Next(0, Math.Max(1, _streamingRetryBaseDelayMs));
        return Math.Clamp(exponential + jitter, _streamingRetryBaseDelayMs, 30_000);
    }

    // Sentinel nội bộ để biểu diễn tín hiệu kết thúc stream.
    private static readonly string StreamChunkDone = "__DONE__";

    /// <summary>
    /// Tạo payload chat completion theo cấu hình provider hiện tại.
    /// Luồng này gom prompt hệ thống và prompt người dùng vào đúng định dạng OpenAI API.
    /// </summary>
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
            temperature = _streamingTemperature,
            stream = true
        };
    }

    /// <summary>
    /// Trích nội dung text từ một dòng SSE trả về.
    /// Luồng bỏ qua dòng không hợp lệ, nhận diện tín hiệu [DONE], và parse JSON delta content.
    /// </summary>
    private string? TryReadChunkContent(string? line)
    {
        if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ", StringComparison.Ordinal))
        {
            // Dòng rỗng/không đúng tiền tố SSE không chứa nội dung cần phát.
            return null;
        }

        var data = line[6..].Trim();
        if (data == "[DONE]")
        {
            return StreamChunkDone;
        }

        try
        {
            // JSON delta có thể không có content ở một số chunk, khi đó trả null để bỏ qua.
            return JsonNode.Parse(data)?["choices"]?[0]?["delta"]?["content"]?.GetValue<string>();
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Malformed OpenAI stream chunk detected and skipped.");
            return null;
        }
    }
}
