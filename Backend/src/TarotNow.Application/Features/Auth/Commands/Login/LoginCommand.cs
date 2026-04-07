

using System.Text.Json.Serialization;
using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<(AuthResponse Response, string RefreshToken)>
{
        public string EmailOrUsername { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
    public string ClientIpAddress { get; set; } = string.Empty;
}
