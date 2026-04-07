

using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
{
    public SubscribeCommandValidator()
    {
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        
        RuleFor(x => x.PlanId)
            .NotEmpty()
            .WithMessage("PlanId is required.");

        
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey is required to prevent duplicate transactions.")
            .MaximumLength(200)
            .WithMessage("IdempotencyKey must not exceed 200 characters.");
    }
}
