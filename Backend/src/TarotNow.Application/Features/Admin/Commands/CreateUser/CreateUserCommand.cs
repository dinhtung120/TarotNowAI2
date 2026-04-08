using MediatR;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

// Command tạo người dùng mới từ màn quản trị.
public class CreateUserCommand : IRequest<Guid>
{
    // Email đăng nhập của tài khoản mới.
    public string Email { get; set; } = string.Empty;
    // Username duy nhất dùng cho định danh hiển thị và đăng nhập.
    public string Username { get; set; } = string.Empty;
    // Tên hiển thị của tài khoản.
    public string DisplayName { get; set; } = string.Empty;
    // Mật khẩu thô đầu vào để băm trước khi lưu.
    public string Password { get; set; } = string.Empty;
    // Vai trò khởi tạo ban đầu (user/tarot_reader/admin).
    public string Role { get; set; } = UserRole.User;
}
