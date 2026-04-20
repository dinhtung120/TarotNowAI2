using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    /// <summary>
    /// Nhận webhook PayOS và chuyển sang command xử lý idempotent.
    /// </summary>
    [HttpPost("webhook/payos")]
    [AllowAnonymous]
    public async Task<IActionResult> PayOsWebhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync(cancellationToken);

        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = rawPayload
        };

        var handled = await _mediator.Send(command, cancellationToken);
        return handled
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process PayOS webhook",
                detail: "Webhook is invalid or cannot be processed.");
    }
}
