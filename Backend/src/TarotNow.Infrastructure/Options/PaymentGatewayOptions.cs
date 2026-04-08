namespace TarotNow.Infrastructure.Options;

// Options cấu hình bảo mật webhook cổng thanh toán.
public sealed class PaymentGatewayOptions
{
    // Secret xác minh chữ ký webhook.
    public string WebhookSecret { get; set; } = string.Empty;
}
