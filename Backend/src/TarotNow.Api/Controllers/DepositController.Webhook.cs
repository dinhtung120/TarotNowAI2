using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    /// <summary>
    /// Nhận webhook PayOS và chuyển sang command xử lý idempotent.
    /// </summary>
    [HttpPost("webhook/payos")]
    [AllowAnonymous]
    [DisableRateLimiting]
    public async Task<IActionResult> PayOsWebhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync(cancellationToken);

        _logger.LogInformation(
            "Received PayOS webhook. RemoteIp={RemoteIp}, PayloadLength={PayloadLength}",
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            rawPayload.Length);

        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = rawPayload
        };

        var handled = await _mediator.Send(command, cancellationToken);
        if (!handled)
        {
            _logger.LogWarning("PayOS webhook was not handled.");
        }

        return handled
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process PayOS webhook",
                detail: "Webhook is invalid or cannot be processed.");
    }
}
