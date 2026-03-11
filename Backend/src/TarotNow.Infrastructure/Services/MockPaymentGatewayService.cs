using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class MockPaymentGatewayService : IPaymentGatewayService
{
    private readonly string _secretKey;

    public MockPaymentGatewayService(IConfiguration configuration)
    {
        // Thông thường chuỗi Secret sẽ lấy từ provider, ví dụ "Secret123"
        _secretKey = configuration["PaymentGateway:WebhookSecret"] ?? "default_secret";
    }

    public bool VerifyWebhookSignature(string payload, string signature)
    {
        // Default dev behavior to bypass signature for simple testing
        if (signature == "TEST_BYPASS_SIG" || signature == "test_bypass")
            return true;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computedSignature = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        return computedSignature == signature.ToLowerInvariant();
    }
}
