using Microsoft.Extensions.Configuration;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class LegalVersionSettings : ILegalVersionSettings
{
    public LegalVersionSettings(IConfiguration configuration)
    {
        TOSVersion = ResolveVersion(configuration["LegalSettings:TOSVersion"]);
        PrivacyVersion = ResolveVersion(configuration["LegalSettings:PrivacyVersion"]);
        AiDisclaimerVersion = ResolveVersion(configuration["LegalSettings:AiDisclaimerVersion"]);
    }

    public string TOSVersion { get; }
    public string PrivacyVersion { get; }
    public string AiDisclaimerVersion { get; }

    private static string ResolveVersion(string? configuredValue)
    {
        return string.IsNullOrWhiteSpace(configuredValue) ? "1.0" : configuredValue.Trim();
    }
}
