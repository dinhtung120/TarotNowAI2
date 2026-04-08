

using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract xác thực webhook cổng thanh toán để đảm bảo callback đến từ nguồn tin cậy.
public interface IPaymentGatewayService
{
    /// <summary>
    /// Xác minh chữ ký webhook trước khi chấp nhận xử lý giao dịch.
    /// Luồng xử lý: dùng payload và signature để kiểm tra tính toàn vẹn và nguồn phát sinh.
    /// </summary>
    bool VerifyWebhookSignature(string payload, string signature);
}
