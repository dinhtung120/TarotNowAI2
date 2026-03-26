using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Ai;

public partial class OpenAiProvider : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly IAiProviderLogRepository _logRepo;
    private readonly ILogger<OpenAiProvider> _logger;
    private readonly string _modelName;
    private readonly int _maxRetries;

    public string ProviderName => "OpenAI";

    public string ModelName => _modelName;

    public OpenAiProvider(
        HttpClient httpClient,
        IOptions<AiProviderOptions> options,
        IAiProviderLogRepository logRepo,
        ILogger<OpenAiProvider> logger)
    {
        _httpClient = httpClient;
        _logRepo = logRepo;
        _logger = logger;

        var providerOptions = options.Value;
        var apiKey = providerOptions.ApiKey
                     ?? throw new ArgumentNullException("AiProvider:ApiKey is missing in AppSettings.json");

        _modelName = providerOptions.Model?.Trim() is { Length: > 0 } configuredModel
            ? configuredModel
            : "gpt-4o-mini";

        _maxRetries = providerOptions.MaxRetries >= 0 ? providerOptions.MaxRetries : 2;

        var timeoutSeconds = providerOptions.TimeoutSeconds > 0 ? providerOptions.TimeoutSeconds : 30;
        var baseUrl = providerOptions.BaseUrl?.Trim();
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            baseUrl = "https://api.openai.com/v1/";
        }

        _httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }
}
