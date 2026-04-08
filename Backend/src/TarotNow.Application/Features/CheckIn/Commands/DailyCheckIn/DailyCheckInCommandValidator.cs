using FluentValidation;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

// Validator đầu vào cho command daily check-in.
public class DailyCheckInCommandValidator : AbstractValidator<DailyCheckInCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho DailyCheckInCommand.
    /// Luồng xử lý: bắt buộc UserId có giá trị để định vị user check-in.
    /// </summary>
    public DailyCheckInCommandValidator()
    {
        RuleFor(v => v.UserId).NotEmpty().WithMessage("UserId là bắt buộc.");
    }
}
