using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public partial class ProcessDepositWebhookCommandHandler
{
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
                return;
            }

            await ApplyWebhookTransitionAsync(order, request, isSuccessStatus, transactionCt);
            await _depositOrderRepository.UpdateAsync(order, transactionCt);
            handled = true;
        }, cancellationToken);

        return handled;
    }

    private async Task<DepositOrder> GetOrderForUpdateAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _depositOrderRepository.GetByIdForUpdateAsync(orderId, cancellationToken)
            ?? throw new NotFoundException($"Deposit order {orderId} not found.");
    }

    private async Task ApplyWebhookTransitionAsync(
        DepositOrder order,
        ProcessDepositWebhookCommand request,
        bool isSuccessStatus,
        CancellationToken cancellationToken)
    {
        if (isSuccessStatus)
        {
            await ApplySuccessAsync(order, request, cancellationToken);
            return;
        }

        order.MarkAsFailed(request.PayloadData.TransactionId);
    }

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

        
        await _walletPushService.PushBalanceChangedAsync(order.UserId, cancellationToken);
    }
}
