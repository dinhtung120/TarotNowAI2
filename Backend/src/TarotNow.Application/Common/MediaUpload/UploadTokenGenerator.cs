using System.Security.Cryptography;

namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Sinh upload token one-time theo chuẩn URL-safe.
/// </summary>
public static class UploadTokenGenerator
{
    /// <summary>
    /// Sinh token ngẫu nhiên 32 bytes và encode Base64Url.
    /// </summary>
    public static string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncode(bytes);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert
            .ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
