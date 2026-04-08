namespace TarotNow.Application.Features.Auth.Commands.Login;

// DTO phản hồi xác thực thành công.
public class AuthResponse
{
    // Access token JWT dùng cho các request đã đăng nhập.
    public string AccessToken { get; set; } = string.Empty;

    // Loại token trả về (mặc định Bearer).
    public string TokenType { get; set; } = "Bearer";

    // Thời gian hết hạn access token theo phút/giây tùy contract hiện tại.
    public int ExpiresIn { get; set; }

    // Hồ sơ người dùng rút gọn để client hydrate session.
    public UserProfileDto User { get; set; } = new();
}

// DTO hồ sơ người dùng trả kèm trong phản hồi đăng nhập/refresh.
public class UserProfileDto
{
    // Định danh người dùng.
    public Guid Id { get; set; }

    // Username.
    public string Username { get; set; } = string.Empty;

    // Tên hiển thị.
    public string DisplayName { get; set; } = string.Empty;

    // Email.
    public string Email { get; set; } = string.Empty;

    // URL ảnh đại diện.
    public string? AvatarUrl { get; set; }

    // Cấp độ hiện tại.
    public int Level { get; set; }

    // Điểm kinh nghiệm hiện tại.
    public long Exp { get; set; }

    // Vai trò tài khoản.
    public string Role { get; set; } = string.Empty;

    // Trạng thái tài khoản.
    public string Status { get; set; } = string.Empty;
}
