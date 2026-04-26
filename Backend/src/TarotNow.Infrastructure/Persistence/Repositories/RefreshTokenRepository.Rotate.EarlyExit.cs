using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
    private async Task<RefreshRotateResult?> TryBuildEarlyExitResultAsync(
        RefreshToken current,
        RotateTransactionContext context,
        CancellationToken cancellationToken)
    {
        if (current.IsExpired)
        {
            return RefreshRotateResult.Expired(current);
        }

        if (current.IsActive)
        {
            return null;
        }

        var idempotencyKey = context.NormalizedIdempotencyKey;
        if (string.Equals(current.LastRotateIdempotencyKey, idempotencyKey, StringComparison.Ordinal))
        {
            var idempotentResult = await TryBuildIdempotentResultAsync(
                current,
                context.Request.NewRawToken,
                cancellationToken);
            // Nếu không thể chứng minh cùng refresh token mới thì fail-closed để tránh trả token sai.
            return idempotentResult ?? RefreshRotateResult.Locked();
        }

        if (IsLockContention(current, idempotencyKey, DateTime.UtcNow))
        {
            return RefreshRotateResult.Locked();
        }

        return RefreshRotateResult.ReplayDetected(current);
    }

    private async Task<RefreshRotateResult?> TryBuildIdempotentResultAsync(
        RefreshToken current,
        string requestNewRawToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requestNewRawToken))
        {
            return null;
        }

        var replacementToken = await TryLoadReplacementTokenAsync(current.ReplacedByTokenId, cancellationToken);
        if (replacementToken is not null && replacementToken.MatchesToken(requestNewRawToken))
        {
            return RefreshRotateResult.Idempotent(current, requestNewRawToken, replacementToken.ExpiresAt);
        }
        return null;
    }

    private async Task<RefreshToken?> TryLoadReplacementTokenAsync(Guid? replacementTokenId, CancellationToken cancellationToken)
    {
        if (!replacementTokenId.HasValue || replacementTokenId.Value == Guid.Empty)
        {
            return null;
        }

        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Id == replacementTokenId.Value, cancellationToken);
    }
}
