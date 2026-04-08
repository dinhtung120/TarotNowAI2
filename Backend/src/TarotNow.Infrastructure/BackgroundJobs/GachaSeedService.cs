using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Seeds;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Hosted service seed dữ liệu gacha khi khởi động ứng dụng.
public class GachaSeedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GachaSeedService> _logger;

    /// <summary>
    /// Khởi tạo service seed gacha.
    /// Luồng xử lý: nhận service provider để tạo scope seed và logger theo dõi tiến trình.
    /// </summary>
    public GachaSeedService(IServiceProvider serviceProvider, ILogger<GachaSeedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Chạy seed dữ liệu gacha khi host start.
    /// Luồng xử lý: tạo scope, lấy ApplicationDbContext, gọi GachaSeed.SeedAsync và log kết quả.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GachaSeed] Bắt đầu kiểm tra và khởi tạo dữ liệu mẫu Gacha...");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await GachaSeed.SeedAsync(context);
            _logger.LogInformation("[GachaSeed] Hoàn tất khởi tạo dữ liệu Gacha.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GachaSeed] Lỗi khi khởi tạo dữ liệu mẫu Gacha.");
            // Bắt lỗi để không làm fail toàn bộ tiến trình startup vì lỗi seed.
        }
    }

    /// <summary>
    /// Dừng service seed gacha.
    /// Luồng xử lý: không giữ tài nguyên chạy nền nên trả CompletedTask.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
