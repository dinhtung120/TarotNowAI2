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
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AuthSessionCleanupJob> _logger;
    private readonly AuthSecurityOptions _options;

    /// <summary>
    /// Khởi tạo cleanup worker cho auth/session.
    /// </summary>
    public AuthSessionCleanupJob(
        IServiceProvider serviceProvider,
        IOptions<AuthSecurityOptions> options,
        ILogger<AuthSessionCleanupJob> logger)
    {
        _serviceProvider = serviceProvider;
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
        try
        {
            await ExecuteCleanupWithDistributedLockAsync(cancellationToken);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "[AuthSessionCleanupJob] Lỗi khi chạy cleanup cycle.");
        }
    }

    private async Task ExecuteCleanupWithDistributedLockAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var lockOwner = Guid.NewGuid().ToString("N");
        if (!await TryAcquireCleanupLockAsync(cacheService, lockOwner, cancellationToken))
        {
            return;
        }

        try
        {
            await ExecuteCleanupBatchLoopAsync(scope.ServiceProvider, cancellationToken);
        }
        finally
        {
            await TryReleaseCleanupLockAsync(cacheService, lockOwner, cancellationToken);
        }
    }

    private async Task ExecuteCleanupBatchLoopAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var authSessionRepository = serviceProvider.GetRequiredService<IAuthSessionRepository>();
        var refreshTokenRepository = serviceProvider.GetRequiredService<IRefreshTokenRepository>();
        var context = BuildCleanupContext();
        var maxBatchLoopsPerCycle = ResolveMaxBatchLoopsPerCycle();

        var totalRefreshDeleted = 0;
        var totalSessionsDeleted = 0;
        for (var loop = 0; loop < maxBatchLoopsPerCycle; loop++)
        {
            var deletedRefresh = await refreshTokenRepository.CleanupRevokedOrExpiredBeforeAsync(
                context.RefreshCutoffUtc,
                context.BatchSize,
                cancellationToken);
            var deletedSessions = await authSessionRepository.CleanupRevokedBeforeAsync(
                context.SessionCutoffUtc,
                context.BatchSize,
                cancellationToken);

            totalRefreshDeleted += deletedRefresh;
            totalSessionsDeleted += deletedSessions;

            if (deletedRefresh < context.BatchSize && deletedSessions < context.BatchSize)
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
                context.RefreshCutoffUtc,
                context.SessionCutoffUtc);
        }
    }

    private CleanupContext BuildCleanupContext()
    {
        var nowUtc = DateTime.UtcNow;
        var refreshRetentionDays = Math.Max(1, _options.RefreshTokenRetentionDays);
        var revokedSessionRetentionDays = Math.Max(1, _options.RevokedSessionRetentionDays);
        return new CleanupContext(
            RefreshCutoffUtc: nowUtc.AddDays(-refreshRetentionDays),
            SessionCutoffUtc: nowUtc.AddDays(-revokedSessionRetentionDays),
            BatchSize: ResolveBatchSize());
    }

    private async Task<bool> TryAcquireCleanupLockAsync(
        ICacheService cacheService,
        string lockOwner,
        CancellationToken cancellationToken)
    {
        return await cacheService.AcquireLockAsync(
            AuthCacheKeys.AuthCleanupLockKey,
            lockOwner,
            ResolveLockLease(),
            cancellationToken);
    }

    private async Task TryReleaseCleanupLockAsync(
        ICacheService cacheService,
        string lockOwner,
        CancellationToken cancellationToken)
    {
        try
        {
            await cacheService.ReleaseLockAsync(AuthCacheKeys.AuthCleanupLockKey, lockOwner, cancellationToken);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "[AuthSessionCleanupJob] Không release được cleanup lock.");
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

    private int ResolveMaxBatchLoopsPerCycle()
    {
        return Math.Clamp(_options.CleanupMaxBatchLoopsPerCycle, 1, 100);
    }

    private readonly record struct CleanupContext(DateTime RefreshCutoffUtc, DateTime SessionCutoffUtc, int BatchSize);
}
