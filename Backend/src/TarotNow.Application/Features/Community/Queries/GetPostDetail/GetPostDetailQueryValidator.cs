using FluentValidation;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

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
