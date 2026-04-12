using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Mặc định không quét; có thể thay bằng ClamAV hoặc dịch vụ cloud khi tích hợp.
/// </summary>
public sealed class NoOpVirusScanService : IVirusScanService
{
    public Task EnsureCleanAsync(byte[] data, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
