

using MediatR;
using System.Text.Json;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Command xử lý webhook xác nhận giao dịch nạp tiền từ cổng thanh toán.
public class ProcessDepositWebhookCommand : IRequest<bool>
{
    // Nội dung payload thô dùng để verify chữ ký.
    public string RawPayload { get; set; } = string.Empty;

    // Chữ ký webhook gửi kèm từ cổng thanh toán.
    public string Signature { get; set; } = string.Empty;

    // Payload đã parse phục vụ xử lý nghiệp vụ.
    public WebhookPayloadData PayloadData { get; set; } = new();
}

// DTO payload webhook của deposit.
public class WebhookPayloadData
{
    // Định danh order phía hệ thống.
    public string OrderId { get; set; } = string.Empty;

    // Mã giao dịch phía cổng thanh toán.
    public string TransactionId { get; set; } = string.Empty;

    // Số tiền giao dịch từ webhook.
    public long Amount { get; set; }

    // Trạng thái giao dịch: SUCCESS hoặc FAILED.
    public string Status { get; set; } = string.Empty;

    // Snapshot tỷ giá/metadata ngoại hối (nếu có).
    public string? FxSnapshot { get; set; }
}
