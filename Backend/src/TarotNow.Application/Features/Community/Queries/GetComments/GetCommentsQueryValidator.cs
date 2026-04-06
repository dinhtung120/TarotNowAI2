using FluentValidation;

namespace TarotNow.Application.Features.Community.Queries.GetComments;

/// <summary>
/// FluentValidation rules for listing comments of a post.
/// </summary>
public sealed class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}
