


using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System;                   
using System.Threading.Tasks;   


using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;


using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Deposits)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public class DepositController : ControllerBase
{
    
    private readonly IMediator Mediator;
    private readonly ILogger<DepositController> _logger;

    public DepositController(IMediator mediator, ILogger<DepositController> logger)
    {
        Mediator = mediator;
        _logger = logger;
    }

        [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDepositOrderRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        
        var command = new CreateDepositOrderCommand
        {
            UserId = userId,            
            AmountVnd = request.AmountVnd 
        };

        
        var response = await Mediator.Send(command);
        return Ok(response); 
    }

        [HttpPost("webhook/vnpay")] 
    [AllowAnonymous] 
    public async Task<IActionResult> Webhook()
    {
        
        
        

        
        using var reader = new System.IO.StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync();
        
        
        var signature = Request.Headers["X-Webhook-Signature"].ToString() ?? string.Empty;

        
        
        

        
        WebhookPayloadData? payloadData;
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            payloadData = System.Text.Json.JsonSerializer.Deserialize<WebhookPayloadData>(rawPayload, options);
        }
        catch (System.Text.Json.JsonException ex)
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

        var result = await Mediator.Send(command);

        
        
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process deposit webhook",
                detail: "Không thể xử lý webhook nạp tiền.");
    }
}
