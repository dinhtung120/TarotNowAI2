using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Ai;

/// <summary>
/// Implementation của IAiProvider kết nối trực tiếp với API của OpenAI (gpt-4o-mini).
/// Mục đích: Xử lý SSE (Server-Sent Events) Streaming Responses theo chuẩn Chunk.
/// </summary>
public class OpenAiProvider : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    public string ProviderName => "OpenAI";
    public string ModelName => "gpt-4o-mini"; // Mặc định như đã trao đổi

    public OpenAiProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["AiProvider:ApiKey"] 
                  ?? throw new ArgumentNullException("AiProvider:ApiKey is missing in AppSettings.json");

        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
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
            model = ModelName,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            temperature = 0.7,
            stream = true // Bắt buộc bật Streaming
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
        {
            Content = JsonContent.Create(requestBody)
        };

        // Gửi request lấy Stream (không đợi toàn bộ payload tải về)
        using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
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
}
