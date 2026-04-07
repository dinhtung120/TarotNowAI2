using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

public class QueueChatModerationCommandValidator : AbstractValidator<QueueChatModerationCommand>
{
    public QueueChatModerationCommandValidator()
    {
        RuleFor(x => x.Payload)
            .NotNull();

        RuleFor(x => x.Payload.MessageId)
            .NotEmpty();

        RuleFor(x => x.Payload.ConversationId)
            .NotEmpty();

        RuleFor(x => x.Payload.SenderId)
            .NotEmpty();

        RuleFor(x => x.Payload.Type)
            .NotEmpty();

        RuleFor(x => x.Payload.Content)
            .NotEmpty();
    }
}
