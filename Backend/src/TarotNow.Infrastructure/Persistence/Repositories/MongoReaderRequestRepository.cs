/*
 * FILE: MongoReaderRequestRepository.cs
 * MỤC ĐÍCH: Repository quản lý đơn xin làm Reader từ MongoDB (collection "reader_requests").
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: tạo đơn xin mới
 *   → GetByIdAsync: lấy theo ObjectId
 *   → GetLatestByUserIdAsync: lấy đơn MỚI NHẤT của User (kiểm tra đã gửi đơn chưa)
 *   → GetPaginatedAsync: phân trang cho Admin approval queue
 *   → UpdateAsync: cập nhật (Admin duyệt/từ chối)
 *
 *   MAPPING: DTO ↔ Document thủ công (không dùng AutoMapper → document nhỏ ~10 fields).
 */

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IReaderRequestRepository — đọc/ghi đơn xin làm Reader từ MongoDB.
/// </summary>
public class MongoReaderRequestRepository : IReaderRequestRepository
{
    private readonly MongoDbContext _context;

    public MongoReaderRequestRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo đơn xin mới, gán ObjectId vừa sinh về DTO.</summary>
    public async Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        await _context.ReaderRequests.InsertOneAsync(doc, cancellationToken: cancellationToken);
        // Ghi ObjectId vừa sinh vào DTO → caller sử dụng
        request.Id = doc.Id;
    }

    /// <summary>Lấy đơn theo ObjectId, chỉ lấy chưa xóa (IsDeleted = false).</summary>
    public async Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, id),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderRequests.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy đơn MỚI NHẤT của User — dùng để kiểm tra:
    ///   - User đã gửi đơn chưa? (nếu có → chặn gửi lại)
    ///   - Đơn cũ status gì? (pending → chờ duyệt, approved → đã là Reader)
    /// SortByDescending(CreatedAt): nếu User gửi nhiều đơn → lấy cái mới nhất.
    /// </summary>
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

    /// <summary>
    /// Phân trang cho Admin approval queue (hàng đợi duyệt đơn).
    /// Hỗ trợ filter theo status: "pending" (chờ duyệt), "approved", "rejected".
    /// Admin thường filter "pending" để xem đơn cần xử lý.
    /// Sắp xếp: mới nhất trước (chưa xếp theo priority vì chưa có urgency level).
    /// </summary>
    public async Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

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
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>Cập nhật đơn: replace toàn bộ document + set UpdatedAt.</summary>
    public async Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderRequests.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    // ==================== MAPPING ====================

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
