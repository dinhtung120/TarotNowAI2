/*
 * FILE: MongoReportRepository.cs
 * MỤC ĐÍCH: Repository quản lý báo cáo vi phạm từ MongoDB (collection "reports").
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: tạo báo cáo mới (User báo cáo vi phạm)
 *   → GetPaginatedAsync: phân trang cho Admin xử lý queue báo cáo
 *
 *   MAPPING: DTO ↔ Document thủ công.
 *   Lưu ý: ReportTarget (nested object) được flatten khi map sang DTO (TargetType, TargetId).
 */

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IReportRepository — đọc/ghi báo cáo vi phạm từ MongoDB.
/// </summary>
public class MongoReportRepository : IReportRepository
{
    private readonly MongoDbContext _context;

    public MongoReportRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo báo cáo mới, gán ObjectId vừa sinh về DTO.</summary>
    public async Task AddAsync(ReportDto report, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(report);
        await _context.Reports.InsertOneAsync(doc, cancellationToken: cancellationToken);
        report.Id = doc.Id;
    }

    /// <summary>
    /// Phân trang cho Admin báo cáo queue.
    /// Hỗ trợ filter theo status: "pending" (chờ xử lý), "processing", "resolved", "rejected".
    /// Sắp xếp: mới nhất trước — Admin cần xem báo cáo gần đây ưu tiên.
    /// </summary>
    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReportDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        // Filter theo status nếu có
        if (!string.IsNullOrEmpty(statusFilter))
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));

        var totalCount = await _context.Reports.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.Reports.Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    // ==================== MAPPING ====================

    /// <summary>
    /// Map Application DTO → MongoDB Document.
    /// Lưu ý: DTO có TargetType + TargetId (flat) → Document có Target object (nested).
    /// </summary>
    private static ReportDocument ToDocument(ReportDto dto)
    {
        return new ReportDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ReporterId = dto.ReporterId,
            Target = new ReportTarget { Type = dto.TargetType, Id = dto.TargetId },
            ConversationRef = dto.ConversationRef,
            Reason = dto.Reason,
            Status = dto.Status,
            Result = dto.Result,
            AdminNote = dto.AdminNote,
            CreatedAt = dto.CreatedAt
        };
    }

    /// <summary>
    /// Map MongoDB Document → Application DTO.
    /// Flatten: Target { Type, Id } → TargetType, TargetId.
    /// </summary>
    private static ReportDto ToDto(ReportDocument doc)
    {
        return new ReportDto
        {
            Id = doc.Id,
            ReporterId = doc.ReporterId,
            TargetType = doc.Target.Type,
            TargetId = doc.Target.Id,
            ConversationRef = doc.ConversationRef,
            Reason = doc.Reason,
            Status = doc.Status,
            Result = doc.Result,
            AdminNote = doc.AdminNote,
            CreatedAt = doc.CreatedAt
        };
    }
}
