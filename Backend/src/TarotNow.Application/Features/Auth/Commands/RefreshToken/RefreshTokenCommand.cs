

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Command làm mới access token bằng refresh token hiện có.
public class RefreshTokenCommand : IRequest<RefreshTokenResult>
{
    // Refresh token thô do client gửi lên.
    public string Token { get; set; } = string.Empty;

    // IP client hiện tại để gắn vào refresh token mới khi rotate.
    public string ClientIpAddress { get; set; } = string.Empty;

    // Device id phía client gửi lên.
    public string DeviceId { get; set; } = string.Empty;

    // User-agent hash của request refresh.
    public string UserAgentHash { get; set; } = string.Empty;

    // Idempotency key để chống refresh đồng thời.
    public string IdempotencyKey { get; set; } = string.Empty;
}
