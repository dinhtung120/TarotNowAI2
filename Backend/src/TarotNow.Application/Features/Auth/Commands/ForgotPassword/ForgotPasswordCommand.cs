/*
 * ===================================================================
 * FILE: ForgotPasswordCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.ForgotPassword
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command kích hoạt quy trình Quên mật khẩu.
 *   Nhận email từ user → hệ thống sẽ tạo OTP và gửi qua email.
 *
 * BẢO MẬT (SECURITY):
 *   IRequest<bool> trả về boolean (thường luôn là true), kể cả khi 
 *   email không tồn tại. Tại sao?
 *   → Dùng để chống lỗi "User Enumeration" (tin tặc nhập bừa email 
 *   để xem email nào đã đăng ký hệ thống). Response luôn như nhau 
 *   giúp bảo mật thông tin khách hàng.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Command chứa dữ liệu yêu cầu quên mật khẩu.
/// </summary>
public class ForgotPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Email của user cần khôi phục mật khẩu.
    /// Email này sẽ được dùng để tìm account và gửi mã OTP.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
