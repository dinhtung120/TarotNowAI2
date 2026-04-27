using System.Collections.Concurrent;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.IntegrationTests;

internal sealed class TestR2UploadService : IR2UploadService
{
    private const string PublicBaseUrl = "https://media.test.local";
    private readonly ConcurrentDictionary<string, byte> _knownObjects = new(StringComparer.Ordinal);

    public bool IsEnabled => true;

    public Task<string> GeneratePresignedPutUrlAsync(
        string objectKey,
        string contentType,
        DateTime expiresAtUtc,
        CancellationToken cancellationToken = default)
    {
        _knownObjects.TryAdd(objectKey, 0);
        var encodedObjectKey = Uri.EscapeDataString(objectKey);
        var uploadUrl = $"{PublicBaseUrl}/__test-presigned-upload/{encodedObjectKey}";
        return Task.FromResult(uploadUrl);
    }

    public Task DeleteObjectAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        _knownObjects.TryRemove(objectKey, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsObjectAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        var exists = _knownObjects.ContainsKey(objectKey);
        return Task.FromResult(exists);
    }

    public string BuildPublicUrl(string objectKey)
    {
        return $"{PublicBaseUrl.TrimEnd('/')}/{objectKey.TrimStart('/')}";
    }

    public bool TryExtractObjectKey(string publicUrl, out string objectKey)
    {
        objectKey = string.Empty;
        if (string.IsNullOrWhiteSpace(publicUrl))
        {
            return false;
        }

        var normalizedBase = PublicBaseUrl.TrimEnd('/');
        if (!publicUrl.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var tail = publicUrl.Substring(normalizedBase.Length).TrimStart('/');
        if (string.IsNullOrWhiteSpace(tail))
        {
            return false;
        }

        objectKey = Uri.UnescapeDataString(tail);
        return true;
    }
}
