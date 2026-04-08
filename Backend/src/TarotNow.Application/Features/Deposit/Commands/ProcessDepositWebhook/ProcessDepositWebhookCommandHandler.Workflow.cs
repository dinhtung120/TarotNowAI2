using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public partial class ProcessDepositWebhookCommandHandler
{
    /// <summary>
    /// Thực thi workflow xử lý webhook trong transaction.
    /// Luồng xử lý: khóa order để cập nhật, validate amount, xử lý nhánh already-processed, áp transition success/failed và persist.
    /// </summary>
    private async Task<bool> ProcessWebhookAsync(
        ProcessDepositWebhookCommand request,
        Guid orderId,
        bool isSuccessStatus,
        CancellationToken cancellationToken)
    {
        var handled = false;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var order = await GetOrderForUpdateAsync(orderId, transactionCt);
            ValidateWebhookAmount(request, order);

            if (TryHandleAlreadyProcessedOrder(order, request.PayloadData.TransactionId, ref handled))
            {
                // Order đã xử lý trước đó thì giữ idempotent và dừng sớm.
                return;
            }

            await ApplyWebhookTransitionAsync(order, request, isSuccessStatus, transactionCt);
            await _depositOrderRepository.UpdateAsync(order, transactionCt);
            handled = true;
        }, cancellationToken);

        return handled;
    }

    /// <summary>
    /// Lấy order ở chế độ for-update để cập nhật an toàn trong transaction.
    /// Luồng xử lý: tải order theo id và ném NotFound nếu không tồn tại.
    /// </summary>
    private async Task<DepositOrder> GetOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _depositOrderRepository.GetByIdForUpdateAsync(orderId, cancellationToken)
            ?? throw new NotFoundException($"Deposit order {orderId} not found.");
    }

    /// <summary>
    /// Áp dụng chuyển trạng thái order theo kết quả webhook.
    /// Luồng xử lý: SUCCESS thì credit ví + mark success; FAILED thì mark failed.
    /// </summary>
    private async Task ApplyWebhookTransitionAsync(
        DepositOrder order,
        ProcessDepositWebhookCommand request,
        bool isSuccessStatus,
        CancellationToken cancellationToken)
    {
        if (isSuccessStatus)
        {
            // Nhánh thành công: cộng kim cương và cập nhật order success.
            await ApplySuccessAsync(order, request, cancellationToken);
            return;
        }

        // Nhánh thất bại: chỉ đánh dấu order failed với transaction id.
        order.MarkAsFailed(request.PayloadData.TransactionId);
    }

    /// <summary>
    /// Xử lý nhánh webhook SUCCESS.
    /// Luồng xử lý: credit diamond vào ví theo idempotency key order, mark order success, rồi push cập nhật số dư realtime.
    /// </summary>
    private async Task ApplySuccessAsync(
        DepositOrder order,
        ProcessDepositWebhookCommand request,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: order.UserId,
            currency: TarotNow.Domain.Enums.CurrencyType.Diamond,
            type: TarotNow.Domain.Enums.TransactionType.Deposit,
            amount: order.DiamondAmount,
            referenceSource: "PaymentGateway",
            referenceId: request.PayloadData.TransactionId,
            description: "Mua Diamond từ Payment Gateway",
            metadataJson: null,
            idempotencyKey: $"DEPOSIT_{order.Id}",
            cancellationToken: cancellationToken);

        order.MarkAsSuccess(request.PayloadData.TransactionId, request.PayloadData.FxSnapshot);

        // Đẩy sự kiện số dư để client thấy kim cương tăng ngay.
        await _walletPushService.PushBalanceChangedAsync(order.UserId, cancellationToken);
    }
}
