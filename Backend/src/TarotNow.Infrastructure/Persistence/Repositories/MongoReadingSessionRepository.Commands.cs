using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý lệnh tạo/cập nhật reading session.
public partial class MongoReadingSessionRepository
{
    /// <summary>
    /// Tạo mới reading session trên Mongo.
    /// Luồng xử lý: map entity domain sang document, bổ sung drawn cards/cost khi có dữ liệu rồi insert.
    /// </summary>
    public async Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var doc = new ReadingSessionDocument
        {
            Id = ObjectId.TryParse(session.Id, out var oid) ? oid : session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            AiStatus = session.IsCompleted
                ? (string.IsNullOrWhiteSpace(session.AiSummary) ? "revealed" : "completed")
                : "pending",
            CreatedAt = session.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        if (session.IsCompleted)
        {
            doc.DrawnCards = ParseDrawnCards(session.CardsDrawn);
            // Session completed cần lưu bộ bài rút để hỗ trợ lịch sử xem lại.
        }

        if (session.CurrencyUsed != null && session.AmountCharged > 0)
        {
            doc.Cost = new SessionCost
            {
                Currency = session.CurrencyUsed,
                Amount = session.AmountCharged
            };
            // Chỉ lưu cost khi có giao dịch thực tế để tránh noise dữ liệu miễn phí.
        }

        await _mongoContext.ReadingSessions.InsertOneAsync(doc, cancellationToken: cancellationToken);
        return session;
    }

    /// <summary>
    /// Cập nhật reading session đã tồn tại.
    /// Luồng xử lý: dựng update động theo trạng thái hoàn tất, ai summary và followups rồi update theo id.
    /// </summary>
    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var update = Builders<ReadingSessionDocument>.Update.Set(r => r.UpdatedAt, DateTime.UtcNow);

        if (session.IsCompleted)
        {
            update = update
                .Set(r => r.DrawnCards, ParseDrawnCards(session.CardsDrawn))
                .Set(r => r.AiStatus, string.IsNullOrWhiteSpace(session.AiSummary) ? "revealed" : "completed");
            // Khi complete phải đồng bộ cả dữ liệu bài rút và trạng thái AI.
        }

        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            update = update.Set(r => r.AiResult, new AiResult { Summary = session.AiSummary });
            // Chỉ set AiResult khi summary thực sự có nội dung.
        }

        if (session.Followups != null && session.Followups.Any())
        {
            await ApplyFollowupsWithOptimisticConcurrencyAsync(session, update, cancellationToken);
            return;
        }

        await _mongoContext.ReadingSessions.UpdateOneAsync(
            BuildIdFilter(session.Id),
            update,
            cancellationToken: cancellationToken);
    }

    private async Task ApplyFollowupsWithOptimisticConcurrencyAsync(
        ReadingSession session,
        UpdateDefinition<ReadingSessionDocument> baseUpdate,
        CancellationToken cancellationToken)
    {
        var mappedFollowups = MapFollowups(session);
        const int maxRetries = 3;
        for (var attempt = 0; attempt < maxRetries; attempt++)
        {
            var snapshot = await GetConcurrencySnapshotAsync(session.Id, cancellationToken);
            if (snapshot is null) return;

            var persisted = await TryPersistFollowupDeltaAsync(
                new FollowupPersistenceRequest(
                    session.Id,
                    snapshot,
                    mappedFollowups,
                    baseUpdate,
                    UseVersionFilter: true),
                cancellationToken);
            if (persisted) return;

            await Task.Delay(BuildRetryDelay(attempt), cancellationToken);
        }

        var finalSnapshot = await GetConcurrencySnapshotAsync(session.Id, cancellationToken);
        if (finalSnapshot is null) return;

        var finalPersisted = await TryPersistFollowupDeltaAsync(
            new FollowupPersistenceRequest(
                session.Id,
                finalSnapshot,
                mappedFollowups,
                baseUpdate,
                UseVersionFilter: false),
            cancellationToken);
        if (finalPersisted) return;

        throw new InvalidOperationException(
            $"Failed to persist follow-up delta for reading session {session.Id} after optimistic retries.");
    }

    private static List<FollowupEntry> MapFollowups(ReadingSession session)
    {
        return session.Followups.Select((followup, index) => new FollowupEntry
        {
            Sequence = index + 1,
            Question = followup.Question,
            Answer = followup.Answer,
            AiRequestId = followup.AiRequestId
        }).ToList();
    }

    private async Task<ReadingSessionConcurrencySnapshot?> GetConcurrencySnapshotAsync(
        string sessionId,
        CancellationToken cancellationToken)
    {
        return await _mongoContext.ReadingSessions
            .Find(BuildIdFilter(sessionId))
            .Project(r => new ReadingSessionConcurrencySnapshot
            {
                UpdatedAt = r.UpdatedAt,
                FollowupsCount = r.Followups.Count
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<bool> TryPersistFollowupDeltaAsync(
        FollowupPersistenceRequest request,
        CancellationToken cancellationToken)
    {
        var deltaFollowups = request.MappedFollowups.Skip(request.Snapshot.FollowupsCount).ToList();
        var update = ComposeFollowupUpdate(deltaFollowups, request.BaseUpdate);
        var filter = BuildUpdateFilter(request.SessionId, request.Snapshot, request.UseVersionFilter);
        var result = await _mongoContext.ReadingSessions.UpdateOneAsync(
            filter,
            update,
            cancellationToken: cancellationToken);
        if (result.ModifiedCount > 0)
        {
            return true;
        }

        // No-op hợp lệ: trạng thái follow-up đã được writer khác ghi trước đó.
        if (deltaFollowups.Count == 0 && result.MatchedCount > 0)
        {
            return true;
        }

        return false;
    }

    private static UpdateDefinition<ReadingSessionDocument> ComposeFollowupUpdate(
        IReadOnlyList<FollowupEntry> deltaFollowups,
        UpdateDefinition<ReadingSessionDocument> baseUpdate)
    {
        return deltaFollowups.Count == 0
            ? baseUpdate
            : baseUpdate.PushEach(r => r.Followups, deltaFollowups);
    }

    private static FilterDefinition<ReadingSessionDocument> BuildUpdateFilter(
        string sessionId,
        ReadingSessionConcurrencySnapshot snapshot,
        bool useVersionFilter)
    {
        var filterById = BuildIdFilter(sessionId);
        return useVersionFilter
            ? Builders<ReadingSessionDocument>.Filter.And(
                filterById,
                Builders<ReadingSessionDocument>.Filter.Eq(r => r.UpdatedAt, snapshot.UpdatedAt))
            : filterById;
    }

    private static TimeSpan BuildRetryDelay(int attempt)
        => TimeSpan.FromMilliseconds(20 * (attempt + 1));

    private sealed class ReadingSessionConcurrencySnapshot
    {
        public DateTime UpdatedAt { get; set; }
        public int FollowupsCount { get; set; }
    }

    private readonly record struct FollowupPersistenceRequest(
        string SessionId,
        ReadingSessionConcurrencySnapshot Snapshot,
        IReadOnlyList<FollowupEntry> MappedFollowups,
        UpdateDefinition<ReadingSessionDocument> BaseUpdate,
        bool UseVersionFilter);
}
