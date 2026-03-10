using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// DTO chứa thông tin input của người dùng khi gọi API Register.
/// Implement IRequest đại diện cho 1 CQRS Command trả về Guid (dành cho UserId mới được tạo).
/// </summary>
public class RegisterCommand : IRequest<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public bool HasConsented { get; set; }
}
