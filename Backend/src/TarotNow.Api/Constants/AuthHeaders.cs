namespace TarotNow.Api.Constants;

/// <summary>
/// Tên HTTP headers cho auth/session flow.
/// </summary>
public static class AuthHeaders
{
    /// <summary>
    /// Header idempotency key cho refresh token.
    /// </summary>
    public const string IdempotencyKey = "x-idempotency-key";

    /// <summary>
    /// Header định danh thiết bị phía client gửi lên.
    /// </summary>
    public const string DeviceId = "x-device-id";
}
