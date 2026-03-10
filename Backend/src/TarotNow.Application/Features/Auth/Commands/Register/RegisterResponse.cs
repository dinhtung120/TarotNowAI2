namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// Response trả về sau khi đăng ký thành công.
/// Vì hệ thống yêu cầu verify OTP mới cấp access token nên ta chỉ trả về thông điệp.
/// </summary>
public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}
