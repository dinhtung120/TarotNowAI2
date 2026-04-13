using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

// Command xử lý duyệt hoặc từ chối một lệnh nạp tiền.
public class ProcessDepositCommand : IRequest<bool>
{
    // Định danh lệnh nạp tiền cần xử lý.
    public Guid DepositId { get; set; }

    // Hành động xử lý (approve/reject).
    public string Action { get; set; } = ApproveAction;

    // Mã giao dịch từ phía admin hoặc hệ thống đối soát.
    public string? TransactionId { get; set; }

    // Action hợp lệ cho nhánh phê duyệt lệnh nạp.
    internal const string ApproveAction = "approve";
    // Action hợp lệ cho nhánh từ chối lệnh nạp.
    internal const string RejectAction = "reject";
}

// Handler xử lý nghiệp vụ duyệt/từ chối lệnh nạp tiền.
public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    // Chỉ xử lý lệnh nạp đang ở trạng thái Pending.
    private const string PendingStatus = "Pending";
    // Reference source chuẩn cho giao dịch credit phát sinh từ deposit order.
    private const string DepositReferenceSource = "DepositOrder";
    // Ghi chú mặc định khi admin duyệt thủ công.
    private const string ManualApprovalNote = "Admin Manual Approval";
    // Thông điệp lỗi action không hợp lệ.
    private const string InvalidActionMessage = "Action phải là 'approve' hoặc 'reject'.";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler process deposit.
    /// Luồng xử lý: nhận repository lệnh nạp và ví để cập nhật trạng thái + cộng tiền.
    /// </summary>
    public ProcessDepositCommandHandler(
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command duyệt hoặc từ chối lệnh nạp tiền.
    /// Luồng xử lý: tải order pending, chuẩn hóa action/transaction id, rẽ nhánh approve hoặc reject rồi lưu.
    /// </summary>
    public async Task<bool> Handle(ProcessDepositCommand request, CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdAsync(request.DepositId, cancellationToken);
        if (order == null || order.Status != PendingStatus)
        {
            // Edge case không tìm thấy order hoặc order không còn pending: không xử lý tiếp để tránh đụng trạng thái sai.
            return false;
        }

        var action = ValidateAndNormalizeAction(request.Action);
        var txnId = ResolveTransactionId(request.TransactionId, action);

        if (action == ProcessDepositCommand.ApproveAction)
        {
            // Nhánh approve: cộng ví trước, sau đó đánh dấu order thành công.
            await ApproveOrderAsync(order, txnId, cancellationToken);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
            return true;
        }

        // Nhánh reject: đánh dấu order failed và lưu lại transaction id truy vết.
        order.MarkAsFailed(txnId);
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        return true;
    }

    /// <summary>
    /// Chuẩn hóa và kiểm tra action đầu vào.
    /// Luồng xử lý: trim+lower action, chỉ chấp nhận approve/reject, ném BadRequest nếu không hợp lệ.
    /// </summary>
    private static string ValidateAndNormalizeAction(string? action)
    {
        var normalizedAction = action?.Trim().ToLowerInvariant();
        if (normalizedAction == ProcessDepositCommand.ApproveAction ||
            normalizedAction == ProcessDepositCommand.RejectAction)
        {
            return normalizedAction;
        }

        // Rule business: action ngoài danh sách cho phép phải bị chặn ngay tại biên command.
        throw new BadRequestException(InvalidActionMessage);
    }

    /// <summary>
    /// Thực thi nhánh approve deposit: cộng ví và chốt trạng thái order thành công.
    /// Luồng xử lý: credit ví bằng giao dịch Deposit có idempotency key cố định theo order id rồi mark success.
    /// </summary>
    private async Task ApproveOrderAsync(
        Domain.Entities.DepositOrder order,
        string transactionId,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: order.UserId,
            currency: CurrencyType.Diamond,
            type: TransactionType.Deposit,
            amount: order.DiamondAmount,
            referenceSource: DepositReferenceSource,
            referenceId: order.Id.ToString(),
            description: $"Approved deposit order {order.Id} (+{order.DiamondAmount} Diamond)",
            idempotencyKey: $"deposit_approve_{order.Id}",
            cancellationToken: cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.MoneyChangedDomainEvent
            {
                UserId = order.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.Deposit,
                DeltaAmount = order.DiamondAmount,
                ReferenceId = transactionId
            },
            cancellationToken);

        // Đổi state order sang success sau khi credit thành công để đảm bảo tính nhất quán tài chính.
        order.MarkAsSuccess(transactionId, ManualApprovalNote);
    }

    /// <summary>
    /// Resolve transaction id cho thao tác admin xử lý lệnh nạp.
    /// Luồng xử lý: ưu tiên transaction id từ request, fallback sinh mã định danh mới có chứa action.
    /// </summary>
    private static string ResolveTransactionId(string? requestedTransactionId, string action)
    {
        if (!string.IsNullOrWhiteSpace(requestedTransactionId))
        {
            // Ưu tiên transaction id do admin/hệ thống cung cấp để đồng bộ đối soát ngoài hệ thống.
            return requestedTransactionId.Trim();
        }

        // Edge case thiếu transaction id: sinh mã nội bộ để vẫn truy vết đầy đủ lịch sử xử lý.
        return $"ADMIN_{action.ToUpperInvariant()}_{Guid.CreateVersion7():N}";
    }
}
