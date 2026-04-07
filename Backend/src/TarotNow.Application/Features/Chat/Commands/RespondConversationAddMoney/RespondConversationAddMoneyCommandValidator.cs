using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public class RespondConversationAddMoneyCommandValidator : AbstractValidator<RespondConversationAddMoneyCommand>
{
    public RespondConversationAddMoneyCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.OfferMessageId)
            .NotEmpty();

        RuleFor(x => x.RejectReason)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(1000)
            .When(x => x.Accept == false);
    }
}
