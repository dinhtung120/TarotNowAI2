using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý auth session theo user/device.
/// </summary>
public sealed class AuthSessionRepository : IAuthSessionRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo auth session repository.
    /// </summary>
    public AuthSessionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<AuthSession> CreateAsync(
        Guid userId,
        string deviceId,
        string userAgentHash,
        string ipHash,
        CancellationToken cancellationToken = default)
    {
        var normalizedDeviceId = NormalizeDeviceId(deviceId);
        var existing = await _dbContext.AuthSessions
            .FirstOrDefaultAsync(
                x => x.UserId == userId
                     && x.RevokedAtUtc == null
                     && x.DeviceId == normalizedDeviceId,
                cancellationToken);
        if (existing is not null)
        {
            existing.Touch(ipHash, DateTime.UtcNow);
            _dbContext.AuthSessions.Update(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }

        var session = new AuthSession(userId, deviceId, userAgentHash, ipHash);
        try
        {
            await _dbContext.AuthSessions.AddAsync(session, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return session;
        }
        catch (DbUpdateException)
        {
            var concurrent = await _dbContext.AuthSessions
                .FirstOrDefaultAsync(
                    x => x.UserId == userId
                         && x.RevokedAtUtc == null
                         && x.DeviceId == normalizedDeviceId,
                    cancellationToken);
            if (concurrent is null)
            {
                throw;
            }

            concurrent.Touch(ipHash, DateTime.UtcNow);
            _dbContext.AuthSessions.Update(concurrent);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return concurrent;
        }
    }

    /// <inheritdoc />
    public async Task<AuthSession?> GetActiveAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AuthSessions
            .FirstOrDefaultAsync(x => x.Id == sessionId && x.RevokedAtUtc == null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RevokeAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _dbContext.AuthSessions.FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);
        if (session is null || session.RevokedAtUtc is not null)
        {
            return;
        }

        session.Revoke(DateTime.UtcNow);
        _dbContext.AuthSessions.Update(session);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _dbContext.AuthSessions
            .Where(x => x.UserId == userId && x.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        if (sessions.Count == 0)
        {
            return;
        }

        var now = DateTime.UtcNow;
        foreach (var session in sessions)
        {
            session.Revoke(now);
        }

        _dbContext.AuthSessions.UpdateRange(sessions);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Guid>> GetActiveSessionIdsByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.AuthSessions
            .Where(x => x.UserId == userId && x.RevokedAtUtc == null)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    private static string NormalizeDeviceId(string deviceId)
    {
        var fallback = "unknown";
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return fallback;
        }

        var trimmed = deviceId.Trim();
        return trimmed.Length <= 128 ? trimmed : trimmed[..128];
    }
}
