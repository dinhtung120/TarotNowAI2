

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Handler điều phối toàn bộ luồng xử lý webhook nạp tiền.
public partial class ProcessDepositWebhookCommandHandler : IRequestHandler<ProcessDepositWebhookCommand, bool>
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IWalletPushService _walletPushService;

    /// <summary>
    /// Khởi tạo handler process deposit webhook.
    /// Luồng xử lý: nhận service verify webhook, repository order/wallet, transaction coordinator và wallet push service.
    /// </summary>
    public ProcessDepositWebhookCommandHandler(
        IPaymentGatewayService paymentGatewayService,
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository,
        ITransactionCoordinator transactionCoordinator,
        IWalletPushService walletPushService)
    {
        _paymentGatewayService = paymentGatewayService;
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
        _transactionCoordinator = transactionCoordinator;
        _walletPushService = walletPushService;
    }

    /// <summary>
    /// Xử lý command webhook deposit.
    /// Luồng xử lý: validate payload, resolve status SUCCESS/FAILED, verify signature, parse order id và chuyển sang workflow transaction-safe.
    /// </summary>
    public async Task<bool> Handle(ProcessDepositWebhookCommand request, CancellationToken cancellationToken)
    {
        ValidateWebhookPayload(request);
        var isSuccessStatus = ResolveWebhookStatus(request.PayloadData.Status);
        // Chỉ xử lý tiếp khi chữ ký hợp lệ để bảo vệ luồng tài chính.
        VerifyWebhookSignature(request);
        var orderId = ParseOrderId(request.PayloadData.OrderId);
        return await ProcessWebhookAsync(request, orderId, isSuccessStatus, cancellationToken);
    }
}
