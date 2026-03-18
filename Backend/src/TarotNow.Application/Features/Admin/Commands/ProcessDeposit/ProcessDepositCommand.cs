using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

/// <summary>
/// Lệnh xử lý đơn nạp tiền từ phía Admin.
/// </summary>
public class ProcessDepositCommand : IRequest<bool>
{
    /// <summary>ID của đơn nạp tiền.</summary>
    public Guid DepositId { get; set; }

    /// <summary>Hành động: approve hoặc reject.</summary>
    public string Action { get; set; } = "approve";

    /// <summary>Mã giao dịch thực tế (nếu có).</summary>
    public string? TransactionId { get; set; }
}

/// <summary>
/// Handler xử lý việc phê duyệt hoặc từ chối đơn nạp tiền.
/// </summary>
public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;

    public ProcessDepositCommandHandler(
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository)
    {
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
    }

    public async Task<bool> Handle(ProcessDepositCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm đơn nạp tiền
        var order = await _depositOrderRepository.GetByIdAsync(request.DepositId, cancellationToken);
        if (order == null || order.Status != "Pending") return false;

        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "approve" && action != "reject")
        {
            throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
        }

        var txnId = request.TransactionId ?? $"ADMIN_MANUAL_{Guid.NewGuid().ToString("N").Substring(0, 8)}";

        if (action == "approve")
        {
            // QUAN TRỌNG: Phải CreditAsync TRƯỚC khi MarkAsSuccess
            // Lý do: CreditAsync gọi _dbContext.SaveChangesAsync() bên trong.
            // Nếu MarkAsSuccess() được gọi trước, EF Core change tracker sẽ
            // track deposit order là "Modified" → SaveChangesAsync trong CreditAsync
            // cố gắng lưu cả deposit order → vi phạm unique constraint 
            // trên cột transaction_id (ix_deposit_orders_transaction_id).
            // → Giải pháp: Credit ví trước (order chưa bị modify), sau đó mới MarkAsSuccess.

            // 2. Cộng kim cương vào ví người dùng TRƯỚC
            await _walletRepository.CreditAsync(
                userId: order.UserId,
                currency: CurrencyType.Diamond,
                type: TransactionType.Deposit,
                amount: order.DiamondAmount,
                referenceSource: "DepositOrder",
                referenceId: order.Id.ToString(),
                description: $"Approved deposit order {order.Id} (+{order.DiamondAmount} Diamond)",
                idempotencyKey: $"deposit_approve_{order.Id}",
                cancellationToken: cancellationToken
            );

            // 3. SAU KHI credit thành công → cập nhật trạng thái đơn hàng
            order.MarkAsSuccess(txnId, "Admin Manual Approval");
        }
        else
        {
            // Từ chối đơn hàng
            order.MarkAsFailed(txnId);
        }

        // 4. Lưu thay đổi deposit order
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);

        return true;
    }
}
