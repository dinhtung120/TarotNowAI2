using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
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

    /// <inheritdoc />
    public async Task<bool> ResolvePostReportWithPostMutationAsync(
        PostReportResolutionMutation mutation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(mutation);

        using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();

        try
        {
            var resolved = await TryResolvePostReportAsync(session, mutation, cancellationToken);
            if (!resolved)
            {
                await session.AbortTransactionAsync(cancellationToken);
                return false;
            }

            if (mutation.RemovePost)
            {
                var postUpdated = await TrySoftDeletePostAsync(session, mutation, cancellationToken);
                if (!postUpdated)
                {
                    await session.AbortTransactionAsync(cancellationToken);
                    return false;
                }
            }

            await session.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch
        {
            if (session.IsInTransaction)
            {
                await session.AbortTransactionAsync(cancellationToken);
            }

            throw;
        }
    }

    private async Task<bool> TryResolvePostReportAsync(
        IClientSessionHandle session,
        PostReportResolutionMutation mutation,
        CancellationToken cancellationToken)
    {
        var reportFilter = Builders<ReportDocument>.Filter.And(
            Builders<ReportDocument>.Filter.Eq(x => x.Id, mutation.ReportId),
            Builders<ReportDocument>.Filter.Eq(x => x.IsDeleted, false),
            Builders<ReportDocument>.Filter.Eq(x => x.Target.Type, "post"));
        var reportUpdate = Builders<ReportDocument>.Update
            .Set(x => x.Status, mutation.Status)
            .Set(x => x.Result, mutation.Result)
            .Set(x => x.ResolvedBy, mutation.ResolvedBy)
            .Set(x => x.ResolvedAt, DateTime.UtcNow)
            .Set(x => x.AdminNote, mutation.AdminNote)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var reportOutcome = await _context.Reports.UpdateOneAsync(
            session,
            reportFilter,
            reportUpdate,
            cancellationToken: cancellationToken);
        return reportOutcome.ModifiedCount > 0;
    }

    private async Task<bool> TrySoftDeletePostAsync(
        IClientSessionHandle session,
        PostReportResolutionMutation mutation,
        CancellationToken cancellationToken)
    {
        var postFilter = Builders<CommunityPostDocument>.Filter.And(
            Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, mutation.PostId),
            Builders<CommunityPostDocument>.Filter.Eq(x => x.IsDeleted, false));
        var postUpdate = Builders<CommunityPostDocument>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedAt, DateTime.UtcNow)
            .Set(x => x.DeletedBy, mutation.DeletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var postOutcome = await _context.CommunityPosts.UpdateOneAsync(
            session,
            postFilter,
            postUpdate,
            cancellationToken: cancellationToken);
        return postOutcome.ModifiedCount > 0;
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
