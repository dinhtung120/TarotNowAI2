using System.Security.Claims;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
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
        var result = await _mediator.Send(new GetUserInventoryQuery(GetCurrentUserId()), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Sử dụng một item trong kho đồ.
    /// </summary>
    [HttpPost(InventoryRoutes.UseItem)]
    public async Task<IActionResult> UseItem([FromBody] UseInventoryItemRequest request, CancellationToken cancellationToken)
    {
        var idempotencyKey = ResolveIdempotencyKey(request.IdempotencyKey);
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return Problem(statusCode: 400, detail: "Missing idempotency key.");
        }

        var command = new UseInventoryItemCommand
        {
            UserId = GetCurrentUserId(),
            ItemCode = request.ItemCode,
            TargetCardId = request.TargetCardId,
            IdempotencyKey = idempotencyKey,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!);
    }

    private string ResolveIdempotencyKey(string? bodyKey)
    {
        if (Request.Headers.TryGetValue(AuthHeaders.IdempotencyKey, out var headerValue)
            && string.IsNullOrWhiteSpace(headerValue.ToString()) == false)
        {
            return headerValue.ToString();
        }

        return bodyKey ?? string.Empty;
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
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// Card mục tiêu nếu item yêu cầu.
    /// </summary>
    public int? TargetCardId { get; set; }

    /// <summary>
    /// Idempotency key (fallback khi không gửi qua header).
    /// </summary>
    public string? IdempotencyKey { get; set; }
}
