using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public class RespondConversationCompleteCommandValidator : AbstractValidator<RespondConversationCompleteCommand>
{
    public RespondConversationCompleteCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
