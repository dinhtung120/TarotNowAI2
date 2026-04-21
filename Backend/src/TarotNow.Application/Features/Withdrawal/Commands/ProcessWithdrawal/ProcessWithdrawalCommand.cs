using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

// Command để admin duyệt hoặc từ chối yêu cầu rút tiền.
public class ProcessWithdrawalCommand : IRequest<bool>
{
    // Định danh yêu cầu rút cần xử lý.
    public Guid RequestId { get; set; }

    // Định danh admin thực hiện thao tác.
    public Guid AdminId { get; set; }

    // Hành động xử lý (approve/reject).
    public string Action { get; set; } = string.Empty;

    // Ghi chú admin khi xử lý (tùy chọn).
    public string? AdminNote { get; set; }

    // Idempotency key cho thao tác process.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// Handler process withdrawal theo Rule 0: chỉ publish domain event.
public sealed class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler xử lý withdrawal.
    /// Luồng xử lý: nhận dispatcher để publish domain event và tách side-effect sang event handlers.
    /// </summary>
    public ProcessWithdrawalCommandHandler(
        IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command duyệt/từ chối withdrawal.
    /// Luồng xử lý: publish domain event process và trả kết quả thành công khi event được xử lý xong.
    /// </summary>
    public async Task<bool> Handle(ProcessWithdrawalCommand requestCommand, CancellationToken cancellationToken)
    {
        var domainEvent = new WithdrawalProcessRequestedDomainEvent
        {
            RequestId = requestCommand.RequestId,
            AdminId = requestCommand.AdminId,
            Action = requestCommand.Action,
            AdminNote = requestCommand.AdminNote,
            IdempotencyKey = requestCommand.IdempotencyKey
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return true;
    }
}
