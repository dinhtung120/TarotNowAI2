using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý resolve report và mapping DTO/document.
public partial class MongoReportRepository
{
    /// <summary>
    /// Resolve một report.
    /// Luồng xử lý: chỉ update report chưa xóa mềm, set trạng thái/kết quả/metadata xử lý và trả true khi có thay đổi.
    /// </summary>
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
        // ModifiedCount phản ánh chính xác report có được resolve thành công hay không.
    }

    /// <summary>
    /// Map ReportDto sang document Mongo.
    /// Luồng xử lý: chuẩn hóa id và gom target type/id vào nested object ReportTarget.
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
    /// Map ReportDocument sang DTO.
    /// Luồng xử lý: tách dữ liệu target nested về các field phẳng trong DTO.
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
