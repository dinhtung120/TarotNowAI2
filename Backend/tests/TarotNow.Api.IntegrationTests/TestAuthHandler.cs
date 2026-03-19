/*
 * FILE: TestAuthHandler.cs
 * MỤC ĐÍCH: Mock Authentication Handler cho Integration Tests.
 *   Thay thế JWT authentication thật → cho phép test đi qua [Authorize] mà không cần login.
 *
 *   TẠI SAO CẦN?
 *   → Integration tests cần gọi API có [Authorize] attribute
 *   → Nếu dùng JWT thật: phải seed DB + gọi login API + parse token → phức tạp, chậm
 *   → TestAuthHandler: set claims trực tiếp qua HTTP headers → nhanh, đơn giản
 *
 *   CÁCH DÙNG:
 *   → Header "Authorization: TestScheme" → trigger TestAuthHandler
 *   → Header "X-Test-UserId: {guid}" → set User ID tùy chỉnh (mặc định: 000...001)
 *   → Header "X-Test-Role: admin" → set role tùy chỉnh (mặc định: "User")
 *
 *   CLAIMS ĐƯỢC TẠO:
 *   → NameIdentifier: User ID (để controller lấy userId từ claims)
 *   → Name: "TestUser" (cố định)
 *   → Role: từ X-Test-Role header (cho policy-based authorization)
 */

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Mock auth handler: bypass JWT cho integration tests.
/// Set UserId + Role qua HTTP headers.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>Tên scheme — dùng khi đăng ký trong DI + set Authorization header.</summary>
    public const string AuthenticationScheme = "TestScheme";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

    /// <summary>
    /// Xử lý authentication: tạo ClaimsPrincipal từ HTTP headers.
    /// → X-Test-UserId: override UserId (mặc định "000...001")
    /// → X-Test-Role: override Role (mặc định "User")
    /// → Luôn trả Success → mọi request đều authenticated.
    /// </summary>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // UserId: lấy từ header hoặc dùng mặc định
        var testUserId = "00000000-0000-0000-0000-000000000001";
        if (Request.Headers.TryGetValue("X-Test-UserId", out var customId))
        {
            testUserId = customId.ToString();
        }

        // Role: lấy từ header hoặc mặc định "User"
        var testRole = "User";
        if (Request.Headers.TryGetValue("X-Test-Role", out var customRole))
        {
            testRole = customRole.ToString();
        }

        // Tạo claims → ClaimsPrincipal → AuthenticationTicket
        var claims = new[] 
        { 
            new Claim(ClaimTypes.NameIdentifier, testUserId), // Controller dùng để lấy userId
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, testRole)              // Policy-based authorization
        };
        
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        // Luôn trả Success → mọi request đều authenticated
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
