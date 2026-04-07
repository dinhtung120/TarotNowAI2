

using FluentValidation;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public class RecordConsentCommandValidator : AbstractValidator<RecordConsentCommand>
{
    public RecordConsentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("DocumentType is required.")
            
            .Must(type => type == "TOS" || type == "PrivacyPolicy" || type == "AiDisclaimer")
            .WithMessage("DocumentType must be TOS, PrivacyPolicy, or AiDisclaimer.");

        RuleFor(x => x.Version)
            .NotEmpty().WithMessage("Version is required.");
    }
}
