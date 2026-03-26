/*
 * FILE: HmacPaymentGatewayService.cs
 * MỤC ĐÍCH: Service xử lý ký/verify HMAC cho webhook cổng thanh toán.
 *   Khi cổng thanh toán gửi webhook (xác nhận User đã nạp tiền thành công),
 *   webhook đi kèm chữ ký HMAC → cần verify để đảm bảo webhook là thật (không bị giả mạo).
 *
 *   CÁCH HOẠT ĐỘNG:
 *   → Cổng thanh toán dùng secret key → HMAC-SHA256(payload) → sinh chữ ký
 *   → Gửi webhook kèm header chữ ký (X-Signature hoặc tên tùy cổng)
 *   → Server nhận webhook → dùng CÙNG secret key → HMAC-SHA256(payload) → so sánh
 *   → Nếu khớp → webhook hợp lệ → xử lý nạp tiền
 *   → Nếu không khớp → từ chối (có thể là tấn công giả mạo)
 *
 *   BẢO MẬT:
 *   → Secret key ≥ 16 ký tự, không chứa "REPLACE" (anti-placeholder check).
 *   → FixedTimeEquals: so sánh constant-time → chống timing attack.
 *   → Lower-case normalize trước khi so sánh → tránh lỗi case-sensitivity.
 */

using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implement IPaymentGatewayService — verify HMAC chữ ký webhook thanh toán.
/// </summary>
public class HmacPaymentGatewayService : IPaymentGatewayService
{
    private readonly string _secretKey;

    public HmacPaymentGatewayService(IOptions<PaymentGatewayOptions> options)
    {
        var paymentOptions = options.Value;
        // Đọc secret key từ config — bắt buộc, throw nếu thiếu
        _secretKey = paymentOptions.WebhookSecret?.Trim()
            ?? throw new InvalidOperationException("Missing PaymentGateway:WebhookSecret configuration.");

        // Kiểm tra secret key đủ mạnh: ≥16 ký tự + không phải placeholder
        if (_secretKey.Length < 16 || _secretKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("PaymentGateway:WebhookSecret is not strong enough.");
    }

    /// <summary>
    /// Verify chữ ký HMAC-SHA256 của webhook payload.
    /// 
    /// Quy trình:
    /// 1. HMAC-SHA256(secretKey, payload) → hash bytes
    /// 2. Chuyển hash → hex string lowercase (vd: "a1b2c3...")
    /// 3. So sánh với signature từ webhook header
    /// 4. FixedTimeEquals: so sánh constant-time → chống timing attack
    ///
    /// Tại sao dùng FixedTimeEquals thay vì ==?
    /// → String comparison thường (==) dừng sớm khi gặp ký tự khác → attacker
    ///   có thể đo thời gian để đoán từng byte → timing attack.
    /// → FixedTimeEquals luôn so sánh HẾT TOÀN BỘ → thời gian không đổi.
    /// </summary>
    public bool VerifyWebhookSignature(string payload, string signature)
    {
        if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature))
            return false;

        // Tính HMAC-SHA256 từ payload + secret key
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computedSignature = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        var providedSignature = signature.Trim().ToLowerInvariant();

        // Constant-time comparison — chống timing attack
        var computedBytes = Encoding.UTF8.GetBytes(computedSignature);
        var providedBytes = Encoding.UTF8.GetBytes(providedSignature);

        return computedBytes.Length == providedBytes.Length
               && CryptographicOperations.FixedTimeEquals(computedBytes, providedBytes);
    }
}
