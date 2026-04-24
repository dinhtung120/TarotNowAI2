using FluentValidation;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Validator cho command gửi đơn Reader.
/// </summary>
public sealed class SubmitReaderRequestValidator : AbstractValidator<SubmitReaderRequestCommand>
{
    private const int MinBioLength = 20;
    private const int MaxBioLength = 4_000;

    /// <summary>
    /// Khởi tạo rule validation cho luồng nộp đơn Reader.
    /// </summary>
    public SubmitReaderRequestValidator(ISystemConfigSettings systemConfigSettings)
    {
        var minYearsOfExperience = systemConfigSettings.ReaderMinYearsOfExperience;
        var minDiamondPerQuestion = systemConfigSettings.ReaderMinDiamondPerQuestion;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");

        RuleFor(x => x.Bio)
            .NotEmpty()
            .WithMessage("Lời giới thiệu không được để trống.")
            .MinimumLength(MinBioLength)
            .WithMessage($"Lời giới thiệu phải có ít nhất {MinBioLength} ký tự.")
            .MaximumLength(MaxBioLength)
            .WithMessage($"Lời giới thiệu không được vượt quá {MaxBioLength} ký tự.");

        RuleFor(x => x.Specialties)
            .NotNull()
            .Must(x => x is { Count: > 0 })
            .WithMessage("Reader phải chọn ít nhất 1 chuyên môn.");

        RuleForEach(x => x.Specialties)
            .Must(ReaderSpecialties.IsSupported)
            .WithMessage("Chuyên môn không hợp lệ.");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(minYearsOfExperience)
            .WithMessage($"Số năm kinh nghiệm tối thiểu là {minYearsOfExperience}.");

        RuleFor(x => x.DiamondPerQuestion)
            .GreaterThanOrEqualTo(minDiamondPerQuestion)
            .WithMessage($"Giá mỗi câu hỏi phải từ {minDiamondPerQuestion} Diamond.");

        RuleFor(x => x)
            .Must(x => ReaderSocialUrlValidator.HasAtLeastOneSocialLink(x.FacebookUrl, x.InstagramUrl, x.TikTokUrl))
            .WithMessage("Phải cung cấp ít nhất 1 link Facebook, Instagram hoặc TikTok.");

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
