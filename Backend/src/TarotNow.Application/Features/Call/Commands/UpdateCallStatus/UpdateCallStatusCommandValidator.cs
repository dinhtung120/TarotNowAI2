using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.UpdateCallStatus;

public sealed class UpdateCallStatusCommandValidator : AbstractValidator<UpdateCallStatusCommand>
{
    private static readonly string[] AllowedStatuses = ["requested", "accepted", "rejected", "ended"];

    public UpdateCallStatusCommandValidator()
    {
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status.Trim().ToLowerInvariant()))
            .WithMessage("Unsupported target call status.");

        RuleFor(x => x.ExpectedPreviousStatus)
            .Must(status => string.IsNullOrWhiteSpace(status)
                || AllowedStatuses.Contains(status.Trim().ToLowerInvariant()))
            .WithMessage("Unsupported expected previous call status.");
    }
}
