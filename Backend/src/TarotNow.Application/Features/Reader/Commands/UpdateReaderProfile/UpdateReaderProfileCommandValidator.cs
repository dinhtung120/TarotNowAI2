using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

public class UpdateReaderProfileCommandValidator : AbstractValidator<UpdateReaderProfileCommand>
{
    public UpdateReaderProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.BioVi)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioVi) == false);

        RuleFor(x => x.BioEn)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioEn) == false);

        RuleFor(x => x.BioZh)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioZh) == false);

        RuleFor(x => x.DiamondPerQuestion)
            .GreaterThan(0)
            .When(x => x.DiamondPerQuestion.HasValue);

        RuleForEach(x => x.Specialties!)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Specialties is { Count: > 0 });
    }
}
