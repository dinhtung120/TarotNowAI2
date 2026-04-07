using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;

public class OpenConversationDisputeCommandValidator : AbstractValidator<OpenConversationDisputeCommand>
{
    public OpenConversationDisputeCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ItemId)
            .NotEqual(Guid.Empty)
            .When(x => x.ItemId.HasValue);

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}
