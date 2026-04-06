using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Queries.GetFeed;

/// <summary>
/// FluentValidation rules for loading community feed.
/// </summary>
public sealed class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    public GetFeedQueryValidator()
    {
        RuleFor(x => x.ViewerId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);

        RuleFor(x => x.VisibilityFilter)
            .Must(v => string.IsNullOrWhiteSpace(v) || v is PostVisibility.Public or PostVisibility.Private)
            .WithMessage("Visibility filter must be either 'public' or 'private'.");

        RuleFor(x => x.AuthorFilter)
            .MaximumLength(64)
            .When(x => string.IsNullOrWhiteSpace(x.AuthorFilter) == false);
    }
}
