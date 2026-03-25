using MediatR;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = UserRole.User;
}
