using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public class RequestConversationCompleteCommandValidator : AbstractValidator<RequestConversationCompleteCommand>
{
    public RequestConversationCompleteCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.RequesterId)
            .NotEmpty();
    }
}
