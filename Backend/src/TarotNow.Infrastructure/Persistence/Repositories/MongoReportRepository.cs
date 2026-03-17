using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository cho reports collection (MongoDB).
/// Map giữa ReportDto (Application) ↔ ReportDocument (Infrastructure).
/// </summary>
public class MongoReportRepository : IReportRepository
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

    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<ReportDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (!string.IsNullOrEmpty(statusFilter))
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));

        var totalCount = await _context.Reports.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.Reports.Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    // --- Mapping ---

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
