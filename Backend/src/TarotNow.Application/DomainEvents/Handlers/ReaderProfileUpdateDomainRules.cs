using TarotNow.Application.Common;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

internal static class ReaderProfileUpdateDomainRules
{
    private const int MinYearsOfExperience = 1;
    private const long MinDiamondPerQuestion = 50;

    public static void ApplyBioPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (domainEvent.BioVi is not null)
        {
            profile.BioVi = domainEvent.BioVi;
        }

        if (domainEvent.BioEn is not null)
        {
            profile.BioEn = domainEvent.BioEn;
        }

        if (domainEvent.BioZh is not null)
        {
            profile.BioZh = domainEvent.BioZh;
        }
    }

    public static void ApplyPricePatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (!domainEvent.DiamondPerQuestion.HasValue)
        {
            return;
        }

        if (domainEvent.DiamondPerQuestion.Value < MinDiamondPerQuestion)
        {
            throw new BadRequestException("Giá mỗi câu hỏi phải từ 50 Diamond.");
        }

        profile.DiamondPerQuestion = domainEvent.DiamondPerQuestion.Value;
    }

    public static void ApplySpecialtiesPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (domainEvent.Specialties is null)
        {
            return;
        }

        var specialties = ReaderSpecialties.NormalizeDistinct(domainEvent.Specialties).ToList();
        if (specialties.Count == 0)
        {
            throw new BadRequestException("Reader phải chọn ít nhất 1 chuyên môn.");
        }

        if (specialties.Any(x => ReaderSpecialties.IsSupported(x) == false))
        {
            throw new BadRequestException("Danh sách chuyên môn không hợp lệ.");
        }

        profile.Specialties = specialties;
    }

    public static void ApplyYearsOfExperiencePatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (!domainEvent.YearsOfExperience.HasValue)
        {
            return;
        }

        if (domainEvent.YearsOfExperience.Value < MinYearsOfExperience)
        {
            throw new BadRequestException("Số năm kinh nghiệm tối thiểu là 1.");
        }

        profile.YearsOfExperience = domainEvent.YearsOfExperience.Value;
    }

    public static void ApplySocialLinksPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        profile.FacebookUrl = ResolvePatchedSocialLink(
            domainEvent.FacebookUrl,
            profile.FacebookUrl,
            ReaderSocialUrlValidator.IsValidFacebookUrl,
            "FacebookUrl không hợp lệ hoặc không đúng domain Facebook.");

        profile.InstagramUrl = ResolvePatchedSocialLink(
            domainEvent.InstagramUrl,
            profile.InstagramUrl,
            ReaderSocialUrlValidator.IsValidInstagramUrl,
            "InstagramUrl không hợp lệ hoặc không đúng domain Instagram.");

        profile.TikTokUrl = ResolvePatchedSocialLink(
            domainEvent.TikTokUrl,
            profile.TikTokUrl,
            ReaderSocialUrlValidator.IsValidTikTokUrl,
            "TikTokUrl không hợp lệ hoặc không đúng domain TikTok.");
    }

    public static void EnsureProfileInvariants(ReaderProfileDto profile)
    {
        if (profile.Specialties.Count == 0)
        {
            throw new BadRequestException("Reader phải chọn ít nhất 1 chuyên môn.");
        }

        if (profile.YearsOfExperience < MinYearsOfExperience)
        {
            throw new BadRequestException("Số năm kinh nghiệm tối thiểu là 1.");
        }

        if (profile.DiamondPerQuestion < MinDiamondPerQuestion)
        {
            throw new BadRequestException("Giá mỗi câu hỏi phải từ 50 Diamond.");
        }

        if (!ReaderSocialUrlValidator.HasAtLeastOneSocialLink(profile.FacebookUrl, profile.InstagramUrl, profile.TikTokUrl))
        {
            throw new BadRequestException("Phải cung cấp ít nhất 1 link Facebook, Instagram hoặc TikTok.");
        }
    }

    private static string? ResolvePatchedSocialLink(
        string? incoming,
        string? current,
        Func<string?, bool> validator,
        string invalidMessage)
    {
        if (incoming is null)
        {
            return current;
        }

        var normalized = ReaderSocialUrlValidator.NormalizeOptionalUrl(incoming);
        if (normalized is null)
        {
            return null;
        }

        if (!validator(normalized))
        {
            throw new BadRequestException(invalidMessage);
        }

        return normalized;
    }
}
