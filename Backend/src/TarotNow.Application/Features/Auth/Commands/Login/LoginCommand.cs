using System.Text.Json.Serialization;
using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Input người dùng cấu hình cho Endpoint Login.
/// Yêu cầu cung cấp Email hoặc Username để xác định danh tính.
/// Xử lý trả về Tuple chứa AuthResponse và giá trị RefreshToken thô cần set Cookie.
/// </summary>
public class LoginCommand : IRequest<(AuthResponse Response, string RefreshToken)>
{
    public string EmailOrUsername { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// IP của thiết bị client. Thường lấy từ `HttpContext.Connection.RemoteIpAddress`.
    /// </summary>
    [JsonIgnore]
    public string ClientIpAddress { get; set; } = string.Empty;
}
