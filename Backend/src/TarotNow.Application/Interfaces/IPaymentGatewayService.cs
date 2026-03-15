using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IPaymentGatewayService
{
    /// <summary>
    /// Xác thực chữ ký webhook từ Payment Gateway.
    /// </summary>
    /// <param name="payload">Nội dung webhook gốc (thường là raw body json).</param>
    /// <param name="signature">Chữ ký được gửi kèm header hoặc trong payload.</param>
    /// <returns>True nếu chữ ký hợp lệ, False nếu không hợp lệ.</returns>
    bool VerifyWebhookSignature(string payload, string signature);
}
