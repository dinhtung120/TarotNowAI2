using System.Security.Claims;

namespace TarotNow.Api.Extensions;

// Extension helper đọc user id từ ClaimsPrincipal theo nhiều chuẩn claim.
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Lấy user id từ claims nếu có thể parse thành Guid.
    /// Luồng xử lý: thử NameIdentifier, fallback sub chuẩn JWT, parse Guid an toàn.
    /// </summary>
    /// <param name="user">Claims principal cần đọc định danh.</param>
    /// <returns>User id dạng Guid hoặc <c>null</c> nếu không hợp lệ.</returns>
    public static Guid? GetUserIdOrNull(this ClaimsPrincipal? user)
    {
        // Ưu tiên claim chuẩn nội bộ rồi fallback claim sub để tương thích nhiều issuer token.
        var userIdValue = user?.FindFirstValue(ClaimTypes.NameIdentifier) 
                       ?? user?.FindFirstValue("sub")
                       ?? user?.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }

    /// <summary>
    /// Thử lấy user id từ claims principal theo dạng Try-pattern.
    /// Luồng xử lý: tái sử dụng <see cref="GetUserIdOrNull(System.Security.Claims.ClaimsPrincipal?)"/>, trả false khi không parse được.
    /// </summary>
    /// <param name="user">Claims principal cần đọc định danh.</param>
    /// <param name="userId">User id đầu ra nếu lấy thành công.</param>
    /// <returns><c>true</c> nếu lấy được user id; ngược lại <c>false</c>.</returns>
    public static bool TryGetUserId(this ClaimsPrincipal? user, out Guid userId)
    {
        var parsedUserId = user.GetUserIdOrNull();
        if (parsedUserId == null)
        {
            // Gán Guid.Empty để caller có giá trị mặc định nhất quán ở nhánh thất bại.
            userId = Guid.Empty;
            return false;
        }

        userId = parsedUserId.Value;
        return true;
    }
}
