

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TarotNow.Api.IntegrationTests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
        public const string AuthenticationScheme = "TestScheme";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        
        var testUserId = "00000000-0000-0000-0000-000000000001";
        if (Request.Headers.TryGetValue("X-Test-UserId", out var customId))
        {
            testUserId = customId.ToString();
        }

        
        var testRole = "User";
        if (Request.Headers.TryGetValue("X-Test-Role", out var customRole))
        {
            testRole = customRole.ToString();
        }

        
        var claims = new[] 
        { 
            new Claim(ClaimTypes.NameIdentifier, testUserId), 
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, testRole)              
        };
        
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
