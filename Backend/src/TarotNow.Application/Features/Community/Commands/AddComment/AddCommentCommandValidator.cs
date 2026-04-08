using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.AddComment;

// Validator đầu vào cho command thêm bình luận.
public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho AddCommentCommand.
    /// Luồng xử lý: bắt buộc PostId/AuthorId/Content và giới hạn độ dài content.
    /// </summary>
    public AddCommentCommandValidator()
    {
        // PostId bắt buộc để định vị bài viết.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // AuthorId bắt buộc để kiểm tra quyền và truy vết tác giả bình luận.
        RuleFor(x => x.AuthorId)
            .NotEmpty();

        // Content bắt buộc và không vượt giới hạn nghiệp vụ.
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
