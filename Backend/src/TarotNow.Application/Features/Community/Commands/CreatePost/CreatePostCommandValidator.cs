using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.CreatePost;

// Validator đầu vào cho command tạo bài viết.
public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho CreatePostCommand.
    /// Luồng xử lý: bắt buộc AuthorId/Content và giới hạn Visibility trong tập public/private.
    /// </summary>
    public CreatePostCommandValidator()
    {
        // AuthorId bắt buộc để xác định chủ bài viết.
        RuleFor(x => x.AuthorId)
            .NotEmpty();

        // Content bắt buộc và giới hạn 5000 ký tự.
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);

        // Visibility chỉ nhận public/private theo enum domain.
        RuleFor(x => x.Visibility)
            .Must(v => v is PostVisibility.Public or PostVisibility.Private)
            .WithMessage("Visibility must be either 'public' or 'private'.");
    }
}
