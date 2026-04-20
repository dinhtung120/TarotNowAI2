using MediatR;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Command xử lý webhook xác nhận giao dịch nạp tiền từ PayOS.
public class ProcessDepositWebhookCommand : IRequest<bool>
{
    // Nội dung payload thô dùng để verify webhook.
    public string RawPayload { get; set; } = string.Empty;
}
