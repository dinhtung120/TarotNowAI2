using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public class AcceptConversationCommandValidator : AbstractValidator<AcceptConversationCommand>
{
    public AcceptConversationCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.ReaderId)
            .NotEmpty();
    }
}
