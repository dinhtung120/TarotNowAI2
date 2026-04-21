using Microsoft.EntityFrameworkCore.Storage;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
    private static bool IsValidRotateRequest(RefreshRotateRequest request)
    {
        return string.IsNullOrWhiteSpace(request.RawToken) == false
            && string.IsNullOrWhiteSpace(request.NewRawToken) == false;
    }

    private async Task<bool> TryAcquireLockAsync(string lockKey, string lockOwner, CancellationToken cancellationToken)
    {
        var lockWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));
        return await _cacheService.AcquireLockAsync(lockKey, lockOwner, lockWindow, cancellationToken);
    }

    private static async Task<RefreshRotateResult> FinalizeResultAsync(
        IDbContextTransaction? localTransaction,
        RefreshRotateResult result,
        CancellationToken cancellationToken)
    {
        if (localTransaction is not null)
        {
            await localTransaction.CommitAsync(cancellationToken);
        }

        return result;
    }
}
