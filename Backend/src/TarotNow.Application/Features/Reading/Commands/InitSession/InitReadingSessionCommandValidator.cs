using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public class InitReadingSessionCommandValidator : AbstractValidator<InitReadingSessionCommand>
{
    public InitReadingSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.SpreadType)
            .NotEmpty()
            .Must(spreadType => spreadType is SpreadType.Daily1Card
                or SpreadType.Spread3Cards
                or SpreadType.Spread5Cards
                or SpreadType.Spread10Cards)
            .WithMessage("SpreadType không hợp lệ.");

        RuleFor(x => x.Question)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.Question) == false);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currency =>
            {
                var normalized = currency?.Trim().ToLowerInvariant();
                return normalized is CurrencyType.Gold or CurrencyType.Diamond;
            })
            .WithMessage("Currency phải là gold hoặc diamond.");
    }
}
