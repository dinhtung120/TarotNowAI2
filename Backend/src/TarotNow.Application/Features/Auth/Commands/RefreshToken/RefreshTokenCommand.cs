

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<(AuthResponse Response, string NewRefreshToken)>
{
        public string Token { get; set; } = string.Empty;

        public string ClientIpAddress { get; set; } = string.Empty;
}
