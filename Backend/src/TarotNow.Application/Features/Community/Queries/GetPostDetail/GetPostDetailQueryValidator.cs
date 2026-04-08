using FluentValidation;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

// Validator đầu vào cho query chi tiết bài viết.
public sealed class GetPostDetailQueryValidator : AbstractValidator<GetPostDetailQuery>
{
    /// <summary>
    /// Khởi tạo rule validate cho GetPostDetailQuery.
    /// Luồng xử lý: bắt buộc PostId và ViewerId để định vị dữ liệu + ngữ cảnh quyền xem.
    /// </summary>
    public GetPostDetailQueryValidator()
    {
        // PostId bắt buộc để định vị bài viết.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // ViewerId bắt buộc để xác thực quyền và reaction theo người xem.
        RuleFor(x => x.ViewerId)
            .NotEmpty();
    }
}
