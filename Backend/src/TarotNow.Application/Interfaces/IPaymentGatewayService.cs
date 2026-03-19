/*
 * ===================================================================
 * FILE: IPaymentGatewayService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Tiếp Đứng Cửa Khẩu Với Các Cổng Thanh Toán Bên Thứ 3 (VNPAY, Momo, Stripe,...).
 *   Có Trách Nhiệm Đóng Dấu Xét Duyệt Kẻ Lạ Mặt Cho Thằng Webhook Thôi Không Cho Hacker Giả Dạng Gọi Lệnh Tính Tiền Khống.
 * ===================================================================
 */

using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Gác Cổng Hải Quan (Thẩm Định Chữ Ký Ngân Hàng).
/// Xây Dựng 1 Cái Khuôn Ở Application Tránh Bị Dính Liền Code Với API Của Thằng Stripe Hay VNPAY Cụ Thể Nào.
/// </summary>
public interface IPaymentGatewayService
{
    /// <summary>
    /// Rọi Đèn Pin Chữ Ký: Tên Khách Tự Nhận Là Momo Trình Thẻ (header signature). 
    /// Ta Lấy Bí Mật Mở Mẫu Webhook Của Mình Ở Backend Mã Hóa HMAC Thử Coi Cơ Nhớ Khớp Vơi Bên Kia Nộp Giấy Hông.
    /// Nếu Giả Mạo -> Fail Cái Rụp.
    /// </summary>
    /// <param name="payload">Nguyên Cục Hàng Json Nó Quăng Vô Không Sửa Đổi Lời Nào (Nguyên Đai Nguyên Kiện).</param>
    /// <param name="signature">Chữ Ký Mã Hóa (HMAC SHA256) Nó Đính Rời Trên Thùng Hàng (Thường Nằm Ở Header Http).</param>
    /// <returns>Chính Phủ Đã Cấp Phép Cứ Qua Đi (True), Hoặc Giam Cổ Thằng Lừa Đảo Lại (False).</returns>
    bool VerifyWebhookSignature(string payload, string signature);
}
