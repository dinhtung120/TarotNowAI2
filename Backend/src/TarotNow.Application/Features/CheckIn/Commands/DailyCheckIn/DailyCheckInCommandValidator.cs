using FluentValidation;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

public class DailyCheckInCommandValidator : AbstractValidator<DailyCheckInCommand>
{
    public DailyCheckInCommandValidator()
    {
        RuleFor(v => v.UserId).NotEmpty().WithMessage("UserId là bắt buộc.");
    }
}
