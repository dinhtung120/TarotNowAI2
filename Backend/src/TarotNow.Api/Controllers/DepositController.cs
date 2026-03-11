using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

[Route("api/v1/deposits")]
[ApiController]
public class DepositController : ControllerBase
{
    private readonly IMediator Mediator;

    public DepositController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDepositOrderRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var command = new CreateDepositOrderCommand
        {
            UserId = userId,
            AmountVnd = request.AmountVnd
        };

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("webhook/vnpay")] // HOẶC tên gateway cung cấp
    [AllowAnonymous] // Webhook call is server-to-server, secured by signature header validation
    public async Task<IActionResult> Webhook()
    {
        // 1. Lọc payload và signature
        using var reader = new System.IO.StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync();
        
        // Example: Lấy header chữ ký từ gateway. Tùy gateway sẽ đổi tên header này.
        var signature = Request.Headers["X-Webhook-Signature"].ToString() ?? string.Empty;

        // 2. Parse payload lấy thông tin giao dịch
        // Lưu ý: Ở môi trường sản xuất cần map JSON chính xác bằng JsonSerializer.Deserialize 
        // dựa theo format của nhà cung cấp.
        WebhookPayloadData payloadData;
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            payloadData = System.Text.Json.JsonSerializer.Deserialize<WebhookPayloadData>(rawPayload, options) 
                          ?? throw new Exception("Payload is null.");
        }
        catch
        {
            return BadRequest(new { msg = "Invalid JSON payload" });
        }

        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = rawPayload,
            Signature = signature,
            PayloadData = payloadData
        };

        try
        {
            var result = await Mediator.Send(command);
            return result ? Ok(new { success = true }) : BadRequest();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { msg = ex.Message });
        }
        catch (Exception ex)
        {
            // Log ex
            return StatusCode(500, new { msg = ex.Message, stack = ex.ToString() });
        }
    }
}

public class CreateDepositOrderRequest
{
    public long AmountVnd { get; set; }
}
