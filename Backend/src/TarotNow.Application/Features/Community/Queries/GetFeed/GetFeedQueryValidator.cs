using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Queries.GetFeed;

// Validator đầu vào cho query lấy feed.
public sealed class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    /// <summary>
    /// Khởi tạo rule validate cho GetFeedQuery.
    /// Luồng xử lý: bắt buộc ViewerId, giới hạn paging, validate VisibilityFilter và chiều dài AuthorFilter.
    /// </summary>
    public GetFeedQueryValidator()
    {
        // ViewerId bắt buộc để enrich reaction theo người xem.
        RuleFor(x => x.ViewerId)
            .NotEmpty();

        // Page tối thiểu bằng 1.
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        // PageSize trong khoảng cho phép.
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);

        // VisibilityFilter chỉ cho phép public/private hoặc bỏ trống.
        RuleFor(x => x.VisibilityFilter)
            .Must(v => string.IsNullOrWhiteSpace(v) || v is PostVisibility.Public or PostVisibility.Private)
            .WithMessage("Visibility filter must be either 'public' or 'private'.");

        // AuthorFilter là tùy chọn nhưng giới hạn độ dài.
        RuleFor(x => x.AuthorFilter)
            .MaximumLength(64)
            .When(x => string.IsNullOrWhiteSpace(x.AuthorFilter) == false);
    }
}
