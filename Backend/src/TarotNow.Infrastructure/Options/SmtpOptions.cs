namespace TarotNow.Infrastructure.Options;

// Options cấu hình SMTP gửi email hệ thống.
public sealed class SmtpOptions
{
    // Host SMTP.
    public string Host { get; set; } = "smtp.gmail.com";

    // Port SMTP.
    public int Port { get; set; } = 587;

    // Username đăng nhập SMTP.
    public string Username { get; set; } = string.Empty;

    // Password/app password SMTP.
    public string Password { get; set; } = string.Empty;

    // Email người gửi mặc định.
    public string SenderEmail { get; set; } = "no-reply@tarotnow.vn";

    // Tên hiển thị người gửi mặc định.
    public string SenderName { get; set; } = "TarotNow Security";
}
