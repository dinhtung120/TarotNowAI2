

using System.Text.Json.Serialization;
using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Login;

// Command đăng nhập bằng email hoặc username.
public class LoginCommand : IRequest<(AuthResponse Response, string RefreshToken)>
{
    // Định danh đăng nhập đầu vào (email hoặc username).
    public string EmailOrUsername { get; set; } = string.Empty;

    // Mật khẩu thô để xác thực.
    public string Password { get; set; } = string.Empty;

    // IP client lấy từ tầng API để lưu lịch sử refresh token.
    [JsonIgnore]
    public string ClientIpAddress { get; set; } = string.Empty;
}
