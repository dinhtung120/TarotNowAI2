using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Api.Constants;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Resolve IP client từ RemoteIpAddress đã được chuẩn hóa ở tầng ForwardedHeaders.
    /// Luồng xử lý: không đọc trực tiếp X-Forwarded-For để tránh spoof header bypass limiter.
    /// </summary>
    private static string ResolveClientIp(HttpContext httpContext)
    {
        // Edge case thiếu IP: dùng khóa "unknown" để vẫn partition được limiter.
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    /// <summary>
    /// Resolve partition key theo user đã xác thực, fallback về IP nếu chưa đăng nhập.
    /// Luồng xử lý: đọc claim NameIdentifier, trả khóa user khi có; ngược lại dùng khóa theo IP.
    /// </summary>
    private static string ResolveAuthenticatedPartitionKey(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Ưu tiên key theo user để quota gắn đúng chủ thể nghiệp vụ.
            return $"user:{userId}";
        }

        // Nhánh anonymous: fallback theo IP để vẫn chặn được traffic bất thường trước bước auth.
        return $"ip:{ResolveClientIp(httpContext)}";
    }

    /// <summary>
    /// Resolve partition key cho refresh theo thứ tự ưu tiên:
    /// refresh token fingerprint -> user claim -> device -> IP.
    /// </summary>
    private static string ResolveRefreshPartitionKey(
        HttpContext httpContext,
        int refreshDeviceLength,
        int refreshTokenLength)
    {
        if (TryResolveRefreshTokenPartition(httpContext, refreshTokenLength, out var refreshTokenPartition))
        {
            return refreshTokenPartition;
        }

        var authenticatedPartition = ResolveAuthenticatedPartitionOrDefault(httpContext);
        if (!string.IsNullOrWhiteSpace(authenticatedPartition))
        {
            return authenticatedPartition;
        }

        var deviceId = httpContext.Request.Headers[AuthHeaders.DeviceId].ToString();
        if (string.IsNullOrWhiteSpace(deviceId)
            && httpContext.Request.Cookies.TryGetValue(AuthCookieNames.DeviceId, out var deviceCookie))
        {
            deviceId = deviceCookie;
        }

        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            return $"refresh-device:{HashPrefix(deviceId, refreshDeviceLength)}";
        }

        return $"ip:{ResolveClientIp(httpContext)}";
    }

    /// <summary>
    /// Resolve partition key theo token-family proxy (token fingerprint trước, fallback user/device/ip).
    /// </summary>
    private static string ResolveRefreshFamilyKey(
        HttpContext httpContext,
        int refreshDeviceLength,
        int refreshTokenLength)
    {
        if (TryResolveRefreshTokenPartition(httpContext, refreshTokenLength, out var refreshTokenPartition))
        {
            return $"refresh-family:{refreshTokenPartition}";
        }

        var authenticatedPartition = ResolveAuthenticatedPartitionOrDefault(httpContext);
        if (!string.IsNullOrWhiteSpace(authenticatedPartition))
        {
            return $"refresh-family:{authenticatedPartition}";
        }

        var deviceId = httpContext.Request.Headers[AuthHeaders.DeviceId].ToString();
        if (string.IsNullOrWhiteSpace(deviceId)
            && httpContext.Request.Cookies.TryGetValue(AuthCookieNames.DeviceId, out var deviceCookie))
        {
            deviceId = deviceCookie;
        }

        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            return $"refresh-family:device:{HashPrefix(deviceId, refreshDeviceLength)}";
        }

        return $"refresh-family:ip:{ResolveClientIp(httpContext)}";
    }

    private static bool TryResolveRefreshTokenPartition(
        HttpContext httpContext,
        int refreshTokenLength,
        out string partition)
    {
        partition = string.Empty;
        if (!httpContext.Request.Cookies.TryGetValue(AuthCookieNames.RefreshToken, out var refreshToken)
            || string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        partition = $"refresh-token:{HashPrefix(refreshToken, refreshTokenLength)}";
        return true;
    }

    private static string? ResolveAuthenticatedPartitionOrDefault(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return $"user:{userId}";
    }

    private static string HashPrefix(string raw, int length)
    {
        var normalized = raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        var hash = Convert.ToHexString(bytes).ToLowerInvariant();
        var max = Math.Clamp(length, HashPrefixMinLength, hash.Length);
        return hash[..max];
    }
}
