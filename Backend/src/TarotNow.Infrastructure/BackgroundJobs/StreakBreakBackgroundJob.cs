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

// Job nền cắt streak người dùng khi quá hạn hoạt động theo quy tắc ngày nghiệp vụ.
public class StreakBreakBackgroundJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StreakBreakBackgroundJob> _logger;

    /// <summary>
    /// Khởi tạo job xử lý break streak.
    /// Luồng xử lý: nhận service provider để tạo scope DB theo từng vòng chạy và logger vận hành.
    /// </summary>
    public StreakBreakBackgroundJob(IServiceProvider serviceProvider, ILogger<StreakBreakBackgroundJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp nền quét người dùng cần break streak theo chu kỳ 1 giờ.
    /// Luồng xử lý: gọi ProcessBreakingStreaksAsync, bắt lỗi cục bộ và delay định kỳ.
    /// </summary>
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
                    // Bỏ qua lỗi dispose khi host đang shutdown.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[StreakBreakBackgroundJob] Ngã ngựa lúc xử tử Steak.");
                    // Bắt lỗi để job không dừng hẳn sau một lần quét lỗi.
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

    /// <summary>
    /// Quét và break streak cho người dùng không hoạt động quá hạn.
    /// Luồng xử lý: truy vấn user cần break, xử lý theo batch nhỏ có throttle, gọi user.BreakStreak và save.
    /// </summary>
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
        {
            // Không có user quá hạn streak thì bỏ qua vòng xử lý hiện tại.
            return;
        }

        _logger.LogInformation("[StreakBreakBackgroundJob] Tiết mục chặt chuỗi của {Count} người lười.", lazyUsersIds.Count);

        const int chunkSize = 250;
        for (var offset = 0; offset < lazyUsersIds.Count; offset += chunkSize)
        {
            var chunkUserIds = lazyUsersIds.Skip(offset).Take(chunkSize).ToArray();
            var users = await dbContext.Users
                .Where(user => chunkUserIds.Contains(user.Id))
                .ToListAsync(stoppingToken);

            foreach (var user in users)
            {
                if (user.CurrentStreak > 0 && user.LastStreakDate.HasValue && user.LastStreakDate.Value < yesterday)
                {
                    user.BreakStreak();
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            if (offset + chunkSize < lazyUsersIds.Count)
            {
                await Task.Delay(50, stoppingToken);
                // Nhịp nghỉ ngắn giữa các chunk để giảm áp lực ghi DB liên tục.
            }
        }

        _logger.LogInformation("[StreakBreakBackgroundJob] Xong nhiệm vụ đồ tể.");
    }
}
