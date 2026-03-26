namespace TarotNow.Infrastructure.Options;

public sealed class SecurityOptions
{
    public string MfaEncryptionKey { get; set; } = string.Empty;
}
