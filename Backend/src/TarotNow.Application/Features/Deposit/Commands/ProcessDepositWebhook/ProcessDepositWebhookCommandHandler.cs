

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public partial class ProcessDepositWebhookCommandHandler : IRequestHandler<ProcessDepositWebhookCommand, bool>
{
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IWalletPushService _walletPushService;

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

    public async Task<bool> Handle(ProcessDepositWebhookCommand request, CancellationToken cancellationToken)
    {
        ValidateWebhookPayload(request);
        var isSuccessStatus = ResolveWebhookStatus(request.PayloadData.Status);
        VerifyWebhookSignature(request);
        var orderId = ParseOrderId(request.PayloadData.OrderId);
        return await ProcessWebhookAsync(request, orderId, isSuccessStatus, cancellationToken);
    }
}
