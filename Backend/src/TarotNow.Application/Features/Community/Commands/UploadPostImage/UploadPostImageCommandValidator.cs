using FluentValidation;
using System.IO;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

// Validator đầu vào cho command upload ảnh bài viết.
public class UploadPostImageCommandValidator : AbstractValidator<UploadPostImageCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho UploadPostImageCommand.
    /// Luồng xử lý: kiểm tra stream không null/không phải Stream.Null, bắt buộc file name và content type hợp lệ.
    /// </summary>
    public UploadPostImageCommandValidator()
    {
        // ImageStream bắt buộc và không được là Stream.Null.
        RuleFor(x => x.ImageStream)
            .NotNull()
            .Must(stream => stream != Stream.Null)
            .WithMessage("ImageStream không hợp lệ.");

        // FileName bắt buộc và giới hạn độ dài tên file.
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        // ContentType bắt buộc và giới hạn độ dài metadata.
        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);
    }
}
