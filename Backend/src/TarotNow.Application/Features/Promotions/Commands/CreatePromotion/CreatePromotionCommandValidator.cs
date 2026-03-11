using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.MinAmountVnd).GreaterThan(0);
        RuleFor(x => x.BonusDiamond).GreaterThan(0);
    }
}
