using FluentValidation;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

/// <summary>
/// FluentValidation rules for loading one community post detail.
/// </summary>
public sealed class GetPostDetailQueryValidator : AbstractValidator<GetPostDetailQuery>
{
    public GetPostDetailQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ViewerId)
            .NotEmpty();
    }
}
