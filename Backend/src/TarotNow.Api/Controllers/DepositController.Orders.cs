using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    /// <summary>
    /// Tạo đơn nạp tiền cho người dùng hiện tại.
    /// Luồng xử lý: xác thực user, map request sang command, gửi command tạo order.
    /// </summary>
    /// <param name="request">Payload tạo đơn nạp tiền.</param>
    /// <returns>Thông tin đơn nạp sau khi tạo thành công.</returns>
    [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDepositOrderRequest request)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn tạo order khi không có user id hợp lệ để tránh đơn nạp mồ côi.
            return this.UnauthorizedProblem();
        }

        // Mapping tường minh để đảm bảo idempotency key được chuyển đúng sang tầng nghiệp vụ.
        var command = new CreateDepositOrderCommand
        {
            UserId = userId,
            AmountVnd = request.AmountVnd,
            IdempotencyKey = request.IdempotencyKey
        };

        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
