namespace TarotNow.Api.Options;

public sealed class ForwardedHeadersRuntimeOptions
{
    public int ForwardLimit { get; set; } = 2;
}

public sealed class SignalRRuntimeOptions
{
    public long MaximumReceiveMessageSizeBytes { get; set; } = 10 * 1024 * 1024;
}
