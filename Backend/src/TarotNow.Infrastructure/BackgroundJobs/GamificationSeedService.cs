using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Seeds;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Hosted service seed dữ liệu gamification lúc khởi động.
public class GamificationSeedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GamificationSeedService> _logger;

    /// <summary>
    /// Khởi tạo service seed gamification.
    /// Luồng xử lý: nhận service provider để tạo scope seed và logger vận hành.
    /// </summary>
    public GamificationSeedService(IServiceProvider serviceProvider, ILogger<GamificationSeedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Chạy seed dữ liệu gamification khi host start.
    /// Luồng xử lý: tạo scope, lấy MongoDbContext, gọi GamificationSeed.SeedAsync và log kết quả.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GamificationSeed] Bắt đầu kiểm tra và khởi tạo dữ liệu mẫu...");
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
            await GamificationSeed.SeedAsync(context);
            _logger.LogInformation("[GamificationSeed] Hoàn tất khởi tạo dữ liệu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GamificationSeed] Lỗi khi khởi tạo dữ liệu mẫu.");
            // Bắt lỗi seed để startup không bị fail cứng do dữ liệu mẫu.
        }
    }

    /// <summary>
    /// Dừng service seed gamification.
    /// Luồng xử lý: service không có vòng lặp nền nên trả CompletedTask.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
