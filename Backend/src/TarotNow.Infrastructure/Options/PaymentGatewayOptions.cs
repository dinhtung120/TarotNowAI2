namespace TarotNow.Infrastructure.Options;

public sealed class PaymentGatewayOptions
{
    public string WebhookSecret { get; set; } = string.Empty;
}

