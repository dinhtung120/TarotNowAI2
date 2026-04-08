using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.DeletePost;

// Validator đầu vào cho command xóa bài viết.
public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho DeletePostCommand.
    /// Luồng xử lý: bắt buộc PostId/RequesterId/RequesterRole và giới hạn chiều dài role.
    /// </summary>
    public DeletePostCommandValidator()
    {
        // PostId bắt buộc để định vị bài viết cần xóa.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // RequesterId bắt buộc để kiểm tra quyền.
        RuleFor(x => x.RequesterId)
            .NotEmpty();

        // RequesterRole bắt buộc để phân nhánh quyền admin.
        RuleFor(x => x.RequesterRole)
            .NotEmpty()
            .MaximumLength(50);
    }
}
