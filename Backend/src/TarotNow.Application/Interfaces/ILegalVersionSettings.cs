namespace TarotNow.Application.Interfaces;

/// <summary>
/// Phiên bản tài liệu pháp lý hiện hành mà hệ thống yêu cầu user đã đồng ý.
/// </summary>
public interface ILegalVersionSettings
{
    string TOSVersion { get; }
    string PrivacyVersion { get; }
    string AiDisclaimerVersion { get; }
}
