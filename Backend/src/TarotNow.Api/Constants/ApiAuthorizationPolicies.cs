using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TarotNow.Api.Constants;

public static class ApiAuthorizationPolicies
{
    public const string AuthenticatedUser = "authenticated_user";

    public static readonly Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser = policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    };
}
