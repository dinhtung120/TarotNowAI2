using MediatR;

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
    /// <summary>
    /// Luồng duyệt tay nạp tiền đã bị vô hiệu hóa sau khi chuyển sang webhook PayOS.
    /// </summary>
    public async Task<bool> Handle(ProcessDepositCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return false;
    }
}
