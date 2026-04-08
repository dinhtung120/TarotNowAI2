

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính quản lý report trên MongoDB.
public partial class MongoReportRepository : IReportRepository
{
    // Mongo context truy cập collection reports.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository report.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu report.
    /// </summary>
    public MongoReportRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tạo mới report.
    /// Luồng xử lý: map DTO sang document, insert Mongo và trả id phát sinh ngược về DTO.
    /// </summary>
    public async Task AddAsync(ReportDto report, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(report);
        await _context.Reports.InsertOneAsync(doc, cancellationToken: cancellationToken);
        report.Id = doc.Id;
    }

    /// <summary>
    /// Lấy report theo id nếu chưa bị xóa mềm.
    /// Luồng xử lý: filter id + is_deleted=false rồi map DTO.
    /// </summary>
    public async Task<ReportDto?> GetByIdAsync(string reportId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReportDocument>.Filter.And(
            Builders<ReportDocument>.Filter.Eq(x => x.Id, reportId),
            Builders<ReportDocument>.Filter.Eq(x => x.IsDeleted, false));

        var doc = await _context.Reports.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy danh sách report có phân trang và bộ lọc.
    /// Luồng xử lý: chuẩn hóa page/pageSize, filter is_deleted=false + status/target tùy chọn, rồi trả trang mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null, string? targetType = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Giới hạn page size để giữ hiệu năng dashboard moderation.

        var filterBuilder = Builders<ReportDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));
            // Lọc theo trạng thái xử lý khi admin yêu cầu.
        }

        if (!string.IsNullOrWhiteSpace(targetType))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Target.Type, targetType));
            // Hỗ trợ phân loại report theo loại đối tượng bị tố cáo.
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
