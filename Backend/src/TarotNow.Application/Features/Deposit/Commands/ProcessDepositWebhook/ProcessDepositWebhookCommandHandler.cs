using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public class ProcessDepositWebhookCommandHandler : IRequestHandler<ProcessDepositWebhookCommand, bool>
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public ProcessDepositWebhookCommandHandler(
        IPaymentGatewayService paymentGatewayService,
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository,
        ITransactionCoordinator transactionCoordinator)
    {
        _paymentGatewayService = paymentGatewayService;
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ProcessDepositWebhookCommand request, CancellationToken cancellationToken)
    {
        if (request.PayloadData.Amount <= 0)
            throw new BadRequestException("Invalid webhook amount.");

        if (string.IsNullOrWhiteSpace(request.PayloadData.TransactionId))
            throw new BadRequestException("Missing webhook transaction id.");

        var webhookStatus = request.PayloadData.Status?.Trim();
        var isSuccessStatus = string.Equals(webhookStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase);
        var isFailedStatus = string.Equals(webhookStatus, "FAILED", StringComparison.OrdinalIgnoreCase);
        if (!isSuccessStatus && !isFailedStatus)
            throw new BadRequestException($"Unsupported webhook status: {request.PayloadData.Status}");

        // 1. Xác thực chữ ký
        if (!_paymentGatewayService.VerifyWebhookSignature(request.RawPayload, request.Signature))
        {
            throw new UnauthorizedAccessException("Invalid webhook signature.");
        }

        // 2. Tìm kiếm OrderId (Parse từ OrderId string trong webhook info)
        if (!Guid.TryParse(request.PayloadData.OrderId, out var orderId))
        {
            throw new BadRequestException("Invalid OrderId format in webhook.");
        }

        var handled = false;
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var order = await _depositOrderRepository.GetByIdForUpdateAsync(orderId, transactionCt)
                ?? throw new NotFoundException($"Deposit order {orderId} not found.");

            if (request.PayloadData.Amount != order.AmountVnd)
                throw new BadRequestException("Webhook amount does not match order amount.");

            // 3. Idempotency Check (Nếu đã xử lý, bỏ qua và trả về OK để cổng thanh toán không gọi lại)
            if (order.Status == "Success" || order.Status == "Failed")
            {
                if (!string.IsNullOrWhiteSpace(order.TransactionId) &&
                    !string.Equals(order.TransactionId, request.PayloadData.TransactionId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new BadRequestException("Processed order transaction id mismatch.");
                }

                handled = true;
                return;
            }

            // 4. Xử lý ghi nhận số dư (Diamond)
            if (isSuccessStatus)
            {
                // Dùng key ổn định theo order để tránh double-credit nếu webhook retry với transactionId khác.
                var idempotencyKey = $"DEPOSIT_{order.Id}";

                await _walletRepository.CreditAsync(
                    userId: order.UserId,
                    currency: TarotNow.Domain.Enums.CurrencyType.Diamond,
                    type: TarotNow.Domain.Enums.TransactionType.Deposit,
                    amount: order.DiamondAmount,
                    referenceSource: "PaymentGateway",
                    referenceId: request.PayloadData.TransactionId,
                    description: "Mua Diamond từ Payment Gateway",
                    metadataJson: null,
                    idempotencyKey: idempotencyKey,
                    cancellationToken: transactionCt
                );

                order.MarkAsSuccess(request.PayloadData.TransactionId, request.PayloadData.FxSnapshot);
            }
            else
            {
                // Nếu FAILED từ provider
                order.MarkAsFailed(request.PayloadData.TransactionId);
            }

            // 5. Cập nhật Order status
            await _depositOrderRepository.UpdateAsync(order, transactionCt);
            handled = true;
        }, cancellationToken);

        return handled;
    }
}
