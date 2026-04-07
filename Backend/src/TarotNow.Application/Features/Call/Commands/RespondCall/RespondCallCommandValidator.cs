using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

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
