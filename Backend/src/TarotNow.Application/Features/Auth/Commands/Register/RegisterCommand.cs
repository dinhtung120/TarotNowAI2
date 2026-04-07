

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
        public string DisplayName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
        public bool HasConsented { get; set; }
}
