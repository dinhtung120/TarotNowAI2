using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.CreatePost;

/// <summary>
/// FluentValidation rules for creating a community post.
/// </summary>
public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.Visibility)
            .Must(v => v is PostVisibility.Public or PostVisibility.Private)
            .WithMessage("Visibility must be either 'public' or 'private'.");
    }
}
