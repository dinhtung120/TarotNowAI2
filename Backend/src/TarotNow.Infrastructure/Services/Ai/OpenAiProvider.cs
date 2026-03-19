/*
 * FILE: OpenAiProvider.cs
 * MỤC ĐÍCH: Adapter kết nối OpenAI API để sinh nội dung/stream kết quả Tarot.
 *   Đây là ADAPTER duy nhất cho AI provider — dễ dàng thay thế bằng Claude, Gemini, v.v.
 *
 *   CÁC CHỨC NĂNG:
 *   → StreamChatAsync: gửi prompt → nhận SSE stream → yield return từng chunk text
 *   → LogRequestAsync: ghi telemetry vào MongoDB (token count, latency, status)
 *   → SendWithRetryAsync: gửi HTTP request với controlled retry (exponential backoff)
 *
 *   STREAMING (SSE - Server-Sent Events):
 *   → OpenAI trả response dạng stream: mỗi dòng "data: {...}" chứa 1 chunk text.
 *   → IAsyncEnumerable<string>: yield return từng chunk → Middleware đẩy về Frontend real-time.
 *   → "[DONE]": marker kết thúc stream.
 *
 *   RETRY STRATEGY:
 *   → MaxRetries (mặc định 2): chỉ retry khi lỗi retryable (network, timeout).
 *   → Delay tăng dần: 200ms, 400ms, 600ms... (linear backoff).
 *   → KHÔNG retry khi user cancel (CancellationToken).
 *
 *   CẤU HÌNH (appsettings.json):
 *   → AiProvider:ApiKey: API key của OpenAI
 *   → AiProvider:Model: model name (mặc định "gpt-4o-mini")
 *   → AiProvider:MaxRetries: số lần retry (mặc định 2)
 *   → AiProvider:TimeoutSeconds: timeout (mặc định 30s)
 *   → AiProvider:BaseUrl: base URL (mặc định OpenAI, có thể đổi sang proxy)
 */

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Ai;

/// <summary>
/// Implement IAiProvider — adapter kết nối OpenAI API.
/// Dùng HttpClient để gọi /chat/completions với stream=true.
/// </summary>
public class OpenAiProvider : IAiProvider
{
    private readonly IAiProviderLogRepository _logRepo;
    private readonly ILogger<OpenAiProvider> _logger;

    /// <summary>Tên provider — dùng trong telemetry log.</summary>
    public string ProviderName => "OpenAI";
    /// <summary>Tên model đang sử dụng — cấu hình từ appsettings.</summary>
    public string ModelName => _modelName;

