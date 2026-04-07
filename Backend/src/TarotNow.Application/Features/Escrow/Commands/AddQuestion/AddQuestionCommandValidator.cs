using FluentValidation;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public AddQuestionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ConversationRef)
            .NotEmpty();

        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);

        RuleFor(x => x.ProposalMessageRef)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.ProposalMessageRef) == false);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}
