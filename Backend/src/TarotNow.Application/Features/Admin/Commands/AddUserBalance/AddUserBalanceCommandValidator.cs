

using FluentValidation; 
using TarotNow.Domain.Enums; 

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;


public class AddUserBalanceCommandValidator : AbstractValidator<AddUserBalanceCommand>
{
    public AddUserBalanceCommandValidator()
    {
        
        RuleFor(x => x.UserId).NotEmpty();

        
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount phải lớn hơn 0.");

        
        RuleFor(x => x.Currency)
            .Must(currency =>
            {
                var normalized = currency?.Trim().ToLowerInvariant();
                return normalized == CurrencyType.Gold || normalized == CurrencyType.Diamond;
            })
            .WithMessage("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
