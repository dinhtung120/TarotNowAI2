using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

/// <summary>
/// FluentValidation rules for responding to a call.
/// </summary>
public sealed class RespondCallCommandValidator : AbstractValidator<RespondCallCommand>
{
    public RespondCallCommandValidator()
    {
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        RuleFor(x => x.ResponderId)
            .NotEmpty();
    }
}
