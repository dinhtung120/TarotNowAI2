using FluentValidation;
using TarotNow.Application.Features.Community;

namespace TarotNow.Application.Features.Community.Commands.ToggleReaction;

/// <summary>
/// FluentValidation rules for toggling a post reaction.
/// </summary>
public sealed class ToggleReactionCommandValidator : AbstractValidator<ToggleReactionCommand>
{
    public ToggleReactionCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ReactionType)
            .NotEmpty()
            .Must(type => CommunityModuleConstants.SupportedReactionTypes.Contains(type))
            .WithMessage("Unsupported reaction type.");
    }
}
