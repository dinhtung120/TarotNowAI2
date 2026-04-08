using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.UpdatePost;

// Validator đầu vào cho command cập nhật bài viết.
public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho UpdatePostCommand.
    /// Luồng xử lý: bắt buộc PostId/AuthorId/Content và giới hạn độ dài content.
    /// </summary>
    public UpdatePostCommandValidator()
    {
        // PostId bắt buộc để định vị bài viết cần sửa.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // AuthorId bắt buộc để kiểm tra quyền sở hữu.
        RuleFor(x => x.AuthorId)
            .NotEmpty();

        // Content bắt buộc và giới hạn 5000 ký tự.
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);
    }
}
