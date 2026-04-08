

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TarotNow.Api.IntegrationTests;

// Auth handler giả lập cho integration tests, đọc user/role từ request headers.
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Tên scheme cố định để test host cấu hình authentication nhất quán.
    public const string AuthenticationScheme = "TestScheme";

    /// <summary>
    /// Khởi tạo handler xác thực giả lập cho môi trường test.
    /// Luồng kế thừa AuthenticationHandler chuẩn để tích hợp trực tiếp vào pipeline ASP.NET Core.
    /// </summary>
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <summary>
    /// Xử lý xác thực test: dựng ClaimsPrincipal từ header tùy biến.
    /// Luồng cho phép test điều khiển userId/role để kiểm tra authorization theo từng kịch bản.
    /// </summary>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Mặc định user test cố định, có thể ghi đè bằng header.
        var testUserId = "00000000-0000-0000-0000-000000000001";
        if (Request.Headers.TryGetValue("X-Test-UserId", out var customId))
        {
            testUserId = customId.ToString();
        }

        // Mặc định role User, có thể ghi đè bằng header để test RBAC.
        var testRole = "User";
        if (Request.Headers.TryGetValue("X-Test-Role", out var customRole))
        {
            testRole = customRole.ToString();
        }

        // Dựng claims tối thiểu cho identity: id, tên hiển thị và role.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, testUserId),
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, testRole)
        };

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        // Luôn trả thành công để tập trung test luồng business/authorization phía sau.
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
