using FluentValidation;
using System.IO;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

// Validator cho command upload avatar.
public class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu upload avatar.
    /// Luồng xử lý: kiểm tra user id, stream ảnh, tên file và content type để chặn request thiếu dữ liệu bắt buộc.
    /// </summary>
    public UploadAvatarCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để xác định đúng hồ sơ cần cập nhật avatar.

        RuleFor(x => x.ImageStream)
            .NotNull()
            .Must(stream => stream != Stream.Null)
            .WithMessage("ImageStream không hợp lệ.");
        // Stream phải khác Stream.Null để tránh request giả có metadata nhưng không có nội dung ảnh.

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);
        // Giới hạn tên file giúp tránh lỗi lưu trữ/path quá dài.

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);
        // Ràng buộc content type tối thiểu để handler xác thực MIME chính xác.
    }
}
