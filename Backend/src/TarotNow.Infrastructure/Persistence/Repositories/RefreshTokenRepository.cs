using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý refresh token với cơ chế rotation atomic + idempotency.
/// </summary>
public sealed partial class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly AuthSecurityOptions _authSecurityOptions;
    private readonly CacheBackendState _cacheBackendState;
    private readonly ILogger<RefreshTokenRepository> _logger;

    /// <summary>
    /// Khởi tạo refresh token repository.
    /// </summary>
    public RefreshTokenRepository(
        ApplicationDbContext dbContext,
        ICacheService cacheService,
        IOptions<AuthSecurityOptions> authSecurityOptions,
        CacheBackendState cacheBackendState,
        ILogger<RefreshTokenRepository> logger)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
        _authSecurityOptions = authSecurityOptions.Value;
        _cacheBackendState = cacheBackendState;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var normalizedToken = token.Trim();
        var hashedToken = RefreshToken.HashToken(normalizedToken);
        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken || rt.Token == normalizedToken, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

}
