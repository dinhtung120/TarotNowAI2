/*
 * ===================================================================
 * FILE: RegisterResponse.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Register
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kết quả trả về cho Frontend khi Đăng Ký Tài Khoản bước 1 Thành Công.
 *   
 * TẠI SAO LẠI KHÔNG TRẢ VỀ TOKEN ĐỂ ĐĂNG NHẬP LUÔN VẬY?
 *   Theo chuẩn an ninh mảng tài chính/app dịch vụ Tarot:
 *   User VỪA ĐĂNG KÝ XONG thì trạng thái lúc này là PENDING.
 *   User bắt buộc phải vào Email của mình click mã xác nhận -> lúc đó 
 *   mới tính là ACTIVE. Cho nên bước Đăng Ký API chỉ trả Guid ID và 
 *   lời nhắn "Bạn nhớ kiểm tra hòm thư nhé", chứ chưa trả ngay Access Token.
 * ===================================================================
 */

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// Gói dữ liệu chuyển đổi mang nội dung xác nhận Đăng Ký thông suốt.
/// </summary>
public class RegisterResponse
{
    /// <summary>
    /// ID Thẻ định danh của Người dùng này. 
    /// UI dùng Guid này để gán vào màn hình nhập OTP cho bước kế tiếp.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Nội dung thân thiện hướng dẫn User sang bước tiếp (Kiểm tra Email).
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
