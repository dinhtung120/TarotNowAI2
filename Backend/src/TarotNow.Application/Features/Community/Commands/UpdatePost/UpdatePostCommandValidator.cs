using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.UpdatePost;

/// <summary>
/// FluentValidation rules for updating a community post.
/// </summary>
public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.AuthorId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);
    }
}
