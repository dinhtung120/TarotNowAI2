

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Register;

// Command đăng ký tài khoản người dùng mới.
public class RegisterCommand : IRequest<Guid>
{
    // Email tài khoản.
    public string Email { get; set; } = string.Empty;

    // Username tài khoản.
    public string Username { get; set; } = string.Empty;

    // Mật khẩu thô để băm trước khi lưu.
    public string Password { get; set; } = string.Empty;

    // Tên hiển thị của người dùng.
    public string DisplayName { get; set; } = string.Empty;

    // Ngày sinh người dùng.
    public DateTime DateOfBirth { get; set; }

    // Cờ xác nhận đã đồng ý điều khoản sử dụng.
    public bool HasConsented { get; set; }
}
