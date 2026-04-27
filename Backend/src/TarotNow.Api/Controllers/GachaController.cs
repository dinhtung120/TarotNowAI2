using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Gacha.Commands.PullGacha;
using TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;
using TarotNow.Application.Features.Gacha.Queries.GetGachaPoolOdds;
using TarotNow.Application.Features.Gacha.Queries.GetGachaPools;

namespace TarotNow.Api.Controllers;

/// <summary>
/// API gacha theo mô hình pool/pull mới.
/// </summary>
[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Gacha)]
[EnableRateLimiting("auth-session")]
public sealed class GachaController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo gacha controller.
    /// </summary>
    public GachaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách pool gacha active.
    /// </summary>
    [HttpGet(GachaRoutes.Pools)]
    public async Task<IActionResult> GetPools(CancellationToken cancellationToken)
    {
        var userId = User.GetUserIdOrNull();
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetGachaPoolsQuery { UserId = userId }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lấy odds của pool theo mã pool.
    /// </summary>
    [HttpGet(GachaRoutes.PoolOdds)]
    public async Task<IActionResult> GetPoolOdds([FromRoute] string poolCode, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetGachaPoolOddsQuery(poolCode), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lấy lịch sử pull của người dùng hiện tại.
    /// </summary>
    [HttpGet(GachaRoutes.History)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserIdOrNull();
        if (!userId.HasValue)
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetGachaHistoryQuery(userId.Value, page, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Thực hiện pull gacha.
    /// </summary>
    [HttpPost(GachaRoutes.Pull)]
    public async Task<IActionResult> Pull(
        [FromBody] PullGachaRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserIdOrNull();
        if (!userId.HasValue)
        {
            return this.UnauthorizedProblem();
        }

        var idempotencyKey = ResolveIdempotencyKey(request.IdempotencyKey);
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad request",
                detail: "Missing idempotency key.");
        }

        var command = new PullGachaCommand
        {
            UserId = userId.Value,
            PoolCode = request.PoolCode,
            Count = request.Count,
            IdempotencyKey = idempotencyKey,
        };

        var result = await _mediator.SendWithRequestCancellation(HttpContext, command, cancellationToken);
        if (string.Equals(result.OperationStatus, PullGachaResult.OperationStatusProcessing, StringComparison.Ordinal))
        {
            return Accepted(result);
        }

        return Ok(result);
    }

    private string ResolveIdempotencyKey(string? bodyValue)
    {
        return Request.GetIdempotencyKeyOrEmpty(bodyValue);
    }
}

/// <summary>
/// Payload pull gacha từ client.
/// </summary>
public sealed class PullGachaRequest
{
    /// <summary>
    /// Mã pool cần pull.
    /// </summary>
    public string PoolCode { get; set; } = string.Empty;

    /// <summary>
    /// Số lượt pull trong request.
    /// </summary>
    public int Count { get; set; } = 1;

    /// <summary>
    /// Idempotency key fallback nếu không gửi qua header.
    /// </summary>
    public string? IdempotencyKey { get; set; }
}
