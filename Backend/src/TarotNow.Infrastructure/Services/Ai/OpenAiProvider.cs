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
/// Implementation của IAiProvider kết nối trực tiếp với API của OpenAI (gpt-4o-mini).
/// Mục đích: Xử lý SSE (Server-Sent Events) Streaming Responses theo chuẩn Chunk.
/// </summary>
public class OpenAiProvider : IAiProvider
{
    private readonly IAiProviderLogRepository _logRepo;
    private readonly ILogger<OpenAiProvider> _logger;
    public string ProviderName => "OpenAI";
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
        _apiKey = configuration["AiProvider:ApiKey"] 
                  ?? throw new ArgumentNullException("AiProvider:ApiKey is missing in AppSettings.json");
        _modelName = configuration["AiProvider:Model"]?.Trim() is { Length: > 0 } model
            ? model
            : "gpt-4o-mini";
        _maxRetries = int.TryParse(configuration["AiProvider:MaxRetries"], out var maxRetries) && maxRetries >= 0
            ? maxRetries
            : 2;
        var timeoutSeconds = int.TryParse(configuration["AiProvider:TimeoutSeconds"], out var configuredTimeout) && configuredTimeout > 0
            ? configuredTimeout
            : 30;
        var baseUrl = configuration["AiProvider:BaseUrl"]?.Trim();
        if (string.IsNullOrWhiteSpace(baseUrl))
            baseUrl = "https://api.openai.com/v1/";

        _httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    /// <summary>
    /// Gửi request tới /chat/completions với cấu hình stream=true.
    /// Yield return từng chunk text để Server-Sent Event có thể push liền lập tức về Frontend.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _modelName,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            temperature = 0.7,
            stream = true // Bắt buộc bật Streaming
        };

        using var response = await SendWithRetryAsync(requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(responseStream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(line)) continue;
            
            // OpenAI trả Chunk có format: `data: {"id":...}`
            if (line.StartsWith("data: "))
            {
                var data = line.Substring(6).Trim();

                if (data == "[DONE]") break; // Dấu hiệu kết thúc luồng

                // Parse Json từng đoạn nhỏ
                var jsonNode = JsonNode.Parse(data);
                if (jsonNode?["choices"]?[0]?["delta"]?["content"] != null)
                {
                    var chunkContent = jsonNode["choices"]![0]!["delta"]!["content"]!.GetValue<string>();
                    
                    // Bắn từng mảnh chữ trực tiếp ra ngoài
                    yield return chunkContent;
                }
            }
        }
    }

    /// <summary>
    /// Extension (internal use): Ghi log vào MongoDB telemetry.
    /// Tại sao không ghi log trực tiếp trong StreamChatAsync? 
    /// -> Để tách biệt logic streaming và logic audit.
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
                PromptVersion = "v1.0" // Cố định phiên bản prompt cho giai đoạn này
            });
        }
        catch
        {
            // Fail-safe: Log telemetry thất bại không được làm sập luồng chính của ứng dụng
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

                return await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (Exception ex) when (IsRetryable(ex, cancellationToken) && attempt < _maxRetries)
            {
                var delayMs = 200 * (attempt + 1);
                _logger.LogWarning(ex, "OpenAI request failed. Retrying attempt {Attempt}/{MaxRetries} after {DelayMs}ms.", attempt + 1, _maxRetries, delayMs);
                await Task.Delay(delayMs, cancellationToken);
            }
        }
    }

    private static bool IsRetryable(Exception ex, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return false;
        return ex is HttpRequestException || ex is TaskCanceledException;
    }

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _modelName;
    private readonly int _maxRetries;
}
