namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Data transfer object chứa kết quả sau khi đăng nhập thành công.
/// Gồm Access Token và một số thông tin người dùng được phép hiển thị trên UI.
/// </summary>
public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresInMinutes { get; set; }
    
    // JWT Authentication scheme yêu cầu token
    // (Refresh Token không nằm trong AuthResponse mà được set Header Set-Cookie tại Controller)

    public UserProfileDto User { get; set; } = new();
}

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Status { get; set; } = string.Empty;
}
