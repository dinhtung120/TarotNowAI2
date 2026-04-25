using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý yêu cầu đăng ký Reader trên Mongo.
/// </summary>
public class MongoReaderRequestRepository : IReaderRequestRepository
{
    private const string PendingRequestUniqueIndexName = "idx_user_pending_unique";
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
        try
        {
            await _context.ReaderRequests.InsertOneAsync(doc, cancellationToken: cancellationToken);
            request.Id = doc.Id;
            request.Version = doc.Version;
        }
        catch (MongoWriteException exception) when (IsPendingUniqueViolation(exception))
        {
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");
        }
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
        var currentVersion = Math.Max(1, request.Version);
        doc.Version = currentVersion + 1;

        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, doc.Id),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Version, currentVersion));
        var result = await _context.ReaderRequests.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
        if (result.ModifiedCount == 0)
        {
            throw new InvalidOperationException("Reader request has been modified by another process.");
        }

        request.Version = doc.Version;
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
            UpdatedAt = dto.UpdatedAt,
            ReviewHistory = dto.ReviewHistory?.Select(ToReviewHistoryDocument).ToList() ?? [],
            Version = dto.Version <= 0 ? 1 : dto.Version
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
            UpdatedAt = doc.UpdatedAt,
            ReviewHistory = doc.ReviewHistory?.Select(ToReviewHistoryDto).ToList() ?? [],
            Version = doc.Version <= 0 ? 1 : doc.Version
        };
    }

    private static bool IsPendingUniqueViolation(MongoWriteException exception)
    {
        if (exception.WriteError.Category != ServerErrorCategory.DuplicateKey)
        {
            return false;
        }

        var message = exception.Message;
        return string.IsNullOrWhiteSpace(message) == false
               && message.Contains(PendingRequestUniqueIndexName, StringComparison.OrdinalIgnoreCase);
    }

    private static ReaderRequestReviewHistoryEntryDocument ToReviewHistoryDocument(ReaderRequestReviewHistoryEntryDto dto)
    {
        return new ReaderRequestReviewHistoryEntryDocument
        {
            Action = dto.Action,
            Status = dto.Status,
            ReviewedBy = dto.ReviewedBy,
            AdminNote = dto.AdminNote,
            ReviewedAt = dto.ReviewedAt
        };
    }

    private static ReaderRequestReviewHistoryEntryDto ToReviewHistoryDto(ReaderRequestReviewHistoryEntryDocument doc)
    {
        return new ReaderRequestReviewHistoryEntryDto
        {
            Action = doc.Action,
            Status = doc.Status,
            ReviewedBy = doc.ReviewedBy,
            AdminNote = doc.AdminNote,
            ReviewedAt = doc.ReviewedAt
        };
    }
}
