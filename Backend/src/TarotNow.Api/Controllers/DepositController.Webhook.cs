using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    /// <summary>
    /// Nhận webhook thanh toán từ cổng nạp tiền.
    /// Luồng xử lý: đọc payload thô, deserialize an toàn, gửi command xác minh/chốt đơn webhook.
    /// </summary>
    /// <returns>Kết quả xử lý webhook thành công hoặc lỗi payload/chữ ký không hợp lệ.</returns>
    [HttpPost("webhook/vnpay")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook()
    {
        // Luôn giữ raw payload để phục vụ xác minh chữ ký và truy vết sự cố sau này.
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
            // Payload sai định dạng JSON được từ chối sớm để tránh đi sâu vào luồng xử lý tài chính.
            _logger.LogWarning(ex, "Invalid webhook JSON payload.");
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid webhook payload",
                detail: "Invalid JSON payload");
        }

        if (payloadData == null)
        {
            // Edge case deserialize ra null: coi như payload không hợp lệ để bảo vệ nghiệp vụ nạp tiền.
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
        // Rẽ nhánh phản hồi để phân biệt webhook hợp lệ và webhook bị từ chối xử lý nghiệp vụ.
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process deposit webhook",
                detail: "Không thể xử lý webhook nạp tiền.");
    }
}
