using FluentValidation;
using TarotNow.Application.Features.Community;

namespace TarotNow.Application.Features.Community.Commands.ReportPost;

/// <summary>
/// FluentValidation rules for reporting a community post.
/// </summary>
public sealed class ReportPostCommandValidator : AbstractValidator<ReportPostCommand>
{
    public ReportPostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ReporterId)
            .NotEmpty();

        RuleFor(x => x.ReasonCode)
            .NotEmpty()
            .Must(code => CommunityModuleConstants.SupportedReportReasonCodes.Contains(code))
            .WithMessage("Unsupported reason code.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}
