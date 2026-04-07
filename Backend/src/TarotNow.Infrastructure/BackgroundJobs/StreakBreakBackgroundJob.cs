using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.BackgroundJobs;

public class StreakBreakBackgroundJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StreakBreakBackgroundJob> _logger;

    public StreakBreakBackgroundJob(IServiceProvider serviceProvider, ILogger<StreakBreakBackgroundJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[StreakBreakBackgroundJob] Rình rập khởi động...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBreakingStreaksAsync(stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[StreakBreakBackgroundJob] Ngã ngựa lúc xử tử Steak.");
                }

                
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[StreakBreakBackgroundJob] Đang dừng công việc đồ tể thầm lặng...");
        }

        _logger.LogInformation("[StreakBreakBackgroundJob] Đã dừng.");
    }

    private async Task ProcessBreakingStreaksAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        
        
        var lazyUsersIds = await dbContext.Users
            .Where(u => u.CurrentStreak > 0 && u.LastStreakDate.HasValue && u.LastStreakDate.Value < yesterday)
            .Select(u => u.Id)
            .ToListAsync(stoppingToken);

        if (!lazyUsersIds.Any())
            return;

        _logger.LogInformation("[StreakBreakBackgroundJob] Tiết mục chặt chuỗi của {Count} người lười.", lazyUsersIds.Count);

        
        int processed = 0;
        foreach (var userId in lazyUsersIds)
        {
            
            if (processed++ % 100 == 0)
            {
                await Task.Delay(100, stoppingToken); 
            }

            var user = await dbContext.Users.FindAsync(new object[] { userId }, stoppingToken);
            if (user != null && user.CurrentStreak > 0 && user.LastStreakDate.HasValue && user.LastStreakDate.Value < yesterday)
            {
                
                user.BreakStreak();
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }

        _logger.LogInformation("[StreakBreakBackgroundJob] Xong nhiệm vụ đồ tể.");
    }
}
