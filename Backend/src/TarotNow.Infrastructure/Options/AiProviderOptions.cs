namespace TarotNow.Infrastructure.Options;

public sealed class AiProviderOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openai.com/v1/";
    public string Model { get; set; } = "gpt-4o-mini";
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetries { get; set; } = 2;
}

