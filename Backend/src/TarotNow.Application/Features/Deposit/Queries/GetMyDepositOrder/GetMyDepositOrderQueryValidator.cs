using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Queries.GetMyDepositOrder;

// Validator cho query lấy trạng thái lệnh nạp.
public class GetMyDepositOrderQueryValidator : AbstractValidator<GetMyDepositOrderQuery>
{
    /// <summary>
    /// Khởi tạo rule validate cho GetMyDepositOrderQuery.
    /// </summary>
    public GetMyDepositOrderQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.OrderId)
            .NotEmpty();
    }
}
