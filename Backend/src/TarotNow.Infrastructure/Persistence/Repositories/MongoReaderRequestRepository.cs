using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation cho reader_requests collection (MongoDB).
///
/// Chịu trách nhiệm map giữa ReaderRequestDto (Application) ↔ ReaderRequestDocument (Infrastructure).
/// Tại sao map thủ công thay vì dùng AutoMapper?
/// → Document nhỏ (~10 fields) → map thủ công nhanh và rõ ràng hơn.
/// → Không thêm dependency AutoMapper vào Infrastructure project.
/// → Format nhất quán với existing repositories (VD: MongoReadingSessionRepository).
/// </summary>
public class MongoReaderRequestRepository : IReaderRequestRepository
{
    private readonly MongoDbContext _context;

    public MongoReaderRequestRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo mới — map DTO → Document rồi insert.</summary>
    public async Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        await _context.ReaderRequests.InsertOneAsync(doc, cancellationToken: cancellationToken);
        // Ghi lại Id do MongoDB generate vào DTO (caller có thể cần)
        request.Id = doc.Id;
    }

    /// <summary>Lấy theo ObjectId, filter is_deleted = false.</summary>
    public async Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, id),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderRequests.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>Lấy đơn mới nhất của user — sort by created_at DESC.</summary>
    public async Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    /// <summary>Phân trang cho admin approval queue.</summary>
    public async Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<ReaderRequestDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));
        }

        var totalCount = await _context.ReaderRequests.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>Cập nhật — map DTO → Document rồi replace.</summary>
    public async Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderRequests.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    // ======================================================================
    // MAPPING HELPERS
    // Map thủ công giữa DTO (Application) ↔ Document (Infrastructure).
    // ======================================================================

    /// <summary>Map Application DTO → MongoDB Document.</summary>
    private static ReaderRequestDocument ToDocument(ReaderRequestDto dto)
    {
        return new ReaderRequestDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            IntroText = dto.IntroText,
            ProofDocuments = dto.ProofDocuments,
            AdminNote = dto.AdminNote,
            ReviewedBy = dto.ReviewedBy,
            ReviewedAt = dto.ReviewedAt,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>Map MongoDB Document → Application DTO.</summary>
    private static ReaderRequestDto ToDto(ReaderRequestDocument doc)
    {
        return new ReaderRequestDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = doc.Status,
            IntroText = doc.IntroText,
            ProofDocuments = doc.ProofDocuments,
            AdminNote = doc.AdminNote,
            ReviewedBy = doc.ReviewedBy,
            ReviewedAt = doc.ReviewedAt,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
