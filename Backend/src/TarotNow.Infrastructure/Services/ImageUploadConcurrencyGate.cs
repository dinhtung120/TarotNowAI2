using System.Threading;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Giới hạn số upload song song để tránh spike CPU/IO khi nén + PUT R2.
/// </summary>
public sealed class ImageUploadConcurrencyGate
{
    private readonly SemaphoreSlim _semaphore;

    public ImageUploadConcurrencyGate(int maxConcurrent)
    {
        var n = Math.Clamp(maxConcurrent, 1, 64);
        _semaphore = new SemaphoreSlim(n, n);
    }

    public Task WaitAsync(CancellationToken cancellationToken) => _semaphore.WaitAsync(cancellationToken);

    public void Release() => _semaphore.Release();
}
