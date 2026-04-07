using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.SenderId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(ChatMessageType.IsValid)
            .WithMessage("Loại tin nhắn không hợp lệ.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .When(x => x.Type == ChatMessageType.Text);
    }
}
