

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var normalizedToken = token.Trim();
        
        var hashedToken = RefreshToken.HashToken(normalizedToken);

        return await _dbContext.RefreshTokens
            .Include(rt => rt.User) 
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken || rt.Token == normalizedToken, cancellationToken);
    }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

        public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

        public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        
        var activeTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        
        foreach (var token in activeTokens)
        {
            token.Revoke();
        }

        
        if (activeTokens.Any())
        {
            _dbContext.RefreshTokens.UpdateRange(activeTokens);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
