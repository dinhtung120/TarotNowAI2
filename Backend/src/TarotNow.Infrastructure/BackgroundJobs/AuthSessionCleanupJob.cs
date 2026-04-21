using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Job nền dọn auth_sessions và refresh_tokens đã hết vòng đời giữ lại.
/// </summary>
public sealed class AuthSessionCleanupJob : BackgroundService
{
    private const int MaxBatchLoopsPerCycle = 10;

    private readonly IServiceProvider _serviceProvider;
    private readonly ICacheService _cacheService;
    private readonly ILogger<AuthSessionCleanupJob> _logger;
    private readonly AuthSecurityOptions _options;

    /// <summary>
    /// Khởi tạo cleanup worker cho auth/session.
    /// </summary>
    public AuthSessionCleanupJob(
        IServiceProvider serviceProvider,
        ICacheService cacheService,
        IOptions<AuthSecurityOptions> options,
        ILogger<AuthSessionCleanupJob> logger)
    {
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[AuthSessionCleanupJob] Khởi động vòng dọn dữ liệu auth session/refresh token.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunCleanupCycleAsync(stoppingToken);
                await Task.Delay(GetCleanupInterval(), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[AuthSessionCleanupJob] Đã nhận tín hiệu dừng.");
        }
    }

    private async Task RunCleanupCycleAsync(CancellationToken cancellationToken)
    {
        var lockOwner = Guid.NewGuid().ToString("N");
        var lockLease = ResolveLockLease();
        var lockAcquired = false;

        try
        {
            lockAcquired = await _cacheService.AcquireLockAsync(
                AuthCacheKeys.AuthCleanupLockKey,
                lockOwner,
                lockLease,
                cancellationToken);
            if (!lockAcquired)
            {
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var authSessionRepository = scope.ServiceProvider.GetRequiredService<IAuthSessionRepository>();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

            var nowUtc = DateTime.UtcNow;
            var refreshRetentionDays = Math.Max(1, _options.RefreshTokenRetentionDays);
            var revokedSessionRetentionDays = Math.Max(1, _options.RevokedSessionRetentionDays);
            var refreshCutoffUtc = nowUtc.AddDays(-refreshRetentionDays);
            var sessionCutoffUtc = nowUtc.AddDays(-revokedSessionRetentionDays);
            var batchSize = ResolveBatchSize();

            var totalRefreshDeleted = 0;
            var totalSessionsDeleted = 0;
            for (var loop = 0; loop < MaxBatchLoopsPerCycle; loop++)
            {
                var deletedRefresh = await refreshTokenRepository.CleanupRevokedOrExpiredBeforeAsync(
                    refreshCutoffUtc,
                    batchSize,
                    cancellationToken);
                var deletedSessions = await authSessionRepository.CleanupRevokedBeforeAsync(
                    sessionCutoffUtc,
                    batchSize,
                    cancellationToken);

                totalRefreshDeleted += deletedRefresh;
                totalSessionsDeleted += deletedSessions;

                if (deletedRefresh < batchSize && deletedSessions < batchSize)
                {
                    break;
                }
            }

            if (totalRefreshDeleted > 0 || totalSessionsDeleted > 0)
            {
                _logger.LogInformation(
                    "[AuthSessionCleanupJob] Cleanup hoàn tất. DeletedRefreshTokens={DeletedRefreshTokens} DeletedAuthSessions={DeletedAuthSessions} RefreshCutoffUtc={RefreshCutoffUtc:o} SessionCutoffUtc={SessionCutoffUtc:o}",
                    totalRefreshDeleted,
                    totalSessionsDeleted,
                    refreshCutoffUtc,
                    sessionCutoffUtc);
            }
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "[AuthSessionCleanupJob] Lỗi khi chạy cleanup cycle.");
        }
        finally
        {
            if (lockAcquired)
            {
                try
                {
                    await _cacheService.ReleaseLockAsync(AuthCacheKeys.AuthCleanupLockKey, lockOwner, cancellationToken);
                }
                catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning(ex, "[AuthSessionCleanupJob] Không release được cleanup lock.");
                }
            }
        }
    }

    private TimeSpan ResolveLockLease()
    {
        var interval = GetCleanupInterval();
        var leaseSeconds = Math.Max(30, (int)Math.Ceiling(interval.TotalSeconds * 0.8));
        return TimeSpan.FromSeconds(Math.Min(1800, leaseSeconds));
    }

    private int ResolveBatchSize()
    {
        return Math.Clamp(_options.CleanupBatchSize, 50, 5000);
    }

    private TimeSpan GetCleanupInterval()
    {
        var minutes = _options.CleanupIntervalMinutes <= 0 ? 30 : _options.CleanupIntervalMinutes;
        return TimeSpan.FromMinutes(minutes);
    }
}
