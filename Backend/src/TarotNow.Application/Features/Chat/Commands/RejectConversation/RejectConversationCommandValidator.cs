using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public class RejectConversationCommandValidator : AbstractValidator<RejectConversationCommand>
{
    public RejectConversationCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.ReaderId)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.Reason) == false);
    }
}
