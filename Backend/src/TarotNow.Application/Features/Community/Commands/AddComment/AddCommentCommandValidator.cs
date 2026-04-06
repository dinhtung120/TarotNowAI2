using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.AddComment;

/// <summary>
/// FluentValidation rules for adding a community comment.
/// </summary>
public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.AuthorId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