    public OpenAiProvider(
        HttpClient httpClient,
        IConfiguration configuration,
        IAiProviderLogRepository logRepo,
        ILogger<OpenAiProvider> logger)
    {
        _httpClient = httpClient;
        _logRepo = logRepo;
        _logger = logger;

        // Đọc cấu hình bắt buộc
        _apiKey = configuration["AiProvider:ApiKey"] 
                  ?? throw new ArgumentNullException("AiProvider:ApiKey is missing in AppSettings.json");

        // Model: mặc định gpt-4o-mini nếu không cấu hình
        _modelName = configuration["AiProvider:Model"]?.Trim() is { Length: > 0 } model
            ? model
            : "gpt-4o-mini";

        // Retry: mặc định 2 lần
        _maxRetries = int.TryParse(configuration["AiProvider:MaxRetries"], out var maxRetries) && maxRetries >= 0
            ? maxRetries
            : 2;

        // Timeout: mặc định 30 giây
        var timeoutSeconds = int.TryParse(configuration["AiProvider:TimeoutSeconds"], out var configuredTimeout) && configuredTimeout > 0
            ? configuredTimeout
            : 30;

        // Base URL: mặc định OpenAI, có thể đổi sang proxy/self-hosted
        var baseUrl = configuration["AiProvider:BaseUrl"]?.Trim();
        if (string.IsNullOrWhiteSpace(baseUrl))
            baseUrl = "https://api.openai.com/v1/";

        // Cấu hình HttpClient
        _httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    /// <summary>
    /// STREAMING: Gửi prompt → nhận SSE → yield return từng chunk text.
    ///
    /// Luồng hoạt động:
    /// 1. Tạo request body với stream=true + system prompt + user prompt
    /// 2. Gửi HTTP POST (với retry) → nhận response stream
    /// 3. Đọc từng dòng: "data: {...}" → parse JSON → trích content
    /// 4. yield return content → Middleware push về Frontend real-time
    /// 5. Gặp "data: [DONE]" → kết thúc
    ///
    /// IAsyncEnumerable: C# 8.0 async iterator — cho phép stream từng item.
    /// [EnumeratorCancellation]: tự động truyền CancellationToken khi caller cancel.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _modelName,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },  // System prompt: hướng dẫn AI vai trò
                new { role = "user", content = userPrompt }       // User prompt: câu hỏi cụ thể
            },
            temperature = 0.7, // Sáng tạo vừa phải (0=deterministic, 1=random)
            stream = true      // BẮT BUỘC bật streaming cho SSE
        };

        // Gửi request với retry strategy
        using var response = await SendWithRetryAsync(requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        // Đọc response stream (không buffer toàn bộ → tối ưu bộ nhớ)
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(responseStream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(line)) continue;
            
            // OpenAI SSE format: "data: {JSON}" hoặc "data: [DONE]"
            if (line.StartsWith("data: "))
            {
                var data = line.Substring(6).Trim();

                if (data == "[DONE]") break; // Marker kết thúc stream

                // Parse JSON → trích nội dung từ choices[0].delta.content
                var jsonNode = JsonNode.Parse(data);
                if (jsonNode?["choices"]?[0]?["delta"]?["content"] != null)
                {
                    var chunkContent = jsonNode["choices"]![0]!["delta"]!["content"]!.GetValue<string>();
                    
                    // Yield return: đẩy chunk ra ngoài → Middleware push SSE về Frontend
                    yield return chunkContent;
                }
            }
        }
    }

    /// <summary>
    /// Ghi telemetry log vào MongoDB (ai_provider_logs).
    /// Tại sao tách riêng thay vì ghi trong StreamChatAsync?
    ///   → Separation of concerns: streaming logic ≠ audit logic
    ///   → Caller (Command handler) quyết định khi nào ghi log (sau khi stream xong)
    /// 
    /// Fail-safe: nếu ghi log thất bại → catch im lặng → KHÔNG làm sập luồng chính.
    /// </summary>
    public async Task LogRequestAsync(Guid userId, string? sessionId, string? requestId, int inputTokens, int outputTokens, int latencyMs, string status, string? errorCode = null)
    {
        try
        {
            await _logRepo.CreateAsync(new AiProviderLogCreateDto
            {
                UserId = userId,
                ReadingRef = sessionId,
                AiRequestRef = requestId,
                Model = _modelName,
                InputTokens = inputTokens,
                OutputTokens = outputTokens,
                LatencyMs = latencyMs,
                Status = status,
                ErrorCode = errorCode,
                PromptVersion = "v1.0" // Phiên bản prompt — tracking để A/B test
            });
        }
        catch
        {
            // Fail-safe: telemetry KHÔNG ĐƯỢC làm sập ứng dụng
        }
    }

    /// <summary>
    /// Gửi HTTP POST với controlled retry.
    /// Linear backoff: 200ms, 400ms, 600ms... (tăng dần).
    /// Chỉ retry khi lỗi retryable (network, timeout) — KHÔNG retry khi user cancel.
    /// ResponseHeadersRead: bắt đầu đọc stream ngay khi có header → không đợi toàn bộ body.
    /// </summary>
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

                // ResponseHeadersRead: stream-friendly — không buffer toàn bộ response
                return await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (Exception ex) when (IsRetryable(ex, cancellationToken) && attempt < _maxRetries)
            {
                // Linear backoff: 200ms → 400ms → 600ms...
                var delayMs = 200 * (attempt + 1);
                _logger.LogWarning(ex, "OpenAI request failed. Retrying attempt {Attempt}/{MaxRetries} after {DelayMs}ms.", attempt + 1, _maxRetries, delayMs);
                await Task.Delay(delayMs, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Kiểm tra lỗi có retryable không — chỉ retry network/timeout errors.
    /// KHÔNG retry khi CancellationToken triggered (user chủ động hủy).
    /// </summary>
    private static bool IsRetryable(Exception ex, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return false;
        return ex is HttpRequestException || ex is TaskCanceledException;
    }

    // ==================== PRIVATE FIELDS ====================
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _modelName;
    private readonly int _maxRetries;
}
