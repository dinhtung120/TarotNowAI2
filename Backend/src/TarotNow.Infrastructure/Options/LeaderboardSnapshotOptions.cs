namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Options vận hành job snapshot leaderboard.
/// </summary>
public sealed class LeaderboardSnapshotOptions
{
    /// <summary>
    /// Delay khởi động job sau khi app start (giây).
    /// </summary>
    public int StartupDelaySeconds { get; set; } = 60;

    /// <summary>
    /// Giờ UTC chạy snapshot hằng ngày.
    /// </summary>
    public int DailyWindowHourUtc { get; set; } = 0;

    /// <summary>
    /// Phút bắt đầu cửa sổ chạy snapshot hằng ngày (UTC).
    /// </summary>
    public int DailyWindowStartMinuteUtc { get; set; } = 5;

    /// <summary>
    /// Phút kết thúc cửa sổ chạy snapshot hằng ngày (UTC).
    /// </summary>
    public int DailyWindowEndMinuteUtc { get; set; } = 15;

    /// <summary>
    /// Delay sau khi snapshot thành công để tránh chạy lặp (phút).
    /// </summary>
    public int PostSnapshotSleepMinutes { get; set; } = 60;

    /// <summary>
    /// Chu kỳ loop kiểm tra điều kiện snapshot (giây).
    /// </summary>
    public int LoopIntervalSeconds { get; set; } = 60;

    /// <summary>
    /// Số lượng top entry lưu vào snapshot.
    /// </summary>
    public int TopEntries { get; set; } = 100;
}
