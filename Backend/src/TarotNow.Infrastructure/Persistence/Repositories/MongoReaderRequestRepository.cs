using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý yêu cầu đăng ký Reader trên Mongo.
/// </summary>
public class MongoReaderRequestRepository : IReaderRequestRepository
{
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository reader request.
    /// </summary>
    public MongoReaderRequestRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        await _context.ReaderRequests.InsertOneAsync(doc, cancellationToken: cancellationToken);
        request.Id = doc.Id;
    }

    /// <inheritdoc />
    public async Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, id),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false));

        var doc = await _context.ReaderRequests.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc is null ? null : ToDto(doc);
    }

    /// <inheritdoc />
    public async Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false));

        var doc = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc is null ? null : ToDto(doc);
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReaderRequestDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (string.IsNullOrWhiteSpace(statusFilter) == false)
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));
        }

        var totalCount = await _context.ReaderRequests.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderRequests.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    private static ReaderRequestDocument ToDocument(ReaderRequestDto dto)
    {
        return new ReaderRequestDocument
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            Bio = dto.Bio,
            Specialties = dto.Specialties,
            YearsOfExperience = dto.YearsOfExperience,
            FacebookUrl = dto.FacebookUrl,
            InstagramUrl = dto.InstagramUrl,
            TikTokUrl = dto.TikTokUrl,
            DiamondPerQuestion = dto.DiamondPerQuestion,
            ProofDocuments = dto.ProofDocuments,
            AdminNote = dto.AdminNote,
            ReviewedBy = dto.ReviewedBy,
            ReviewedAt = dto.ReviewedAt,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    private static ReaderRequestDto ToDto(ReaderRequestDocument doc)
    {
        return new ReaderRequestDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = doc.Status,
            Bio = doc.Bio,
            Specialties = doc.Specialties,
            YearsOfExperience = doc.YearsOfExperience,
            FacebookUrl = doc.FacebookUrl,
            InstagramUrl = doc.InstagramUrl,
            TikTokUrl = doc.TikTokUrl,
            DiamondPerQuestion = doc.DiamondPerQuestion,
            ProofDocuments = doc.ProofDocuments,
            AdminNote = doc.AdminNote,
            ReviewedBy = doc.ReviewedBy,
            ReviewedAt = doc.ReviewedAt,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
