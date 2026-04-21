using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

// Command tạo yêu cầu rút tiền bằng diamond.
public class CreateWithdrawalCommand : IRequest<Guid>
{
    // Định danh user gửi yêu cầu rút.
    public Guid UserId { get; set; }

    // Số diamond muốn rút.
    public long AmountDiamond { get; set; }

    // Khóa idempotency để chống tạo yêu cầu trùng khi retry.
    public string IdempotencyKey { get; set; } = string.Empty;

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Tên chủ tài khoản ngân hàng.
    public string BankAccountName { get; set; } = string.Empty;

    // Số tài khoản ngân hàng.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Ghi chú từ user.
    public string? UserNote { get; set; }
}

// Handler tạo yêu cầu rút tiền theo Rule 0: chỉ publish domain event.
public sealed class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalCommand, Guid>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler tạo withdrawal request.
    /// Luồng xử lý: nhận dispatcher để publish domain event và tách toàn bộ side-effect sang event handlers.
    /// </summary>
    public CreateWithdrawalCommandHandler(
        IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command tạo yêu cầu rút.
    /// Luồng xử lý: publish domain event, để event handler xử lý freeze ví + persist request và trả về request id.
    /// </summary>
    public async Task<Guid> Handle(
        CreateWithdrawalCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = new WithdrawalCreateRequestedDomainEvent
        {
            UserId = request.UserId,
            AmountDiamond = request.AmountDiamond,
            IdempotencyKey = request.IdempotencyKey,
            BankName = request.BankName,
            BankAccountName = request.BankAccountName,
            BankAccountNumber = request.BankAccountNumber,
            UserNote = request.UserNote
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.RequestId;
    }
}
