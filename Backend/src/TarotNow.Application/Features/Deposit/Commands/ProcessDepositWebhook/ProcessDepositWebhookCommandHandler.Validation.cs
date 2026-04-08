using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public partial class ProcessDepositWebhookCommandHandler
{
    /// <summary>
    /// Validate các trường bắt buộc trong payload webhook.
    /// Luồng xử lý: kiểm tra amount dương và transaction id không rỗng trước khi vào workflow.
    /// </summary>
    private static void ValidateWebhookPayload(ProcessDepositWebhookCommand request)
    {
        if (request.PayloadData.Amount <= 0)
        {
            // Amount không hợp lệ thì dừng ngay để tránh cập nhật sai order.
            throw new BadRequestException("Invalid webhook amount.");
        }

        if (string.IsNullOrWhiteSpace(request.PayloadData.TransactionId))
        {
            // TransactionId bắt buộc để đối soát và idempotency cho order.
            throw new BadRequestException("Missing webhook transaction id.");
        }
    }

    /// <summary>
    /// Chuẩn hóa trạng thái webhook thành cờ thành công/thất bại.
    /// Luồng xử lý: chấp nhận SUCCESS hoặc FAILED, còn lại ném lỗi unsupported.
    /// </summary>
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

    /// <summary>
    /// Xác thực chữ ký webhook từ cổng thanh toán.
    /// Luồng xử lý: gọi service verify và ném lỗi unauthorized nếu chữ ký không khớp.
    /// </summary>
    private void VerifyWebhookSignature(ProcessDepositWebhookCommand request)
    {
        if (!_paymentGatewayService.VerifyWebhookSignature(request.RawPayload, request.Signature))
        {
            throw new UnauthorizedAccessException("Invalid webhook signature.");
        }
    }

    /// <summary>
    /// Parse OrderId từ payload webhook sang Guid.
    /// Luồng xử lý: parse Guid hợp lệ, nếu không hợp lệ thì ném BadRequest.
    /// </summary>
    private static Guid ParseOrderId(string orderId)
    {
        if (Guid.TryParse(orderId, out var parsed))
        {
            return parsed;
        }

        throw new BadRequestException("Invalid OrderId format in webhook.");
    }

    /// <summary>
    /// Kiểm tra số tiền webhook có khớp số tiền order hay không.
    /// Luồng xử lý: so sánh Amount trong payload với AmountVnd của order trước khi áp transition.
    /// </summary>
    private static void ValidateWebhookAmount(ProcessDepositWebhookCommand request, DepositOrder order)
    {
        if (request.PayloadData.Amount != order.AmountVnd)
        {
            throw new BadRequestException("Webhook amount does not match order amount.");
        }
    }

    /// <summary>
    /// Xử lý nhánh order đã được xử lý trước đó (idempotent webhook).
    /// Luồng xử lý: nếu order đã Success/Failed thì kiểm tra transaction id nhất quán, đánh dấu handled=true và bỏ qua transition mới.
    /// </summary>
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
            // Webhook trùng nhưng transaction id khác nhau là dấu hiệu bất thường.
            throw new BadRequestException("Processed order transaction id mismatch.");
        }

        handled = true;
        return true;
    }
}
