using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class LegalVersionSettings : ILegalVersionSettings
{
    public LegalVersionSettings(IOptions<LegalSettingsOptions> options)
    {
        var configured = options.Value;
        TOSVersion = ResolveVersion(configured.TOSVersion);
        PrivacyVersion = ResolveVersion(configured.PrivacyVersion);
        AiDisclaimerVersion = ResolveVersion(configured.AiDisclaimerVersion);
    }

    public string TOSVersion { get; }
    public string PrivacyVersion { get; }
    public string AiDisclaimerVersion { get; }

    private static string ResolveVersion(string? configuredValue)
    {
        return string.IsNullOrWhiteSpace(configuredValue) ? "1.0" : configuredValue.Trim();
    }
}
