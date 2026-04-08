using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

// Validator đầu vào cho command cấp tất cả title.
public class GrantAllTitlesCommandValidator : AbstractValidator<GrantAllTitlesCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho GrantAllTitlesCommand.
    /// Luồng xử lý: bắt buộc UserId có giá trị.
    /// </summary>
    public GrantAllTitlesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
