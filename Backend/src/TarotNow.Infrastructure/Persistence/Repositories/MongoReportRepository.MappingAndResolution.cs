using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReportRepository
{
    public async Task<bool> ResolveAsync(
        string reportId,
        string status,
        string result,
        string resolvedBy,
        string? adminNote,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReportDocument>.Filter.And(
            Builders<ReportDocument>.Filter.Eq(x => x.Id, reportId),
            Builders<ReportDocument>.Filter.Eq(x => x.IsDeleted, false));

        var update = Builders<ReportDocument>.Update
            .Set(x => x.Status, status)
            .Set(x => x.Result, result)
            .Set(x => x.ResolvedBy, resolvedBy)
            .Set(x => x.ResolvedAt, DateTime.UtcNow)
            .Set(x => x.AdminNote, adminNote)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var outcome = await _context.Reports.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return outcome.ModifiedCount > 0;
    }

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
