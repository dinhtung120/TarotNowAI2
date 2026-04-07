using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public class RequestConversationAddMoneyCommandValidator : AbstractValidator<RequestConversationAddMoneyCommand>
{
    public RequestConversationAddMoneyCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.ReaderId)
            .NotEmpty();

        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.Description) == false);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
