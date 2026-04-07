using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommandValidator : AbstractValidator<DeletePromotionCommand>
{
    public DeletePromotionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
