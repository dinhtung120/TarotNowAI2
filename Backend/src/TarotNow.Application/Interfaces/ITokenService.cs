

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface ITokenService
{
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
}
