using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

/// <summary>
/// FluentValidation rules for initiating a call.
/// </summary>
public sealed class InitiateCallCommandValidator : AbstractValidator<InitiateCallCommand>
{
    public InitiateCallCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.InitiatorId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(type => type is "audio" or "video")
            .WithMessage("Call type must be 'audio' or 'video'.");
    }
}
