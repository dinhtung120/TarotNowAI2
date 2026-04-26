using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Queries.GetMyDepositOrder;

// Handler lấy trạng thái lệnh nạp của user hiện tại.
public class GetMyDepositOrderQueryHandler : IRequestHandler<GetMyDepositOrderQuery, MyDepositOrderDto>
{
    private readonly IDepositOrderRepository _depositOrderRepository;

    /// <summary>
    /// Khởi tạo handler lấy trạng thái đơn nạp.
    /// </summary>
    public GetMyDepositOrderQueryHandler(IDepositOrderRepository depositOrderRepository)
    {
        _depositOrderRepository = depositOrderRepository;
    }

    /// <summary>
    /// Xử lý query lấy trạng thái đơn nạp.
    /// </summary>
    public async Task<MyDepositOrderDto> Handle(GetMyDepositOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdForUserAsync(
            request.OrderId,
            request.UserId,
            cancellationToken);

        if (order == null)
        {
            throw new NotFoundException($"Deposit order {request.OrderId} not found.");
        }

        return new MyDepositOrderDto
        {
            OrderId = order.Id,
            Status = order.Status,
            PackageCode = order.PackageCode,
            AmountVnd = order.AmountVnd,
            BaseDiamondAmount = order.BaseDiamondAmount,
            BonusGoldAmount = order.BonusGoldAmount,
            TotalDiamondAmount = order.DiamondAmount,
            PayOsOrderCode = order.PayOsOrderCode,
            PaymentLinkStatus = order.PaymentLinkStatus,
            CheckoutUrl = order.CheckoutUrl,
            QrCode = order.QrCode,
            PaymentLinkId = order.PayOsPaymentLinkId,
            TransactionId = order.TransactionId,
            FailureReason = order.FailureReason,
            ProcessedAt = order.ProcessedAt,
            ExpiresAtUtc = order.ExpiresAtUtc,
            PaymentLinkFailureReason = order.PaymentLinkFailureReason
        };
    }
}
