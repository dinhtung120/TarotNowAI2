using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

public class MarkMessagesReadCommandValidator : AbstractValidator<MarkMessagesReadCommand>
{
    public MarkMessagesReadCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.ReaderId)
            .NotEmpty();
    }
}
