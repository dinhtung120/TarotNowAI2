/*
 * ===================================================================
 * FILE: ProcessDepositWebhookCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook
 * ===================================================================
 * MỤC ĐÍCH:
 *   Đây là trái tim của Nghiệp Vụ Tài Chính (Nạp tiền). Handler này hứng 
 *   lời gọi từ VNPay/MoMo và ra quyết định: Cộng Tiền hay Huỷ Bỏ Hoá Đơn.
 *
 * CHIẾN LƯỢC BẢO MẬT GIAO DỊCH TÀI CHÍNH CẤP CAO (ACID & IDEMPOTENCY):
 *   1. Chữ Ký Khóa Vàng (Webhook Signature): Phải xác minh gói tin xuất phát từ đúng VNPay, 
 *      ngăn chặn kẻ gian dùng IP ảo gọi API để tự cộng tiền chùa.
 *   2. Khóa Cơ Sở Dữ Liệu (Pessimistic Locking / GetByIdForUpdate): Ngăn chặn lỗi Race Condition 
 *      (VNPay gọi 2 Request cùng lúc -> Cộng x2 Diamond).
 *   3. Tính Lũy Đẳng (Idempotency Check): Nếu VNPay bị lag báo lỗi Timeout, nó sẽ 
 *      gửi lại luồng Webhook thêm 1 lần nữa. Server phải nhận diện "Bill này cộng tiền rồi, ignore".
 * ===================================================================
 */

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

/// <summary>
/// Bộ kiểm soát Giao Dịch Nạp Tiền An Toàn.
/// </summary>
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
