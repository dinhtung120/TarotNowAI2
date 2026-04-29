using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureAuthIndexes()
    {
        SafeCreateIndex(RefreshTokens, new CreateIndexModel<RefreshTokenDocument>(
            Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.TokenHash),
            new CreateIndexOptions { Unique = true, Name = "idx_refresh_token_hash_unique" }));

        SafeCreateIndex(RefreshTokens, new CreateIndexModel<RefreshTokenDocument>(
            Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.UserId).Descending(x => x.CreatedAtUtc),
            new CreateIndexOptions { Name = "idx_refresh_user_created_desc" }));

        SafeCreateIndex(RefreshTokens, new CreateIndexModel<RefreshTokenDocument>(
            Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.SessionId),
            new CreateIndexOptions { Name = "idx_refresh_session_id" }));

        SafeCreateIndex(RefreshTokens, new CreateIndexModel<RefreshTokenDocument>(
            Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.FamilyId).Ascending(x => x.ParentTokenId),
            new CreateIndexOptions { Name = "idx_refresh_family_parent" }));

        SafeCreateIndex(RefreshTokens, new CreateIndexModel<RefreshTokenDocument>(
            Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.ExpiresAtUtc),
            new CreateIndexOptions { Name = "idx_refresh_expires_at" }));
    }
}
