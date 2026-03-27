namespace TarotNow.Infrastructure.Options;

public class ChatModerationOptions
{
    public bool Enabled { get; set; } = true;

    public int MaxQueueSize { get; set; } = 1000;

    public string[] Keywords { get; set; } = Array.Empty<string>();
}
