

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Command làm mới access token bằng refresh token hiện có.
public class RefreshTokenCommand : IRequest<(AuthResponse Response, string NewRefreshToken)>
{
    // Refresh token thô do client gửi lên.
    public string Token { get; set; } = string.Empty;

    // IP client hiện tại để gắn vào refresh token mới khi rotate.
    public string ClientIpAddress { get; set; } = string.Empty;
}
