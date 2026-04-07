

using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

public class HmacPaymentGatewayService : IPaymentGatewayService
{
    private readonly string _secretKey;

    public HmacPaymentGatewayService(IOptions<PaymentGatewayOptions> options)
    {
        var paymentOptions = options.Value;
        
        _secretKey = paymentOptions.WebhookSecret?.Trim()
            ?? throw new InvalidOperationException("Missing PaymentGateway:WebhookSecret configuration.");

        
        if (_secretKey.Length < 16 || _secretKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("PaymentGateway:WebhookSecret is not strong enough.");
    }

        public bool VerifyWebhookSignature(string payload, string signature)
    {
        if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature))
            return false;

        
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computedSignature = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        var providedSignature = signature.Trim().ToLowerInvariant();

        
        var computedBytes = Encoding.UTF8.GetBytes(computedSignature);
        var providedBytes = Encoding.UTF8.GetBytes(providedSignature);

        return computedBytes.Length == providedBytes.Length
               && CryptographicOperations.FixedTimeEquals(computedBytes, providedBytes);
    }
}
