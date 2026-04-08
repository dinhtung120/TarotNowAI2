namespace TarotNow.Infrastructure.Options;

public sealed class LiveKitOptions
{
    public string Url { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string ApiSecret { get; set; } = string.Empty;

    public int TokenTtlMinutes { get; set; } = 30;
}
