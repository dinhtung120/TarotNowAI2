using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation cho refresh token rotation/revoke với distributed lock qua Redis cache service.
/// </summary>
public sealed partial class MongoRefreshTokenRepository : IRefreshTokenRepository
{
    private const int MaxLockContentionAttempts = 5;
    private static readonly BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private readonly IMongoCollection<RefreshTokenDocument> _refreshTokenCollection;
    private readonly ICacheService _cacheService;
    private readonly AuthSecurityOptions _authSecurityOptions;
    private readonly CacheBackendState _cacheBackendState;
    private readonly ILogger<MongoRefreshTokenRepository> _logger;

    public MongoRefreshTokenRepository(
        MongoDbContext mongoDbContext,
        ICacheService cacheService,
        IOptions<AuthSecurityOptions> authSecurityOptions,
        CacheBackendState cacheBackendState,
        ILogger<MongoRefreshTokenRepository> logger)
    {
        _refreshTokenCollection = mongoDbContext.RefreshTokens;
        _cacheService = cacheService;
        _authSecurityOptions = authSecurityOptions.Value;
        _cacheBackendState = cacheBackendState;
        _logger = logger;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var tokenHash = RefreshToken.HashToken(token.Trim());
        var document = await FindByTokenHashAsync(tokenHash, cancellationToken);
        return document is null ? null : ToEntity(document);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _refreshTokenCollection.InsertOneAsync(ToDocument(refreshToken), cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var document = ToDocument(refreshToken);
        var result = await _refreshTokenCollection.ReplaceOneAsync(
            x => x.Id == refreshToken.Id,
            document,
            cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            _logger.LogWarning("Refresh token update missed target document. TokenId={TokenId}", refreshToken.Id);
        }
    }

    private async Task<RefreshTokenDocument?> FindByTokenHashAsync(string tokenHash, CancellationToken cancellationToken)
    {
        return await _refreshTokenCollection
            .Find(x => x.TokenHash == tokenHash)
            .SortByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<RefreshToken?> TryLoadByIdAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var document = await _refreshTokenCollection.Find(x => x.Id == tokenId).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : ToEntity(document);
    }
}
