using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
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

    private readonly record struct RotateTransactionContext(
        RefreshRotateRequest Request,
        string NormalizedToken,
        string TokenHash,
        string NormalizedIdempotencyKey,
        string TokenIdempotencyCacheKey);

    private readonly record struct RotateExecutionContext(
        string LockKey,
        string LockOwner,
        string TokenIdempotencyCacheKey,
        RotateTransactionContext TransactionContext);
}
