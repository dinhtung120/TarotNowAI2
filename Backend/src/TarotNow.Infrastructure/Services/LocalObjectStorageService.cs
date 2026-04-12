using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Lưu object qua <see cref="IFileStorageService"/> (wwwroot/uploads) — dùng cho dev/test.
/// </summary>
public sealed class LocalObjectStorageService : IObjectStorageService
{
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<LocalObjectStorageService> _logger;

    public LocalObjectStorageService(IFileStorageService fileStorage, ILogger<LocalObjectStorageService> logger)
    {
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<(string ObjectKey, string PublicUrl)> PutBytesAsync(
        byte[] data,
        string contentType,
        string keyPrefix,
        string fileExtension,
        CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream(data, writable: false);
        var name = $"{Guid.NewGuid():N}{fileExtension}";
        var relativeUrl = await _fileStorage.SaveFileAsync(ms, name, keyPrefix, cancellationToken);
        var objectKey = relativeUrl.TrimStart('/');
        _logger.LogInformation("Local object storage PUT key={ObjectKey} bytes={Bytes}", objectKey, data.Length);
        return (objectKey, relativeUrl);
    }

    public Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        var path = objectKey.StartsWith('/') ? objectKey : $"/{objectKey}";
        return _fileStorage.DeleteFileAsync(path, cancellationToken);
    }
}
