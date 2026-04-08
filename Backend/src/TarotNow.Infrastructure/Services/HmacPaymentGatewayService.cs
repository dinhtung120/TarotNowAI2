using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

// Service xác minh chữ ký webhook thanh toán bằng HMAC-SHA256.
public class HmacPaymentGatewayService : IPaymentGatewayService
{
    // Secret dùng để tính HMAC, được nạp từ cấu hình an toàn.
    private readonly string _secretKey;

    /// <summary>
    /// Khởi tạo service xác minh webhook và kiểm tra cấu hình secret.
    /// Luồng fail-fast khi secret thiếu hoặc quá yếu để tránh chấp nhận webhook giả mạo.
    /// </summary>
    public HmacPaymentGatewayService(IOptions<PaymentGatewayOptions> options)
    {
        var paymentOptions = options.Value;

        _secretKey = paymentOptions.WebhookSecret?.Trim()
            ?? throw new InvalidOperationException("Missing PaymentGateway:WebhookSecret configuration.");

        // Chặn secret mẫu/yếu để giảm rủi ro bypass xác minh chữ ký.
        if (_secretKey.Length < 16 || _secretKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("PaymentGateway:WebhookSecret is not strong enough.");
    }

    /// <summary>
    /// Xác minh chữ ký webhook dựa trên payload và chữ ký gửi kèm.
    /// Luồng tạo chữ ký nội bộ từ payload rồi so sánh constant-time để chống timing attack.
    /// </summary>
    public bool VerifyWebhookSignature(string payload, string signature)
    {
        if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature))
        {
            // Payload hoặc signature trống luôn bị từ chối để tránh xác minh sai.
            return false;
        }

        // Tính chữ ký HMAC chuẩn từ payload nhận được.
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computedSignature = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        var providedSignature = signature.Trim().ToLowerInvariant();

        // Chuyển sang byte để dùng so sánh constant-time tránh rò rỉ thời gian.
        var computedBytes = Encoding.UTF8.GetBytes(computedSignature);
        var providedBytes = Encoding.UTF8.GetBytes(providedSignature);

        return computedBytes.Length == providedBytes.Length
               && CryptographicOperations.FixedTimeEquals(computedBytes, providedBytes);
    }
}
