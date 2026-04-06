using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Seeds;

namespace TarotNow.Infrastructure.BackgroundJobs;

public class GachaSeedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GachaSeedService> _logger;

    public GachaSeedService(IServiceProvider serviceProvider, ILogger<GachaSeedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

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
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
