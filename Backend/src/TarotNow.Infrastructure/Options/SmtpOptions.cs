namespace TarotNow.Infrastructure.Options;

public sealed class SmtpOptions
{
    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = "no-reply@tarotnow.vn";
    public string SenderName { get; set; } = "TarotNow Security";
}
