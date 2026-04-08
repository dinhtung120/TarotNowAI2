using FluentValidation;
using TarotNow.Application.Features.Community;

namespace TarotNow.Application.Features.Community.Commands.ToggleReaction;

// Validator đầu vào cho command toggle reaction.
public sealed class ToggleReactionCommandValidator : AbstractValidator<ToggleReactionCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ToggleReactionCommand.
    /// Luồng xử lý: bắt buộc PostId/UserId/ReactionType và kiểm tra reaction type thuộc tập hỗ trợ.
    /// </summary>
    public ToggleReactionCommandValidator()
    {
        // PostId bắt buộc để định vị post.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // UserId bắt buộc để định vị reaction của người dùng.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // ReactionType bắt buộc và phải nằm trong danh sách hỗ trợ.
        RuleFor(x => x.ReactionType)
            .NotEmpty()
            .Must(type => CommunityModuleConstants.SupportedReactionTypes.Contains(type))
            .WithMessage("Unsupported reaction type.");
    }
}
