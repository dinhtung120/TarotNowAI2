using MediatR;

namespace TarotNow.Application.Features.Deposit.Commands.ReconcileMyDepositOrder;

/// <summary>
/// Command yêu cầu reconcile trạng thái đơn nạp của user theo dữ liệu PayOS.
/// </summary>
public sealed class ReconcileMyDepositOrderCommand : IRequest<bool>
{
    /// <summary>
    /// User gửi yêu cầu reconcile.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Id đơn nạp cần reconcile.
    /// </summary>
    public Guid OrderId { get; set; }
}
