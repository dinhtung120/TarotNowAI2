using TarotNow.Api.Constants;

namespace TarotNow.Api.Extensions;

/// <summary>
/// Helper chuẩn hóa cách đọc idempotency key từ header/request body.
/// </summary>
public static class HttpRequestIdempotencyExtensions
{
    /// <summary>
    /// Lấy idempotency key ưu tiên theo thứ tự:
    /// `Idempotency-Key` -> `x-idempotency-key` -> fallback values.
    /// </summary>
    public static string GetIdempotencyKeyOrEmpty(this HttpRequest request, params string?[] fallbacks)
    {
        if (TryReadHeader(request, AuthHeaders.IdempotencyKey, out var value))
        {
            return value;
        }

        if (TryReadHeader(request, AuthHeaders.LegacyIdempotencyKey, out value))
        {
            return value;
        }

        foreach (var fallback in fallbacks)
        {
            if (string.IsNullOrWhiteSpace(fallback))
            {
                continue;
            }

            return fallback.Trim();
        }

        return string.Empty;
    }

    private static bool TryReadHeader(HttpRequest request, string headerName, out string value)
    {
        value = string.Empty;
        if (!request.Headers.TryGetValue(headerName, out var headerValue))
        {
            return false;
        }

        var normalized = headerValue.ToString().Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return false;
        }

        value = normalized;
        return true;
    }
}
