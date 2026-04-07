namespace TarotNow.Application.Interfaces;

public interface ILegalVersionSettings
{
    string TOSVersion { get; }
    string PrivacyVersion { get; }
    string AiDisclaimerVersion { get; }
}
