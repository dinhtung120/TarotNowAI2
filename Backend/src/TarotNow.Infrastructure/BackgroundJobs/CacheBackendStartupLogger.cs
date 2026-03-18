using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed class CacheBackendStartupLogger : IHostedService
{
    private readonly CacheBackendState _cacheBackendState;
    private readonly ILogger<CacheBackendStartupLogger> _logger;

    public CacheBackendStartupLogger(
        CacheBackendState cacheBackendState,
        ILogger<CacheBackendStartupLogger> logger)
    {
        _cacheBackendState = cacheBackendState;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_cacheBackendState.UsesRedis)
        {
            _logger.LogInformation("Cache backend initialized with Redis.");
        }
        else
        {
            _logger.LogWarning("Redis unavailable at startup. Falling back to in-memory cache; distributed rate limiting/quota consistency is reduced.");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
