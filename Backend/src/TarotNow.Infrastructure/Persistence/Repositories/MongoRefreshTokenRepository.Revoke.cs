using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class MongoRefreshTokenRepository
{
    public async Task RevokeFamilyAsync(Guid familyId, string reason, CancellationToken cancellationToken = default)
    {
        if (familyId == Guid.Empty)
        {
            return;
        }

        var filter = Builders<RefreshTokenDocument>.Filter.And(
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.FamilyId, familyId),
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.RevokedAtUtc, null));
        await RevokeByFilterAsync(filter, reason, cancellationToken);
    }

    public async Task RevokeSessionAsync(Guid sessionId, string reason, CancellationToken cancellationToken = default)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        var filter = Builders<RefreshTokenDocument>.Filter.And(
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.SessionId, sessionId),
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.RevokedAtUtc, null));
        await RevokeByFilterAsync(filter, reason, cancellationToken);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RefreshTokenDocument>.Filter.And(
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<RefreshTokenDocument>.Filter.Eq(x => x.RevokedAtUtc, null));
        await RevokeByFilterAsync(filter, RefreshRevocationReasons.ManualRevoke, cancellationToken);
    }

    public async Task<int> CleanupRevokedOrExpiredBeforeAsync(
        DateTime cutoffUtc,
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0)
        {
            return 0;
        }

        var filter = Builders<RefreshTokenDocument>.Filter.Or(
            Builders<RefreshTokenDocument>.Filter.And(
                Builders<RefreshTokenDocument>.Filter.Ne(x => x.RevokedAtUtc, null),
                Builders<RefreshTokenDocument>.Filter.Lte(x => x.RevokedAtUtc, cutoffUtc)),
            Builders<RefreshTokenDocument>.Filter.And(
                Builders<RefreshTokenDocument>.Filter.Eq(x => x.RevokedAtUtc, null),
                Builders<RefreshTokenDocument>.Filter.Lte(x => x.ExpiresAtUtc, cutoffUtc)));

        var candidateIds = await _refreshTokenCollection
            .Find(filter)
            .SortBy(x => x.ExpiresAtUtc)
            .Limit(batchSize)
            .Project(x => x.Id)
            .ToListAsync(cancellationToken);
        if (candidateIds.Count == 0)
        {
            return 0;
        }

        var deleteResult = await _refreshTokenCollection.DeleteManyAsync(
            Builders<RefreshTokenDocument>.Filter.In(x => x.Id, candidateIds),
            cancellationToken);
        return (int)deleteResult.DeletedCount;
    }

    private async Task RevokeByFilterAsync(
        FilterDefinition<RefreshTokenDocument> filter,
        string reason,
        CancellationToken cancellationToken)
    {
        var documents = await _refreshTokenCollection.Find(filter).ToListAsync(cancellationToken);
        if (documents.Count == 0)
        {
            return;
        }

        foreach (var document in documents)
        {
            var token = ToEntity(document);
            token.Revoke(reason);
            await UpdateAsync(token, cancellationToken);
        }
    }
}
