using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public class ApproveReaderCommandValidator : AbstractValidator<ApproveReaderCommand>
{
    public ApproveReaderCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action => action is "approve" or "reject")
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");

        RuleFor(x => x.AdminId)
            .NotEmpty();

        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}
