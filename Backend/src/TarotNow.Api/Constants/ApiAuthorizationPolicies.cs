using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TarotNow.Api.Constants;

public static class ApiAuthorizationPolicies
{
    public const string AuthenticatedUser = "authenticated_user";
    public const string AdminOnly = "AdminOnly";

    public static readonly Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser = policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    };

    public static readonly Action<AuthorizationPolicyBuilder> RequireAdminOnly = policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireRole("admin");
    };
}
