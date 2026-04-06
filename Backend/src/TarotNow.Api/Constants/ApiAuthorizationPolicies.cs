using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TarotNow.Api.Constants;

/// <summary>
/// Authorization policy names used by API endpoints and hubs.
/// </summary>
public static class ApiAuthorizationPolicies
{
    public const string AuthenticatedUser = "authenticated_user";

    public static readonly Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser = policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    };
}
