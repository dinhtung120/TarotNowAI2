using FluentValidation;

namespace TarotNow.Application.Features.Community.Queries.GetComments;

// Validator đầu vào cho query lấy bình luận.
public sealed class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    /// <summary>
    /// Khởi tạo rule validate cho GetCommentsQuery.
    /// Luồng xử lý: bắt buộc PostId và giới hạn Page/PageSize trong ngưỡng hợp lệ.
    /// </summary>
    public GetCommentsQueryValidator()
    {
        // PostId bắt buộc để định vị nguồn comment.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // Page tối thiểu bằng 1.
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        // PageSize giới hạn từ 1 đến 50.
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}
