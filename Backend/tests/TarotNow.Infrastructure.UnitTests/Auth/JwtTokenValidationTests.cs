using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TarotNow.Infrastructure.UnitTests.Auth;

public sealed class JwtTokenValidationTests
{
    [Fact]
    public async Task ResolveTokenValidationAsync_ShouldFail_WhenSessionClaimMissing()
    {
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
                JwtBearerDefaults.AuthenticationScheme));
        var context = CreateTokenValidatedContext(principal);

        await InvokeResolveTokenValidationAsync(context);

        Assert.NotNull(context.Result?.Failure);
        Assert.Contains(
            "session binding",
            context.Result!.Failure!.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ResolveTokenValidationAsync_ShouldFail_WhenSessionClaimIsMalformed()
    {
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim("sid", "not-a-guid")
                ],
                JwtBearerDefaults.AuthenticationScheme));
        var context = CreateTokenValidatedContext(principal);

        await InvokeResolveTokenValidationAsync(context);

        Assert.NotNull(context.Result?.Failure);
        Assert.Contains(
            "session binding",
            context.Result!.Failure!.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    private static TokenValidatedContext CreateTokenValidatedContext(ClaimsPrincipal principal)
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var scheme = new AuthenticationScheme(
            JwtBearerDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme,
            typeof(JwtBearerHandler));
        var options = new JwtBearerOptions();
        return new TokenValidatedContext(httpContext, scheme, options)
        {
            Principal = principal
        };
    }

    private static async Task InvokeResolveTokenValidationAsync(TokenValidatedContext context)
    {
        var method = typeof(TarotNow.Infrastructure.DependencyInjection).GetMethod(
            "ResolveTokenValidationAsync",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);

        var invocation = method!.Invoke(null, [context]);
        Assert.NotNull(invocation);
        await (Task)invocation!;
    }
}
