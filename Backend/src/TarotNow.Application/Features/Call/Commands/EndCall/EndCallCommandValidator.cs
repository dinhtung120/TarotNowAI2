using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

public sealed class EndCallCommandValidator : AbstractValidator<EndCallCommand>
{
    private static readonly string[] AllowedReasons = ["normal", "timeout", "cancelled", "disconnected", "timeout_server"];

    public EndCallCommandValidator()
    {
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .NotEmpty()
            .Must(reason => AllowedReasons.Contains(reason))
            .WithMessage("Unsupported call end reason.");
    }
}
