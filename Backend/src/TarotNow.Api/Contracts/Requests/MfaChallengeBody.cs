namespace TarotNow.Api.Contracts.Requests;

// Payload thử thách MFA cho bước xác thực nhạy cảm.
public class MfaChallengeBody
{
    // Mã OTP/TOTP do người dùng nhập để xác thực.
    public string Code { get; set; } = string.Empty;
}
