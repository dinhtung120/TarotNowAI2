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

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

/// <summary>
/// Bộ kiểm soát Giao Dịch Nạp Tiền An Toàn.
/// </summary>
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
        // -------------------------------------------------------------
        //  1. CHẶN VÀ ĐIỀU KIỆN TIÊN QUYẾT TỪ VNPay
        // -------------------------------------------------------------
        if (request.PayloadData.Amount <= 0)
            throw new BadRequestException("Invalid webhook amount.");

        if (string.IsNullOrWhiteSpace(request.PayloadData.TransactionId))
            throw new BadRequestException("Missing webhook transaction id.");

        var webhookStatus = request.PayloadData.Status?.Trim();
        var isSuccessStatus = string.Equals(webhookStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase);
        var isFailedStatus = string.Equals(webhookStatus, "FAILED", StringComparison.OrdinalIgnoreCase);
        
        if (!isSuccessStatus && !isFailedStatus)
            throw new BadRequestException($"Unsupported webhook status: {request.PayloadData.Status}");

        // -------------------------------------------------------------
        //  2. XÁC THỰC CHỮ KÝ HMAC SHA256 CỦA NGÂN HÀNG
        //  (Đây là bước sống còn để chống Tool Fake Request hack tiền)
        // -------------------------------------------------------------
        if (!_paymentGatewayService.VerifyWebhookSignature(request.RawPayload, request.Signature))
        {
            throw new UnauthorizedAccessException("Invalid webhook signature.");
        }

        // 3. Giải mã con số Đơn hàng
        if (!Guid.TryParse(request.PayloadData.OrderId, out var orderId))
        {
            throw new BadRequestException("Invalid OrderId format in webhook.");
        }

        var handled = false;
        
        // -------------------------------------------------------------
        //  4. MỞ TRANSACTION DATABASE (COMMIT OR ROLLBACK ALL)
        // -------------------------------------------------------------
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // Khoá ROW lại (FOR UPDATE) chống người khác đụng vào lúc đang xử lý.
            var order = await _depositOrderRepository.GetByIdForUpdateAsync(orderId, transactionCt)
                ?? throw new NotFoundException($"Deposit order {orderId} not found.");

            // So sánh số tiền VNPay báo về có khớp với đơn hàng lúc tạo không? (Chống hack truyền tham số mạo danh).
            if (request.PayloadData.Amount != order.AmountVnd)
                throw new BadRequestException("Webhook amount does not match order amount.");

            // -------------------------------------------------------------
            //  5. THUẬT TOÁN LUỸ ĐẲNG (IDEMPOTENCY CHECK)
            //  Cổng thanh toán rất hay gọi đúp (gọi 2 lần cùng 1 lúc).
            // -------------------------------------------------------------
            if (order.Status == "Success" || order.Status == "Failed")
            {
                // Nếu gọi trùng mà Mã giao dịch ngân hàng sai -> Hack cmnr -> Ném lỗi.
                if (!string.IsNullOrWhiteSpace(order.TransactionId) &&
                    !string.Equals(order.TransactionId, request.PayloadData.TransactionId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new BadRequestException("Processed order transaction id mismatch.");
                }

                handled = true;
                return; // Âm thầm Thoát ra, không cộng tiền lần 2 nữa.
            }

            // -------------------------------------------------------------
            //  6. TÁC ĐỘNG TÀI CHÍNH (CỘNG VÍ ĐIỆN TỬ DIAMOND)
            // -------------------------------------------------------------
            if (isSuccessStatus)
            {
                // Gắn chìa khoá Idempotency để nếu đứt mạng nửa chừng, tầng Repo cũng tự vệ được.
                var idempotencyKey = $"DEPOSIT_{order.Id}";

                await _walletRepository.CreditAsync(
                    userId: order.UserId,
                    currency: TarotNow.Domain.Enums.CurrencyType.Diamond,
                    type: TarotNow.Domain.Enums.TransactionType.Deposit,
                    amount: order.DiamondAmount, // Tính cả khuyến mãi.
                    referenceSource: "PaymentGateway",
                    referenceId: request.PayloadData.TransactionId,
                    description: "Mua Diamond từ Payment Gateway",
                    metadataJson: null,
                    idempotencyKey: idempotencyKey,
                    cancellationToken: transactionCt
                );

                // Đóng mộc Hoá đơn Thành công.
                order.MarkAsSuccess(request.PayloadData.TransactionId, request.PayloadData.FxSnapshot);
            }
            else
            {
                // Ngân hàng báo GD Thất Bại (Khách không đủ tiền móm).
                order.MarkAsFailed(request.PayloadData.TransactionId);
            }

            // 7. Cập nhật Trạng thái Hoá đơn.
            await _depositOrderRepository.UpdateAsync(order, transactionCt);
            handled = true; // Báo hiệu đã nuốt gọn gói tin.
            
        }, cancellationToken); // Cuối vòng block này, SQL sẽ tự COMMIT dữ liệu. Bất trắc gì thì TỰ ROLLBACK vứt hết.

        return handled;
    }
}
