using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation cho reader_profiles collection (MongoDB).
///
/// Map giữa ReaderProfileDto (Application) ↔ ReaderProfileDocument (Infrastructure).
/// Hỗ trợ bộ lọc nâng cao cho directory listing.
/// </summary>
public class MongoReaderProfileRepository : IReaderProfileRepository
{
    private readonly MongoDbContext _context;

    public MongoReaderProfileRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo profile — map DTO → Document rồi insert.</summary>
    public async Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        await _context.ReaderProfiles.InsertOneAsync(doc, cancellationToken: cancellationToken);
        profile.Id = doc.Id;
    }

    /// <summary>Lấy profile theo userId — unique index đảm bảo 1 kết quả.</summary>
    public async Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderProfiles.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>Cập nhật profile — map DTO → Document rồi replace.</summary>
    public async Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderProfileDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderProfiles.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    public async Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false));

        var update = Builders<ReaderProfileDocument>.Update
            .Set(r => r.IsDeleted, true)
            .Set(r => r.UpdatedAt, DateTime.UtcNow);

        await _context.ReaderProfiles.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Phân trang directory listing với bộ lọc nâng cao.
    /// Sắp xếp: accepting_questions trước, updated_at DESC.
    /// </summary>
    public async Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 12 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReaderProfileDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        // Filter theo chuyên môn
        if (!string.IsNullOrEmpty(specialty))
        {
            filter = filterBuilder.And(filter, filterBuilder.AnyEq(r => r.Specialties, specialty));
        }

        // Filter theo trạng thái online
        if (!string.IsNullOrEmpty(status))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, status));
        }

        // Tìm kiếm theo tên — case-insensitive regex
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var regex = new MongoDB.Bson.BsonRegularExpression(searchTerm, "i");
            filter = filterBuilder.And(filter, filterBuilder.Regex(r => r.DisplayName, regex));
        }

        var totalCount = await _context.ReaderProfiles.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var statusPriority = new BsonDocument("$switch", new BsonDocument
        {
            {
                "branches", new BsonArray
                {
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.AcceptingQuestions }) },
                        { "then", 0 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Online }) },
                        { "then", 1 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Offline }) },
                        { "then", 2 }
                    }
                }
            },
            { "default", 3 }
        });

        var docs = await _context.ReaderProfiles
            .Aggregate()
            .Match(filter)
            .AppendStage<BsonDocument>(new BsonDocument("$addFields", new BsonDocument("status_priority", statusPriority)))
            .AppendStage<BsonDocument>(new BsonDocument("$sort", new BsonDocument
            {
                { "status_priority", 1 },
                { "updated_at", -1 }
            }))
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .AppendStage<BsonDocument>(new BsonDocument("$project", new BsonDocument("status_priority", 0)))
            .As<ReaderProfileDocument>()
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    // ======================================================================
    // MAPPING HELPERS
    // ======================================================================

    /// <summary>Map Application DTO → MongoDB Document.</summary>
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

    /// <summary>Map MongoDB Document → Application DTO.</summary>
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
