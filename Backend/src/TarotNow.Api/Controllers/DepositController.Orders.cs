using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Queries.GetMyDepositOrder;
using TarotNow.Application.Features.Deposit.Queries.ListDepositPackages;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    private const string IdempotencyHeaderName = "Idempotency-Key";

    /// <summary>
    /// Lấy danh sách gói nạp preset đang active.
    /// </summary>
    [HttpGet("packages")]
    [AllowAnonymous]
    public async Task<IActionResult> ListPackages(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListDepositPackagesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Tạo đơn nạp tiền PayOS cho người dùng hiện tại.
    /// </summary>
    [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateDepositOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var idempotencyKey = ResolveIdempotencyKey(request);
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid idempotency key",
                detail: "Idempotency key is required.");
        }

        var command = new CreateDepositOrderCommand
        {
            UserId = userId,
            PackageCode = request.PackageCode,
            IdempotencyKey = idempotencyKey
        };

        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Lấy trạng thái đơn nạp của chính người dùng hiện tại.
    /// </summary>
    [HttpGet("orders/{orderId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMyOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var query = new GetMyDepositOrderQuery
        {
            UserId = userId,
            OrderId = orderId
        };

        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    private string ResolveIdempotencyKey(CreateDepositOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey) == false)
        {
            return request.IdempotencyKey.Trim();
        }

        var headerValue = Request.Headers[IdempotencyHeaderName].ToString();
        return string.IsNullOrWhiteSpace(headerValue)
            ? string.Empty
            : headerValue.Trim();
    }
}
