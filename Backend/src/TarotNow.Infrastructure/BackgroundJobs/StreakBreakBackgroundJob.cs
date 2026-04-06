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

/// <summary>
/// Thằng đao phủ thầm lặng cầm dao chặt những cánh tay Streak lười biếng.
/// Quét định kỳ để ép gãy streak với những người dùng nhắp nhả không vào rút bài,
/// nhằm kịp thời hiện trạng thái vỡ cho UI để gạ bán Streak Freeze.
/// </summary>
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

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBreakingStreaksAsync(stoppingToken);

                // Quét 1 giờ 1 lần. Job này không cần phải realtime khắt khe từng giây. 
                // AI quay vào rút bài muộn cũng tự bị gãy do service.
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[StreakBreakBackgroundJob] Ngã ngựa lúc xử tử Steak.");
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }

    private async Task ProcessBreakingStreaksAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        // Những ông nhõi nào đang có chuỗi (CurrentStreak > 0)
        // Mà có LastStreakDate nhỏ hơn Hôm Qua thì tức là đã trễ nhịp 1 nhát nguyên ngày. Đứt!
        var lazyUsersIds = await dbContext.Users
            .Where(u => u.CurrentStreak > 0 && u.LastStreakDate.HasValue && u.LastStreakDate.Value < yesterday)
            .Select(u => u.Id)
            .ToListAsync(stoppingToken);

        if (!lazyUsersIds.Any())
            return;

        _logger.LogInformation("[StreakBreakBackgroundJob] Tiết mục chặt chuỗi của {Count} người lười.", lazyUsersIds.Count);

        // Băm nhuyễn batch SQL hoặc foreach update tuỳ độ to của App. Đang làm foreach cho gọn.
        int processed = 0;
        foreach (var userId in lazyUsersIds)
        {
            // Tránh quá tải CPU DB nên cho nghỉ tý (Nhường đường cho Chat)
            if (processed++ % 100 == 0)
            {
                await Task.Delay(100, stoppingToken); 
            }

            var user = await dbContext.Users.FindAsync(new object[] { userId }, stoppingToken);
            if (user != null && user.CurrentStreak > 0 && user.LastStreakDate.HasValue && user.LastStreakDate.Value < yesterday)
            {
                // Hành hình
                user.BreakStreak();
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }

        _logger.LogInformation("[StreakBreakBackgroundJob] Xong nhiệm vụ đồ tể.");
    }
}
