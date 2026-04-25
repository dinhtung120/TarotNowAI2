using System.Security.Claims;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Inventory.Commands;
using TarotNow.Application.Features.Inventory.Queries;

namespace TarotNow.Api.Controllers;

/// <summary>
/// API quản lý kho đồ tarot của người dùng.
/// </summary>
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Inventory)]
[Authorize]
[EnableRateLimiting("auth-session")]
public sealed class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo inventory controller.
    /// </summary>
    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy toàn bộ kho đồ của user hiện tại.
    /// </summary>
    [HttpGet(InventoryRoutes.GetMyInventory)]
    public async Task<IActionResult> GetMyInventory(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetUserInventoryQuery(userId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Sử dụng một item trong kho đồ.
    /// </summary>
    [HttpPost(InventoryRoutes.UseItem)]
    public async Task<IActionResult> UseItem([FromBody] UseInventoryItemRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
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

        var command = new UseInventoryItemCommand
        {
            UserId = userId,
            ItemCode = request.ItemCode,
            Quantity = Math.Clamp(request.Quantity, 1, 10),
            TargetCardId = request.TargetCardId,
            IdempotencyKey = idempotencyKey,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claim, out userId);
    }

    private string ResolveIdempotencyKey(string? bodyKey)
    {
        return Request.GetIdempotencyKeyOrEmpty(bodyKey);
    }
}

/// <summary>
/// Payload sử dụng item inventory.
/// </summary>
public sealed class UseInventoryItemRequest
{
    /// <summary>
    /// Mã item cần sử dụng.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("itemCode")]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// Card mục tiêu nếu item yêu cầu.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("targetCardId")]
    public int? TargetCardId { get; set; }

    /// <summary>
    /// Số lượng muốn sử dụng (tối đa 10).
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Idempotency key (fallback khi không gửi qua header).
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("idempotencyKey")]
    public string? IdempotencyKey { get; set; }
}
