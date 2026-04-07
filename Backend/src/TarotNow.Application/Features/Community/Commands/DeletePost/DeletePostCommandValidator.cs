using FluentValidation;

namespace TarotNow.Application.Features.Community.Commands.DeletePost;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.RequesterId)
            .NotEmpty();

        RuleFor(x => x.RequesterRole)
            .NotEmpty()
            .MaximumLength(50);
    }
}
