namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Options bảo mật riêng cho auth/session rotation.
/// </summary>
public sealed class AuthSecurityOptions
{
    /// <summary>
    /// Thời gian giữ lock refresh token để tránh race condition.
    /// </summary>
    public int RefreshLockSeconds { get; set; } = 15;

    /// <summary>
    /// Cửa sổ idempotency cho refresh token.
    /// </summary>
    public int RefreshIdempotencyWindowSeconds { get; set; } = 60;

    /// <summary>
    /// TTL cho deny-list access token theo jti khi logout/replay.
    /// </summary>
    public int AccessTokenBlacklistTtlSeconds { get; set; } = 1200;

    /// <summary>
    /// TTL cho cờ session bị revoke để chặn access token cũ theo sid.
    /// </summary>
    public int SessionRevocationTtlSeconds { get; set; } = 1800;

    /// <summary>
    /// TTL cache snapshot session active (Redis).
    /// </summary>
    public int SessionCacheTtlSeconds { get; set; } = 30 * 24 * 60 * 60;

    /// <summary>
    /// TTL lưu security record khi phát hiện refresh replay.
    /// </summary>
    public int ReplaySecurityRecordTtlSeconds { get; set; } = 24 * 60 * 60;

    /// <summary>
    /// Batch size cho mỗi lần dọn auth session/refresh token.
    /// </summary>
    public int CleanupBatchSize { get; set; } = 200;

    /// <summary>
    /// Số vòng lặp batch tối đa trong mỗi chu kỳ cleanup.
    /// </summary>
    public int CleanupMaxBatchLoopsPerCycle { get; set; } = 10;

    /// <summary>
    /// Chu kỳ chạy auth cleanup job (phút).
    /// </summary>
    public int CleanupIntervalMinutes { get; set; } = 30;

    /// <summary>
    /// Số ngày giữ lại refresh token đã revoke/hết hạn trước khi cleanup.
    /// </summary>
    public int RefreshTokenRetentionDays { get; set; } = 30;

    /// <summary>
    /// Số ngày giữ lại auth session đã revoke trước khi cleanup.
    /// </summary>
    public int RevokedSessionRetentionDays { get; set; } = 30;

    /// <summary>
    /// Bật fail-closed cho refresh rotation khi Redis không sẵn sàng.
    /// </summary>
    public bool RequireRedisForRefreshConsistency { get; set; } = true;
}
