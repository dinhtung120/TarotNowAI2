/*
 * ===================================================================
 * FILE: RegisterCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Register
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói cấu trúc chuẩn Form Đăng Ký Tài Khoản cho người dùng mới.
 *   
 * THÔNG TIN THU THẬP:
 *   Yêu cầu Email, Username định danh và thông tin xác thực Password.
 *   HasConsented (Trạng thái xin phép đồng thuận điều khoản) - Giúp 
 *   hệ thống tuân thủ đạo luật GDPR/CCPA. Quy định bắt buộc người dùng 
 *   chọn check mark chấp thuận mới được đăng ký.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// DTO chứa thông tin input của người dùng khi gọi API Register.
/// IRequest<Guid> báo hiệu Command này thành công sẽ trả về con số Guid (Mã ID của người dùng mới).
/// </summary>
public class RegisterCommand : IRequest<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Biệt danh hiển thị lên giao diện (không bắt buộc unique như Username).
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Đồng ý điều khoản Dịch vụ và Chính sách riêng tư.
    /// Giá trị này MUST BE = true, sẽ được chặn ở phần Validation.
    /// </summary>
    public bool HasConsented { get; set; }
}
