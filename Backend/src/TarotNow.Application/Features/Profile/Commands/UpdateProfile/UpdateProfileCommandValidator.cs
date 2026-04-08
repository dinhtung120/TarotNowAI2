using FluentValidation;
using System;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

// Validator cho command cập nhật profile.
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu cập nhật profile.
    /// Luồng xử lý: kiểm tra trường bắt buộc, giới hạn độ dài display name và bảo đảm ngày sinh nằm trong quá khứ.
    /// </summary>
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
        // UserId bắt buộc để xác định đúng hồ sơ cần cập nhật.

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("DisplayName is required.")
            .MaximumLength(100)
            .WithMessage("DisplayName must not exceed 100 characters.");
        // Giới hạn độ dài giúp đồng bộ UI và tránh payload bất thường.

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("DateOfBirth is required.")
            .LessThan(DateTime.UtcNow)
            .WithMessage("DateOfBirth must be in the past.");
        // Business rule: ngày sinh không thể ở tương lai.
    }
}
