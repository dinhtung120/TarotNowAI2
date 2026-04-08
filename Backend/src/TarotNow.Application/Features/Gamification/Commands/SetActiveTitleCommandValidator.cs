using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

// Validator đầu vào cho command đặt active title.
public class SetActiveTitleCommandValidator : AbstractValidator<SetActiveTitleCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho SetActiveTitleCommand.
    /// Luồng xử lý: bắt buộc UserId và giới hạn độ dài TitleCode.
    /// </summary>
    public SetActiveTitleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.TitleCode)
            .MaximumLength(100);
    }
}
