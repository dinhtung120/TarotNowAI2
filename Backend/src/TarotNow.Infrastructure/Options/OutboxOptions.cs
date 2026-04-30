namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Options vận hành outbox processor.
/// </summary>
public sealed class OutboxOptions
{
    /// <summary>
    /// Số lượng message tối đa claim mỗi vòng.
    /// </summary>
    public int BatchSize { get; set; } = 50;

    /// <summary>
    /// Số lần retry tối đa trước khi chuyển dead-letter.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 12;

    /// <summary>
    /// Timeout lock processing (giây) để reclaim stale worker.
    /// </summary>
    public int LockTimeoutSeconds { get; set; } = 120;

    /// <summary>
    /// Backoff tối đa giữa các lần retry (giây).
    /// </summary>
    public int MaxBackoffSeconds { get; set; } = 300;

    /// <summary>
    /// Chu kỳ quét outbox worker (giây).
    /// </summary>
    public int PollIntervalSeconds { get; set; } = 1;
}
