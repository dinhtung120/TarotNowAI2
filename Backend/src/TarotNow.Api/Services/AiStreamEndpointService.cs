using System.Security.Claims;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Logging;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Services;

public sealed partial class AiStreamEndpointService : IAiStreamEndpointService
{
    private const string BadRequestType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    private const string ForbiddenType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
    private const string ServiceUnavailableType = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4";
    private const string UnauthorizedType = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";

    private readonly IFeatureManagerSnapshot _featureManager;
    private readonly IAiStreamTicketService _aiStreamTicketService;
    private readonly ILogger<AiStreamEndpointService> _logger;

    public AiStreamEndpointService(
        IFeatureManagerSnapshot featureManager,
        IAiStreamTicketService aiStreamTicketService,
        ILogger<AiStreamEndpointService> logger)
    {
        _featureManager = featureManager;
        _aiStreamTicketService = aiStreamTicketService;
        _logger = logger;
    }

    private async Task<AiStreamAccessResult> EnsureStreamingAccessAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AiStreamingEnabled))
        {
            return new AiStreamAccessResult(Guid.Empty, CreateServiceUnavailable("AI streaming is temporarily disabled by feature flag."));
        }

        return user.TryGetUserId(out var userId)
            ? new AiStreamAccessResult(userId, null)
            : new AiStreamAccessResult(Guid.Empty, CreateUnauthorized("Authentication is required or token is invalid."));
    }

    private static AiStreamEndpointProblem CreateBadRequest(string detail) =>
        new(StatusCodes.Status400BadRequest, "Bad Request", detail, BadRequestType);

    private static AiStreamEndpointProblem CreateForbidden(string detail) =>
        new(StatusCodes.Status403Forbidden, "Forbidden", detail, ForbiddenType);

    private static AiStreamEndpointProblem CreateServiceUnavailable(string detail) =>
        new(StatusCodes.Status503ServiceUnavailable, "Service Unavailable", detail, ServiceUnavailableType);

    private static AiStreamEndpointProblem CreateUnauthorized(string detail) =>
        new(StatusCodes.Status401Unauthorized, "Unauthorized", detail, UnauthorizedType);

    private static string NormalizeLanguage(string? language) =>
        string.IsNullOrWhiteSpace(language) ? "en" : language.Trim();

    private readonly record struct AiStreamAccessResult(
        Guid UserId,
        AiStreamEndpointProblem? Problem);
}
