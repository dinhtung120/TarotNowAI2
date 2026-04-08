namespace TarotNow.Api.Contracts.Requests;

// Payload xác nhận MFA sau khi người dùng nhận mã.
public class MfaVerifyBody
{
    // Mã OTP/TOTP dùng để hoàn tất xác minh MFA.
    public string Code { get; set; } = string.Empty;
}
