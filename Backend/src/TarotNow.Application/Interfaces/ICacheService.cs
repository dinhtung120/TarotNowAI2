

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface ICacheService
{
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default);

        Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
}
