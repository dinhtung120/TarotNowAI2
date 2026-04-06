using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Seeds;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.BackgroundJobs;

public class GamificationSeedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GamificationSeedService> _logger;

    public GamificationSeedService(IServiceProvider serviceProvider, ILogger<GamificationSeedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

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
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
