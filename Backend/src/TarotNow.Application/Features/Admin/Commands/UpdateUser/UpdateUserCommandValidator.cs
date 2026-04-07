using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Status)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DiamondBalance)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.GoldBalance)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
