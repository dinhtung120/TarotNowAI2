using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial mapper chuyển đổi ReaderProfile DTO/document.
/// </summary>
public partial class MongoReaderProfileRepository
{
    /// <summary>
    /// Map ReaderProfileDto sang document Mongo.
    /// </summary>
    private static ReaderProfileDocument ToDocument(ReaderProfileDto dto)
    {
        return new ReaderProfileDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = ReaderOnlineStatus.NormalizeOrDefault(dto.Status),
            DisplayName = dto.DisplayName,
            AvatarUrl = dto.AvatarUrl,
            Pricing = new ReaderPricing
            {
                DiamondPerQuestion = dto.DiamondPerQuestion
            },
            Bio = new LocalizedText
            {
                Vi = dto.BioVi,
                En = dto.BioEn,
                Zh = dto.BioZh
            },
            Specialties = dto.Specialties,
            YearsOfExperience = dto.YearsOfExperience,
            SocialLinks = new ReaderSocialLinks
            {
                FacebookUrl = dto.FacebookUrl,
                InstagramUrl = dto.InstagramUrl,
                TikTokUrl = dto.TikTokUrl
            },
            Stats = new ReaderStats
            {
                AvgRating = dto.AvgRating,
                TotalReviews = dto.TotalReviews
            },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>
    /// Map ReaderProfileDocument sang DTO.
    /// </summary>
    private static ReaderProfileDto ToDto(ReaderProfileDocument doc)
    {
        return new ReaderProfileDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = ReaderOnlineStatus.NormalizeOrDefault(doc.Status),
            DiamondPerQuestion = doc.Pricing.DiamondPerQuestion,
            BioVi = doc.Bio.Vi,
            BioEn = doc.Bio.En,
            BioZh = doc.Bio.Zh,
            Specialties = doc.Specialties,
            YearsOfExperience = doc.YearsOfExperience,
            FacebookUrl = doc.SocialLinks.FacebookUrl,
            InstagramUrl = doc.SocialLinks.InstagramUrl,
            TikTokUrl = doc.SocialLinks.TikTokUrl,
            AvgRating = doc.Stats.AvgRating,
            TotalReviews = doc.Stats.TotalReviews,
            DisplayName = doc.DisplayName,
            AvatarUrl = doc.AvatarUrl,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
