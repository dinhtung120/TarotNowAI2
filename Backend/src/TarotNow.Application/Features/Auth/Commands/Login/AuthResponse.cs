

namespace TarotNow.Application.Features.Auth.Commands.Login;

public class AuthResponse
{
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public int ExpiresIn { get; set; }
    
        public UserProfileDto User { get; set; } = new();
}

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int Level { get; set; } 
    public long Exp { get; set; } 
    public string Role { get; set; } = string.Empty; 
    public string Status { get; set; } = string.Empty; 
}
