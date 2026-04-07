

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;
using TarotNow.Application.Features.Subscription.Commands.Subscribe;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Subscriptions)]
[Authorize]
[EnableRateLimiting("auth-session")]
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpPost("plans")]
    [Authorize(Policy = ApiAuthorizationPolicies.AdminOnly)] 
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    public async Task<IActionResult> CreatePlan(
        [FromBody] CreateSubscriptionPlanCommand command,
        CancellationToken cancellationToken)
    {
        var planId = await _mediator.Send(command, cancellationToken);
        return Created($"/api/subscriptions/plans/{planId}", planId);
    }

        [HttpPost("subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Subscribe(
        [FromBody] SubscribeRequest request,
        CancellationToken cancellationToken)
    {
        
        
        
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return this.UnauthorizedProblem();

        var command = new SubscribeCommand(userId, request.PlanId, request.IdempotencyKey);
        var subId = await _mediator.Send(command, cancellationToken);
        
        return Ok(new { SubscriptionId = subId });
    }

        [HttpGet("plans")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(System.Collections.Generic.List<TarotNow.Application.Features.Subscription.Queries.Dtos.SubscriptionPlanDto>))]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var plans = await _mediator.Send(new TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans.GetSubscriptionPlansQuery(), cancellationToken);
        return Ok(plans);
    }

        [HttpGet("me/entitlements")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(System.Collections.Generic.List<TarotNow.Application.Interfaces.EntitlementBalanceDto>))]
    public async Task<IActionResult> GetMyEntitlements(CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return this.UnauthorizedProblem();

        var entitlements = await _mediator.Send(new TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements.GetMyEntitlementsQuery(userId), cancellationToken);
        return Ok(entitlements);
    }
}

public record SubscribeRequest(Guid PlanId, string IdempotencyKey);
