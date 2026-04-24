using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Job nền chụp snapshot leaderboard định kỳ để lưu lại bảng xếp hạng đã chốt theo kỳ.
public class LeaderboardSnapshotJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LeaderboardSnapshotJob> _logger;
    private readonly LeaderboardSnapshotOptions _options;

    /// <summary>
    /// Khởi tạo job snapshot leaderboard.
    /// Luồng xử lý: nhận service provider để resolve repository theo scope và logger theo dõi tiến trình.
    /// </summary>
    public LeaderboardSnapshotJob(
        IServiceProvider serviceProvider,
        ILogger<LeaderboardSnapshotJob> logger,
        IOptions<LeaderboardSnapshotOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Vòng lặp chạy nền chờ khung giờ snapshot.
    /// Luồng xử lý: delay warm-up, kiểm tra cửa sổ thời gian đầu ngày, chạy snapshot và ngủ theo lịch.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(ResolveStartupDelay(), stoppingToken);
            // Delay ngắn sau startup để giảm cạnh tranh tài nguyên lúc hệ thống vừa khởi động.

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;

                    if (IsWithinDailyWindow(now))
                    {
                        await PerformSnapshotsAsync(stoppingToken);
                        // Sau khi snapshot thành công thì ngủ dài để tránh chạy lặp nhiều lần cùng ngày.
                        await Task.Delay(ResolvePostSnapshotSleep(), stoppingToken);
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

                await Task.Delay(ResolveLoopInterval(), stoppingToken);
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
        var entries = await lbRepo.GetTopEntriesAsync(track, periodKey, ResolveTopEntries(), ct);
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

    private bool IsWithinDailyWindow(DateTime nowUtc)
    {
        var hour = Math.Clamp(_options.DailyWindowHourUtc, 0, 23);
        var startMinute = Math.Clamp(_options.DailyWindowStartMinuteUtc, 0, 59);
        var endMinute = Math.Clamp(_options.DailyWindowEndMinuteUtc, 0, 59);
        if (endMinute < startMinute)
        {
            endMinute = startMinute;
        }

        return nowUtc.Hour == hour && nowUtc.Minute >= startMinute && nowUtc.Minute <= endMinute;
    }

    private TimeSpan ResolveStartupDelay()
    {
        var seconds = _options.StartupDelaySeconds <= 0 ? 60 : _options.StartupDelaySeconds;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, 1, 3600));
    }

    private TimeSpan ResolveLoopInterval()
    {
        var seconds = _options.LoopIntervalSeconds <= 0 ? 60 : _options.LoopIntervalSeconds;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, 5, 3600));
    }

    private TimeSpan ResolvePostSnapshotSleep()
    {
        var minutes = _options.PostSnapshotSleepMinutes <= 0 ? 60 : _options.PostSnapshotSleepMinutes;
        return TimeSpan.FromMinutes(Math.Clamp(minutes, 1, 240));
    }

    private int ResolveTopEntries()
    {
        return Math.Clamp(_options.TopEntries, 1, 1000);
    }
}
