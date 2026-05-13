

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Hosted service ghi log backend cache lúc khởi động để vận hành biết hệ thống đang dùng Redis hay fallback memory.
public sealed class CacheBackendStartupLogger : IHostedService
{
    private readonly CacheBackendState _cacheBackendState;
    private readonly ILogger<CacheBackendStartupLogger> _logger;

    /// <summary>
    /// Khởi tạo logger trạng thái cache backend bằng state đã detect và logger hạ tầng.
    /// Luồng xử lý: nhận dependency qua DI để dùng trong StartAsync.
    /// </summary>
    public CacheBackendStartupLogger(
        CacheBackendState cacheBackendState,
        ILogger<CacheBackendStartupLogger> logger)
    {
        _cacheBackendState = cacheBackendState;
        _logger = logger;
    }

    /// <summary>
    /// Ghi log backend cache tại thời điểm service start để cảnh báo mức độ nhất quán của rate limit/quota.
    /// Luồng xử lý: kiểm tra cờ UsesRedis, log debug khi Redis sẵn sàng hoặc warning khi fallback memory.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_cacheBackendState.BootstrapSettingsFailureType is not null)
        {
            _logger.LogWarning(
                "[RedisBootstrap] Unable to load bootstrap tuning values from PostgreSQL. Using static fallback values. ReasonType={ReasonType}",
                _cacheBackendState.BootstrapSettingsFailureType);
        }

        if (_cacheBackendState.RedisInitializationFailureType is not null)
        {
            _logger.LogWarning(
                "[RedisBootstrap] Failed to initialize Redis multiplexer. Endpoints={RedisEndpoints}. FallingBackTo={CacheBackend}. ReasonType={ReasonType}",
                _cacheBackendState.RedisEndpointSummary ?? "unknown",
                "DistributedMemoryCache",
                _cacheBackendState.RedisInitializationFailureType);
        }

        if (_cacheBackendState.UsesRedis)
        {
            // Nhánh chuẩn: Redis khả dụng nên cache phân tán hoạt động đầy đủ.
            _logger.LogInformation("Cache backend initialized with Redis.");
            return Task.CompletedTask;
        }

        // Edge case quan trọng: fallback memory làm giảm tính nhất quán trong môi trường nhiều instance.
        _logger.LogWarning("Redis unavailable at startup. Falling back to in-memory cache; distributed rate limiting/quota consistency is reduced.");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Dừng hosted service logger.
    /// Luồng xử lý: không giữ tài nguyên nên trả CompletedTask ngay.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
