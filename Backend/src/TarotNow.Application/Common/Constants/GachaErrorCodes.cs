namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Mã lỗi nghiệp vụ cho module gacha.
/// </summary>
public static class GachaErrorCodes
{
    /// <summary>
    /// Pool không tồn tại hoặc không active.
    /// </summary>
    public const string PoolNotFound = "GACHA_POOL_NOT_FOUND";

    /// <summary>
    /// Cấu hình reward rate không hợp lệ.
    /// </summary>
    public const string InvalidPoolConfiguration = "GACHA_INVALID_POOL_CONFIGURATION";

    /// <summary>
    /// Idempotency key thiếu hoặc không hợp lệ.
    /// </summary>
    public const string InvalidIdempotencyKey = "GACHA_INVALID_IDEMPOTENCY_KEY";

    /// <summary>
    /// Không đủ số dư để pull.
    /// </summary>
    public const string InsufficientBalance = "GACHA_INSUFFICIENT_BALANCE";

    /// <summary>
    /// Không thể resolve reward.
    /// </summary>
    public const string RewardResolutionFailed = "GACHA_REWARD_RESOLUTION_FAILED";
}
