
namespace TarotNow.Domain.Enums;

// Tập hằng loại OTP theo nghiệp vụ xác thực.
public static class OtpType
{
    // OTP xác thực email khi đăng ký.
    public const string VerifyEmail = "register";

    // OTP đặt lại mật khẩu.
    public const string ResetPassword = "reset_password";
}
