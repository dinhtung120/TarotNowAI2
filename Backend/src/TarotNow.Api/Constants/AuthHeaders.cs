namespace TarotNow.Api.Constants;

/// <summary>
/// Tên HTTP headers cho auth/session flow.
/// </summary>
public static class AuthHeaders
{
    /// <summary>
    /// Header idempotency key chuẩn cho API.
    /// </summary>
    public const string IdempotencyKey = "Idempotency-Key";

    /// <summary>
    /// Header idempotency key legacy (backward compatibility tạm thời).
    /// </summary>
    public const string LegacyIdempotencyKey = "x-idempotency-key";

    /// <summary>
    /// Header định danh thiết bị phía client gửi lên.
    /// </summary>
    public const string DeviceId = "x-device-id";

    /// <summary>
    /// Header chuyển tiếp user-agent gốc từ BFF/middleware.
    /// </summary>
    public const string ForwardedUserAgent = "x-forwarded-user-agent";
}
