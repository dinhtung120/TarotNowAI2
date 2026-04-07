

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReportRepository : IReportRepository
{
    private readonly MongoDbContext _context;

    public MongoReportRepository(MongoDbContext context)
    {
        _context = context;
    }

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

        public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null, string? targetType = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReportDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        
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
