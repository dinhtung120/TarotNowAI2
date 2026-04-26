using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Commands.ReconcileMyDepositOrder;
using TarotNow.Application.Features.Deposit.Queries.GetMyDepositOrder;
using TarotNow.Application.Features.Deposit.Queries.ListMyDepositOrders;
using TarotNow.Application.Features.Deposit.Queries.ListDepositPackages;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    private static readonly TimeSpan CreateOrderProvisioningWaitTimeout = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan CreateOrderProvisioningPollInterval = TimeSpan.FromMilliseconds(150);
    private const string ReadyPaymentLinkStatus = "ready";

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
    [EnableRateLimiting("payment-create-order")]
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
        if (!string.Equals(response.PaymentLinkStatus, ReadyPaymentLinkStatus, StringComparison.OrdinalIgnoreCase))
        {
            response = await WaitForProvisionedPaymentLinkAsync(userId, response, cancellationToken);
        }

        return Ok(response);
    }

    /// <summary>
    /// Lấy lịch sử nạp của user hiện tại.
    /// </summary>
    [HttpGet("orders")]
    [Authorize]
    public async Task<IActionResult> ListMyOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var query = new ListMyDepositOrdersQuery
        {
            UserId = userId,
            Page = page,
            PageSize = pageSize,
            Status = status
        };

        var response = await _mediator.Send(query, cancellationToken);
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

    /// <summary>
    /// Reconcile trạng thái đơn nạp theo dữ liệu PayOS khi webhook bị trễ hoặc miss callback.
    /// </summary>
    [HttpPost("orders/{orderId:guid}/reconcile")]
    [Authorize]
    public async Task<IActionResult> ReconcileMyOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var command = new ReconcileMyDepositOrderCommand
        {
            UserId = userId,
            OrderId = orderId
        };

        var handled = await _mediator.Send(command, cancellationToken);
        return Ok(new { handled });
    }

    private string ResolveIdempotencyKey(CreateDepositOrderRequest request)
    {
        return Request.GetIdempotencyKeyOrEmpty(request.IdempotencyKey);
    }

    private async Task<CreateDepositOrderResponse> WaitForProvisionedPaymentLinkAsync(
        Guid userId,
        CreateDepositOrderResponse currentResponse,
        CancellationToken cancellationToken)
    {
        var deadline = DateTime.UtcNow + CreateOrderProvisioningWaitTimeout;
        while (DateTime.UtcNow < deadline)
        {
            await Task.Delay(CreateOrderProvisioningPollInterval, cancellationToken);

            var snapshot = await _mediator.Send(
                new GetMyDepositOrderQuery
                {
                    UserId = userId,
                    OrderId = currentResponse.OrderId
                },
                cancellationToken);
            if (!string.Equals(snapshot.PaymentLinkStatus, ReadyPaymentLinkStatus, StringComparison.OrdinalIgnoreCase))
            {
                currentResponse = MapToCreateResponse(snapshot);
                continue;
            }

            return MapToCreateResponse(snapshot);
        }

        return currentResponse;
    }

    private static CreateDepositOrderResponse MapToCreateResponse(MyDepositOrderDto snapshot)
    {
        return new CreateDepositOrderResponse
        {
            OrderId = snapshot.OrderId,
            Status = snapshot.Status,
            AmountVnd = snapshot.AmountVnd,
            BaseDiamondAmount = snapshot.BaseDiamondAmount,
            BonusGoldAmount = snapshot.BonusGoldAmount,
            TotalDiamondAmount = snapshot.TotalDiamondAmount,
            PayOsOrderCode = snapshot.PayOsOrderCode,
            PaymentLinkStatus = snapshot.PaymentLinkStatus,
            CheckoutUrl = snapshot.CheckoutUrl,
            QrCode = snapshot.QrCode,
            PaymentLinkId = snapshot.PaymentLinkId,
            ExpiresAtUtc = snapshot.ExpiresAtUtc,
            PaymentLinkFailureReason = snapshot.PaymentLinkFailureReason
        };
    }
}
