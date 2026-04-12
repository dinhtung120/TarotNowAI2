using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Quét malware cho payload nhị phân; implementation mặc định no-op.
/// </summary>
public interface IVirusScanService
{
    Task EnsureCleanAsync(byte[] data, CancellationToken cancellationToken = default);
}
