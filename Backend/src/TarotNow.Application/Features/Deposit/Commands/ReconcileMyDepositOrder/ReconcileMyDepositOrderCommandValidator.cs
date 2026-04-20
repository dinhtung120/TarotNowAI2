using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.ReconcileMyDepositOrder;

/// <summary>
/// Validator đầu vào cho command reconcile đơn nạp.
/// </summary>
public sealed class ReconcileMyDepositOrderCommandValidator : AbstractValidator<ReconcileMyDepositOrderCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho command reconcile đơn nạp.
    /// </summary>
    public ReconcileMyDepositOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.OrderId)
            .NotEmpty();
    }
}
