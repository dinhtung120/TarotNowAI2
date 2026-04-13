using MongoDB.Driver;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository lưu session upload token one-time trên MongoDB.
/// </summary>
public sealed class UploadSessionRepository : IUploadSessionRepository
{
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository upload sessions.
    /// </summary>
    public UploadSessionRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task CreateAsync(UploadSessionRecord session, CancellationToken cancellationToken = default)
    {
        var document = ToDocument(session);
        return _context.UploadSessions.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<UploadSessionRecord?> GetByTokenAsync(string uploadToken, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UploadSessionDocument>.Filter.Eq(x => x.UploadToken, uploadToken);
        var document = await _context.UploadSessions.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : ToRecord(document);
    }

    /// <inheritdoc />
    public async Task<bool> ConsumeAsync(string uploadToken, DateTime consumedAtUtc, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UploadSessionDocument>.Filter.Eq(x => x.UploadToken, uploadToken)
                     & Builders<UploadSessionDocument>.Filter.Eq(x => x.ConsumedAtUtc, null)
                     & Builders<UploadSessionDocument>.Filter.Gt(x => x.ExpiresAtUtc, consumedAtUtc);

        var update = Builders<UploadSessionDocument>.Update.Set(x => x.ConsumedAtUtc, consumedAtUtc);
        var result = await _context.UploadSessions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UploadSessionRecord>> GetExpiredUnconsumedAsync(
        DateTime nowUtc,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 100 : Math.Min(limit, 1000);
        var filter = Builders<UploadSessionDocument>.Filter.Eq(x => x.ConsumedAtUtc, null)
                     & Builders<UploadSessionDocument>.Filter.Eq(x => x.CleanedUpAtUtc, null)
                     & Builders<UploadSessionDocument>.Filter.Lte(x => x.ExpiresAtUtc, nowUtc);

        var documents = await _context.UploadSessions.Find(filter)
            .SortBy(x => x.ExpiresAtUtc)
            .Limit(normalizedLimit)
            .ToListAsync(cancellationToken);

        return documents.Select(ToRecord).ToList();
    }

    /// <inheritdoc />
    public async Task<bool> MarkCleanedAsync(string uploadToken, DateTime cleanedAtUtc, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UploadSessionDocument>.Filter.Eq(x => x.UploadToken, uploadToken)
                     & Builders<UploadSessionDocument>.Filter.Eq(x => x.CleanedUpAtUtc, null);

        var update = Builders<UploadSessionDocument>.Update.Set(x => x.CleanedUpAtUtc, cleanedAtUtc);
        var result = await _context.UploadSessions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    private static UploadSessionDocument ToDocument(UploadSessionRecord session)
    {
        return new UploadSessionDocument
        {
            UploadToken = session.UploadToken,
            OwnerUserId = session.OwnerUserId.ToString(),
            Scope = session.Scope,
            ObjectKey = session.ObjectKey,
            PublicUrl = session.PublicUrl,
            ContentType = session.ContentType,
            SizeBytes = session.SizeBytes,
            ConversationId = session.ConversationId,
            ContextType = session.ContextType,
            ContextDraftId = session.ContextDraftId,
            CreatedAtUtc = session.CreatedAtUtc,
            ExpiresAtUtc = session.ExpiresAtUtc,
            ConsumedAtUtc = session.ConsumedAtUtc,
            CleanedUpAtUtc = session.CleanedUpAtUtc,
        };
    }

    private static UploadSessionRecord ToRecord(UploadSessionDocument document)
    {
        return new UploadSessionRecord
        {
            UploadToken = document.UploadToken,
            OwnerUserId = Guid.TryParse(document.OwnerUserId, out var ownerId) ? ownerId : Guid.Empty,
            Scope = document.Scope,
            ObjectKey = document.ObjectKey,
            PublicUrl = document.PublicUrl,
            ContentType = document.ContentType,
            SizeBytes = document.SizeBytes,
            ConversationId = document.ConversationId,
            ContextType = document.ContextType,
            ContextDraftId = document.ContextDraftId,
            CreatedAtUtc = document.CreatedAtUtc,
            ExpiresAtUtc = document.ExpiresAtUtc,
            ConsumedAtUtc = document.ConsumedAtUtc,
            CleanedUpAtUtc = document.CleanedUpAtUtc,
        };
    }
}
