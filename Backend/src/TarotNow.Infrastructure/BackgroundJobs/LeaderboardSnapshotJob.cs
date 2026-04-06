using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Công Việc Chạy Ngầm Chụp Ảnh Bảng Xếp Hạng (Leaderboard Snapshot Job).
/// Chạy định kỳ để lưu trữ kết quả xếp hạng của các chu kỳ đã qua (Hôm qua, Tháng trước).
/// Giúp người dùng xem lại lịch sử vinh danh mà không tốn tài nguyên tính toán lại từ đầu.
/// </summary>
public class LeaderboardSnapshotJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LeaderboardSnapshotJob> _logger;

    public LeaderboardSnapshotJob(IServiceProvider serviceProvider, ILogger<LeaderboardSnapshotJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try 
        {
            // Chờ 1 phút sau khi khởi động app để tránh tranh chấp tài nguyên lúc startup
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    
                    // Chỉ chạy snapshot vào lúc 00:05 sáng hàng ngày (giờ UTC)
                    if (now.Hour == 0 && now.Minute >= 5 && now.Minute <= 15)
                    {
                        await PerformSnapshotsAsync(stoppingToken);
                        // Sau khi chạy xong, ngủ 1 tiếng để tránh chạy lại trong cùng khung giờ
                        await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                    }
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Shutdown phase
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Lỗi khi thực hiện Leaderboard Snapshot Job.");
                }

                // Kiểm tra mỗi phút
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("LeaderboardSnapshotJob đang dừng do yêu cầu hủy bỏ.");
        }

        _logger.LogInformation("LeaderboardSnapshotJob đã dừng.");
    }

    private async Task PerformSnapshotsAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var lbRepo = scope.ServiceProvider.GetRequiredService<ILeaderboardRepository>();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        _logger.LogInformation("Bắt đầu tiến trình chụp ảnh Bảng xếp hạng...");

        // 1. Snapshot Daily (cho ngày hôm qua)
        var yesterday = DateTime.UtcNow.AddDays(-1);
        string dailyKey = yesterday.ToString("yyyy-MM-dd");
        await CreateSnapshotAsync(lbRepo, userRepo, "daily_rank_score", dailyKey, ct);

        // 2. Snapshot Monthly (nếu hôm nay là ngày đầu tháng, snapshot cho tháng trước)
        if (DateTime.UtcNow.Day == 1)
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            string monthlyKey = lastMonth.ToString("yyyy-MM");
            await CreateSnapshotAsync(lbRepo, userRepo, "monthly_rank_score", monthlyKey, ct);
        }

        _logger.LogInformation("Hoàn tất chụp ảnh Bảng xếp hạng.");
    }

    private async Task CreateSnapshotAsync(
        ILeaderboardRepository lbRepo, 
        IUserRepository userRepo, 
        string track, 
        string periodKey, 
        CancellationToken ct)
    {
        // Lấy top 100 entries live
        var entries = await lbRepo.GetTopEntriesAsync(track, periodKey, 100, ct);
        if (entries.Count == 0) return;

        // Enrich thông tin user (Avatar, DisplayName) để lưu vào Snapshot vĩnh viễn
        var userIds = entries.ConvertAll(e => Guid.Parse(e.UserId));
        var userMap = await userRepo.GetUserBasicInfoMapAsync(userIds, ct);

        foreach (var e in entries)
        {
            if (userMap.TryGetValue(Guid.Parse(e.UserId), out var info))
            {
                e.DisplayName = info.DisplayName;
                e.Avatar = info.AvatarUrl;
                e.ActiveTitle = info.ActiveTitle;
            }
        }

        // Lưu vào MongoDB collection leaderboard_snapshots
        await lbRepo.UpsertSnapshotAsync(new Application.Features.Gamification.Dtos.LeaderboardSnapshotDto
        {
            ScoreTrack = track,
            PeriodKey = periodKey,
            TotalParticipants = entries.Count,
            Entries = entries,
            CreatedAt = DateTime.UtcNow
        }, ct);

        _logger.LogInformation("Đã lưu Snapshot cho {Track} - {Period}", track, periodKey);
    }
}
