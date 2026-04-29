using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class MongoRefreshTokenRepository
{
    private async Task<RefreshRotateResult?> TryBuildEarlyExitResultAsync(
        RefreshToken currentToken,
        string requestNewRawToken,
        string normalizedIdempotencyKey,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        if (currentToken.IsExpired)
        {
            return RefreshRotateResult.Expired(currentToken);
        }

        if (currentToken.IsActive)
        {
            return null;
        }

        if (string.Equals(currentToken.LastRotateIdempotencyKey, normalizedIdempotencyKey, StringComparison.Ordinal))
        {
            var idempotent = await TryBuildIdempotentResultAsync(currentToken, requestNewRawToken, cancellationToken);
            return idempotent ?? RefreshRotateResult.Locked();
        }

        if (IsLockContention(currentToken, normalizedIdempotencyKey, nowUtc))
        {
            return RefreshRotateResult.Locked();
        }

        return RefreshRotateResult.ReplayDetected(currentToken);
    }

    private async Task<RefreshRotateResult?> TryBuildIdempotentResultAsync(
        RefreshToken currentToken,
        string requestNewRawToken,
        CancellationToken cancellationToken)
    {
        if (!currentToken.ReplacedByTokenId.HasValue || string.IsNullOrWhiteSpace(requestNewRawToken))
        {
            return null;
        }

        var replacement = await TryLoadByIdAsync(currentToken.ReplacedByTokenId.Value, cancellationToken);
        if (replacement is null || !replacement.MatchesToken(requestNewRawToken))
        {
            return null;
        }

        return RefreshRotateResult.Idempotent(currentToken, requestNewRawToken, replacement.ExpiresAt);
    }

    private async Task<RefreshRotateResult> HandleLockContentionAsync(
        string tokenHash,
        string newRawToken,
        string normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= MaxLockContentionAttempts; attempt++)
        {
            var snapshotDocument = await FindByTokenHashAsync(tokenHash, cancellationToken);
            if (snapshotDocument is null)
            {
                return RefreshRotateResult.Locked();
            }

            var snapshotToken = ToEntity(snapshotDocument);
            var idempotent = await TryBuildIdempotentResultAsync(snapshotToken, newRawToken, cancellationToken);
            if (idempotent is not null
                && string.Equals(snapshotToken.LastRotateIdempotencyKey, normalizedIdempotencyKey, StringComparison.Ordinal))
            {
                return idempotent;
            }

            if (!snapshotToken.IsActive)
            {
                return RefreshRotateResult.Locked();
            }

            if (attempt < MaxLockContentionAttempts)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(35 * attempt), cancellationToken);
            }
        }

        return RefreshRotateResult.Locked();
    }

    private bool IsLockContention(RefreshToken current, string idempotencyKey, DateTime nowUtc)
    {
        if (current.ReplacedByTokenId is null
            || current.UsedAtUtc is null
            || string.Equals(current.LastRotateIdempotencyKey, idempotencyKey, StringComparison.Ordinal) == false)
        {
            return false;
        }

        var contentionWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));
        var usedAtUtc = current.UsedAtUtc.Value;
        if (usedAtUtc > nowUtc)
        {
            return true;
        }

        return nowUtc - usedAtUtc <= contentionWindow;
    }
}
