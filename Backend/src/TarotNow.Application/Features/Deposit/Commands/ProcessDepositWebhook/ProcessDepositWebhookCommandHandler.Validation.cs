using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public partial class ProcessDepositWebhookCommandHandler
{
    private static void ValidateWebhookPayload(ProcessDepositWebhookCommand request)
    {
        if (request.PayloadData.Amount <= 0)
        {
            throw new BadRequestException("Invalid webhook amount.");
        }

        if (string.IsNullOrWhiteSpace(request.PayloadData.TransactionId))
        {
            throw new BadRequestException("Missing webhook transaction id.");
        }
    }

    private static bool ResolveWebhookStatus(string? status)
    {
        var webhookStatus = status?.Trim();
        if (string.Equals(webhookStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(webhookStatus, "FAILED", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        throw new BadRequestException($"Unsupported webhook status: {status}");
    }

    private void VerifyWebhookSignature(ProcessDepositWebhookCommand request)
    {
        if (!_paymentGatewayService.VerifyWebhookSignature(request.RawPayload, request.Signature))
        {
            throw new UnauthorizedAccessException("Invalid webhook signature.");
        }
    }

    private static Guid ParseOrderId(string orderId)
    {
        if (Guid.TryParse(orderId, out var parsed))
        {
            return parsed;
        }

        throw new BadRequestException("Invalid OrderId format in webhook.");
    }

    private static void ValidateWebhookAmount(ProcessDepositWebhookCommand request, DepositOrder order)
    {
        if (request.PayloadData.Amount != order.AmountVnd)
        {
            throw new BadRequestException("Webhook amount does not match order amount.");
        }
    }

    private static bool TryHandleAlreadyProcessedOrder(
        DepositOrder order,
        string transactionId,
        ref bool handled)
    {
        var alreadyProcessed = order.Status == "Success" || order.Status == "Failed";
        if (!alreadyProcessed)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(order.TransactionId)
            && !string.Equals(order.TransactionId, transactionId, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Processed order transaction id mismatch.");
        }

        handled = true;
        return true;
    }
}
