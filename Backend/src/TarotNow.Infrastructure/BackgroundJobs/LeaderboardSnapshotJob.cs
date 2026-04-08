using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Job nền chụp snapshot leaderboard định kỳ để lưu lại bảng xếp hạng đã chốt theo kỳ.
public class LeaderboardSnapshotJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LeaderboardSnapshotJob> _logger;

    /// <summary>
    /// Khởi tạo job snapshot leaderboard.
    /// Luồng xử lý: nhận service provider để resolve repository theo scope và logger theo dõi tiến trình.
    /// </summary>
    public LeaderboardSnapshotJob(IServiceProvider serviceProvider, ILogger<LeaderboardSnapshotJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp chạy nền chờ khung giờ snapshot.
    /// Luồng xử lý: delay warm-up, kiểm tra cửa sổ thời gian đầu ngày, chạy snapshot và ngủ theo lịch.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            // Delay ngắn sau startup để giảm cạnh tranh tài nguyên lúc hệ thống vừa khởi động.

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;

                    if (now.Hour == 0 && now.Minute >= 5 && now.Minute <= 15)
                    {
                        await PerformSnapshotsAsync(stoppingToken);
                        // Sau khi snapshot thành công thì ngủ dài để tránh chạy lặp nhiều lần cùng ngày.
                        await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                    }
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Bỏ qua lỗi dispose khi job đang dừng có chủ đích.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Lỗi khi thực hiện Leaderboard Snapshot Job.");
                    // Bắt lỗi cục bộ để vòng lặp job vẫn tiếp tục.
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("LeaderboardSnapshotJob đang dừng do yêu cầu hủy bỏ.");
        }

        _logger.LogInformation("LeaderboardSnapshotJob đã dừng.");
    }

    /// <summary>
    /// Thực hiện snapshot daily và monthly (nếu tới ngày đầu tháng).
    /// Luồng xử lý: resolve repository, snapshot daily của hôm qua, snapshot monthly của tháng trước vào ngày mùng 1.
    /// </summary>
    private async Task PerformSnapshotsAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var lbRepo = scope.ServiceProvider.GetRequiredService<ILeaderboardRepository>();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        _logger.LogInformation("Bắt đầu tiến trình chụp ảnh Bảng xếp hạng...");

        var yesterday = DateTime.UtcNow.AddDays(-1);
        string dailyKey = yesterday.ToString("yyyy-MM-dd");
        await CreateSnapshotAsync(lbRepo, userRepo, "daily_rank_score", dailyKey, ct);
        // Snapshot daily luôn lấy kỳ của hôm qua để đảm bảo dữ liệu ngày đã chốt ổn định.

        if (DateTime.UtcNow.Day == 1)
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            string monthlyKey = lastMonth.ToString("yyyy-MM");
            await CreateSnapshotAsync(lbRepo, userRepo, "monthly_rank_score", monthlyKey, ct);
            // Snapshot monthly chỉ chạy ngày đầu tháng cho kỳ tháng trước.
        }

        _logger.LogInformation("Hoàn tất chụp ảnh Bảng xếp hạng.");
    }

    /// <summary>
    /// Tạo snapshot cho một track và period cụ thể.
    /// Luồng xử lý: lấy top entries, enrich thông tin user, upsert snapshot rồi log kết quả.
    /// </summary>
    private async Task CreateSnapshotAsync(
        ILeaderboardRepository lbRepo,
        IUserRepository userRepo,
        string track,
        string periodKey,
        CancellationToken ct)
    {
        var entries = await lbRepo.GetTopEntriesAsync(track, periodKey, 100, ct);
        if (entries.Count == 0)
        {
            // Không có dữ liệu thì bỏ qua snapshot để tránh tạo bản ghi rỗng.
            return;
        }

        var userIds = entries.ConvertAll(e => Guid.Parse(e.UserId));
        var userMap = await userRepo.GetUserBasicInfoMapAsync(userIds, ct);

        foreach (var e in entries)
        {
            if (userMap.TryGetValue(Guid.Parse(e.UserId), out var info))
            {
                e.DisplayName = info.DisplayName;
                e.Avatar = info.AvatarUrl;
                e.ActiveTitle = info.ActiveTitle;
                // Bổ sung metadata hiển thị vào entry để snapshot dùng trực tiếp trên UI.
            }
        }

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
