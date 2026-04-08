

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
// API subscription và entitlement.
// Luồng chính: quản lý plan, subscribe gói và truy vấn quyền lợi hiện tại của user.
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller subscription.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query subscription.</param>
    public SubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tạo mới một subscription plan (admin).
    /// </summary>
    /// <param name="command">Command tạo plan.</param>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Id plan vừa được tạo.</returns>
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

    /// <summary>
    /// Đăng ký gói subscription cho người dùng hiện tại.
    /// Luồng xử lý: xác thực user id từ claim, dựng command subscribe, trả subscription id.
    /// </summary>
    /// <param name="request">Payload plan id và idempotency key.</param>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Id subscription mới tạo.</returns>
    [HttpPost("subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Subscribe(
        [FromBody] SubscribeRequest request,
        CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            // Chặn subscribe khi claim định danh không hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Idempotency key được truyền trực tiếp để tránh tạo trùng subscription khi retry.
        var command = new SubscribeCommand(userId, request.PlanId, request.IdempotencyKey);
        var subId = await _mediator.Send(command, cancellationToken);
        return Ok(new { SubscriptionId = subId });
    }

    /// <summary>
    /// Lấy danh sách subscription plan công khai.
    /// </summary>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Danh sách plan có thể đăng ký.</returns>
    [HttpGet("plans")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(System.Collections.Generic.List<TarotNow.Application.Features.Subscription.Queries.Dtos.SubscriptionPlanDto>))]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var plans = await _mediator.Send(new TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans.GetSubscriptionPlansQuery(), cancellationToken);
        return Ok(plans);
    }

    /// <summary>
    /// Lấy danh sách entitlement của người dùng hiện tại.
    /// </summary>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Danh sách quyền lợi còn lại của user.</returns>
    [HttpGet("me/entitlements")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(System.Collections.Generic.List<TarotNow.Application.Interfaces.EntitlementBalanceDto>))]
    public async Task<IActionResult> GetMyEntitlements(CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            // Chặn truy vấn entitlement khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        var entitlements = await _mediator.Send(new TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements.GetMyEntitlementsQuery(userId), cancellationToken);
        return Ok(entitlements);
    }
}

// Payload đăng ký subscription plan.
public record SubscribeRequest(Guid PlanId, string IdempotencyKey);
