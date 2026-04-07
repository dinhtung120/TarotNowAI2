

using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    public UpdatePromotionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.MinAmountVnd).GreaterThan(0);
        RuleFor(x => x.BonusDiamond).GreaterThan(0);
    }
}
