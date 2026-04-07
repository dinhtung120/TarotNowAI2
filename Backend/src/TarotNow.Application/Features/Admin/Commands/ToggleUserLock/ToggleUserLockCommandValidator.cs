using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

public class ToggleUserLockCommandValidator : AbstractValidator<ToggleUserLockCommand>
{
    public ToggleUserLockCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
