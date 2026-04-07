using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    [HttpPost("webhook/vnpay")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook()
    {
        using var reader = new StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync();
        var signature = Request.Headers["X-Webhook-Signature"].ToString() ?? string.Empty;

        WebhookPayloadData? payloadData;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            payloadData = JsonSerializer.Deserialize<WebhookPayloadData>(rawPayload, options);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid webhook JSON payload.");
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid webhook payload",
                detail: "Invalid JSON payload");
        }

        if (payloadData == null)
        {
            _logger.LogWarning("Webhook payload is null after deserialization.");
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid webhook payload",
                detail: "Invalid JSON payload");
        }

        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = rawPayload,
            Signature = signature,
            PayloadData = payloadData
        };

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process deposit webhook",
                detail: "Không thể xử lý webhook nạp tiền.");
    }
}
