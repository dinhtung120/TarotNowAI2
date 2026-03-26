using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReaderProfileRepository
{
    private static ReaderProfileDocument ToDocument(ReaderProfileDto dto)
    {
        return new ReaderProfileDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            DisplayName = dto.DisplayName,
            AvatarUrl = dto.AvatarUrl,
            Pricing = new ReaderPricing { DiamondPerQuestion = dto.DiamondPerQuestion },
            Bio = new LocalizedText
            {
                Vi = dto.BioVi,
                En = dto.BioEn,
                Zh = dto.BioZh
            },
            Specialties = dto.Specialties,
            Stats = new ReaderStats
            {
                AvgRating = dto.AvgRating,
                TotalReviews = dto.TotalReviews
            },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    private static ReaderProfileDto ToDto(ReaderProfileDocument doc)
    {
        return new ReaderProfileDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = doc.Status,
            DiamondPerQuestion = doc.Pricing.DiamondPerQuestion,
            BioVi = doc.Bio.Vi,
            BioEn = doc.Bio.En,
            BioZh = doc.Bio.Zh,
            Specialties = doc.Specialties,
            AvgRating = doc.Stats.AvgRating,
            TotalReviews = doc.Stats.TotalReviews,
            DisplayName = doc.DisplayName,
            AvatarUrl = doc.AvatarUrl,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
