using System.Security.Claims;

namespace TarotNow.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserIdOrNull(this ClaimsPrincipal? user)
    {
        var userIdValue = user?.FindFirstValue(ClaimTypes.NameIdentifier) 
                       ?? user?.FindFirstValue("sub")
                       ?? user?.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }

    public static bool TryGetUserId(this ClaimsPrincipal? user, out Guid userId)
    {
        var parsedUserId = user.GetUserIdOrNull();
        if (parsedUserId == null)
        {
            userId = Guid.Empty;
            return false;
        }

        userId = parsedUserId.Value;
        return true;
    }
}
