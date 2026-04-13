using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Job nền dọn object R2 bị orphan hoặc session upload hết hạn chưa consume.
public sealed class MediaUploadCleanupJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MediaUploadCleanupJob> _logger;
    private readonly ObjectStorageOptions _options;

    /// <summary>
    /// Khởi tạo cleanup worker cho media upload.
    /// </summary>
    public MediaUploadCleanupJob(
        IServiceProvider serviceProvider,
        IOptions<ObjectStorageOptions> options,
        ILogger<MediaUploadCleanupJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[MediaUploadCleanupJob] Khởi động vòng dọn media upload.");

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
            _logger.LogInformation("[MediaUploadCleanupJob] Đã nhận tín hiệu dừng.");
        }
    }

    private async Task RunCleanupCycleAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var r2UploadService = scope.ServiceProvider.GetRequiredService<IR2UploadService>();
            if (r2UploadService.IsEnabled == false)
            {
                _logger.LogWarning("[MediaUploadCleanupJob] Bỏ qua cleanup vì R2 chưa cấu hình.");
                return;
            }

            await CleanupExpiredUploadSessionsAsync(scope.ServiceProvider, r2UploadService, cancellationToken);
            await CleanupCommunityAssetsAsync(scope.ServiceProvider, r2UploadService, cancellationToken);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "[MediaUploadCleanupJob] Lỗi khi chạy cleanup cycle.");
        }
    }

    private async Task CleanupExpiredUploadSessionsAsync(
        IServiceProvider serviceProvider,
        IR2UploadService r2UploadService,
        CancellationToken cancellationToken)
    {
        var sessionRepository = serviceProvider.GetRequiredService<IUploadSessionRepository>();
        var sessions = await sessionRepository.GetExpiredUnconsumedAsync(
            DateTime.UtcNow,
            _options.CleanupBatchSize,
            cancellationToken);

        foreach (var session in sessions)
        {
            var deleted = await TryDeleteObjectAsync(r2UploadService, session.ObjectKey, cancellationToken);
            if (deleted)
            {
                await sessionRepository.MarkCleanedAsync(session.UploadToken, DateTime.UtcNow, cancellationToken);
            }
        }
    }

    private async Task CleanupCommunityAssetsAsync(
        IServiceProvider serviceProvider,
        IR2UploadService r2UploadService,
        CancellationToken cancellationToken)
    {
        var assetRepository = serviceProvider.GetRequiredService<ICommunityMediaAssetRepository>();
        var assets = await assetRepository.GetCleanupCandidatesAsync(
            DateTime.UtcNow,
            _options.CleanupBatchSize,
            cancellationToken);

        foreach (var asset in assets)
        {
            var deleted = await TryDeleteObjectAsync(r2UploadService, asset.ObjectKey, cancellationToken);
            if (deleted)
            {
                await assetRepository.MarkDeletedAsync(asset.ObjectKey, DateTime.UtcNow, cancellationToken);
            }
        }
    }

    private async Task<bool> TryDeleteObjectAsync(
        IR2UploadService r2UploadService,
        string objectKey,
        CancellationToken cancellationToken)
    {
        try
        {
            await r2UploadService.DeleteObjectAsync(objectKey, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[MediaUploadCleanupJob] Không xóa được object key={ObjectKey}", objectKey);
            return false;
        }
    }

    private TimeSpan GetCleanupInterval()
    {
        var minutes = _options.CleanupIntervalMinutes <= 0 ? 10 : _options.CleanupIntervalMinutes;
        return TimeSpan.FromMinutes(minutes);
    }
}
