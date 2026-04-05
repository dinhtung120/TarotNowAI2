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
public partial class MongoReportRepository : IReportRepository
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

    public async Task<ReportDto?> GetByIdAsync(string reportId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReportDocument>.Filter.And(
            Builders<ReportDocument>.Filter.Eq(x => x.Id, reportId),
            Builders<ReportDocument>.Filter.Eq(x => x.IsDeleted, false));

        var doc = await _context.Reports.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Phân trang cho Admin báo cáo queue.
    /// Hỗ trợ filter theo status: "pending" (chờ xử lý), "processing", "resolved", "rejected".
    /// Sắp xếp: mới nhất trước — Admin cần xem báo cáo gần đây ưu tiên.
    /// </summary>
    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null, string? targetType = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReportDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        // Filter theo status nếu có
        if (!string.IsNullOrEmpty(statusFilter))
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));

        if (!string.IsNullOrWhiteSpace(targetType))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Target.Type, targetType));
        }

        var totalCount = await _context.Reports.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.Reports.Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

}
