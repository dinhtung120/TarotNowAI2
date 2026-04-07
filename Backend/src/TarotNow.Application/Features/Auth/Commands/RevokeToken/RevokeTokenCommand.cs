

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<bool>
{
        public string Token { get; set; } = string.Empty;

        public bool RevokeAll { get; set; } = false;
    
        public Guid? UserId { get; set; }
}
