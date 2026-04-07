

using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandValidator : AbstractValidator<CreateSubscriptionPlanCommand>
{
    public CreateSubscriptionPlanCommandValidator()
    {
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Plan name is required.")
            .MaximumLength(200).WithMessage("Plan name must not exceed 200 characters.");

        
        RuleFor(x => x.PriceDiamond)
            .GreaterThan(0).WithMessage("PriceDiamond must be greater than 0.");

        
        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365).WithMessage("DurationDays must be between 1 and 365.");

        
        RuleFor(x => x.EntitlementsJson)
            .NotEmpty().WithMessage("EntitlementsJson is required.");

        
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be 0 or positive.");
    }
}
