using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;

public class CancelPendingConversationCommandValidator : AbstractValidator<CancelPendingConversationCommand>
{
    public CancelPendingConversationCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
