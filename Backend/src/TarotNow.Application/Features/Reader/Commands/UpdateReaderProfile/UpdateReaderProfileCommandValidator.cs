using FluentValidation;
using TarotNow.Application.Common.Helpers;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

/// <summary>
/// Validator cho command cập nhật hồ sơ Reader.
/// </summary>
public sealed class UpdateReaderProfileCommandValidator : AbstractValidator<UpdateReaderProfileCommand>
{
    private const int MaxBioLength = 4_000;
    private const int MinYearsOfExperience = 1;
    private const long MinDiamondPerQuestion = 50;

    /// <summary>
    /// Khởi tạo rule validation cho cập nhật hồ sơ Reader.
    /// </summary>
    public UpdateReaderProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.BioVi)
            .MaximumLength(MaxBioLength)
            .When(x => string.IsNullOrWhiteSpace(x.BioVi) == false);

        RuleFor(x => x.BioEn)
            .MaximumLength(MaxBioLength)
            .When(x => string.IsNullOrWhiteSpace(x.BioEn) == false);

        RuleFor(x => x.BioZh)
            .MaximumLength(MaxBioLength)
            .When(x => string.IsNullOrWhiteSpace(x.BioZh) == false);

        RuleFor(x => x.DiamondPerQuestion)
            .GreaterThanOrEqualTo(MinDiamondPerQuestion)
            .When(x => x.DiamondPerQuestion.HasValue)
            .WithMessage("Giá mỗi câu hỏi phải từ 50 Diamond.");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(MinYearsOfExperience)
            .When(x => x.YearsOfExperience.HasValue)
            .WithMessage("Số năm kinh nghiệm tối thiểu là 1.");

        RuleForEach(x => x.Specialties!)
            .Must(ReaderSpecialties.IsSupported)
            .WithMessage("Chuyên môn không hợp lệ.")
            .When(x => x.Specialties is not null);

        RuleFor(x => x.FacebookUrl)
            .Must(ReaderSocialUrlValidator.IsValidFacebookUrl)
            .WithMessage("FacebookUrl không hợp lệ hoặc không đúng domain Facebook.");

        RuleFor(x => x.InstagramUrl)
            .Must(ReaderSocialUrlValidator.IsValidInstagramUrl)
            .WithMessage("InstagramUrl không hợp lệ hoặc không đúng domain Instagram.");

        RuleFor(x => x.TikTokUrl)
            .Must(ReaderSocialUrlValidator.IsValidTikTokUrl)
            .WithMessage("TikTokUrl không hợp lệ hoặc không đúng domain TikTok.");
    }
}
