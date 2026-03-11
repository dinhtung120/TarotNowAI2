using MediatR;
using System.Text.Json;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public class ProcessDepositWebhookCommand : IRequest<bool>
{
    // Nội dung raw JSON body để check signature
    public string RawPayload { get; set; } = string.Empty;

    // Chữ ký từ Header (vd: X-Sapo-SessionId, X-Signature, tùy cổng thanh toán)
    public string Signature { get; set; } = string.Empty;

    // Payload đã được parse
    public WebhookPayloadData PayloadData { get; set; } = new();
}

public class WebhookPayloadData
{
    public string OrderId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Status { get; set; } = string.Empty; // "SUCCESS" or "FAILED"
    
    // Thuận tiện lưu trữ tỷ giá hoặc thông tin mở rộng
    public string? FxSnapshot { get; set; } 
}
