

using MediatR;
using System.Text.Json;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public class ProcessDepositWebhookCommand : IRequest<bool>
{
        public string RawPayload { get; set; } = string.Empty;

        public string Signature { get; set; } = string.Empty;

        public WebhookPayloadData PayloadData { get; set; } = new();
}

public class WebhookPayloadData
{
    public string OrderId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public long Amount { get; set; }
    
        public string Status { get; set; } = string.Empty; 
    
    
    public string? FxSnapshot { get; set; } 
}
