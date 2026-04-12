using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common.UserImageUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Pipeline upload ảnh dùng chung: đọc có giới hạn → MIME → virus scan → nén AVIF/WebP → PUT storage (có semaphore).
/// </summary>
public sealed class UserImagePipeline : IUserImagePipeline
{
    private readonly IMediaProcessorService _mediaProcessor;
    private readonly IObjectStorageService _objectStorage;
    private readonly IVirusScanService _virusScan;
    private readonly ImageUploadConcurrencyGate _concurrencyGate;
    private readonly ObjectStorageOptions _options;
    private readonly ILogger<UserImagePipeline> _logger;

    public UserImagePipeline(
        IMediaProcessorService mediaProcessor,
        IObjectStorageService objectStorage,
        IVirusScanService virusScan,
        ImageUploadConcurrencyGate concurrencyGate,
        IOptions<ObjectStorageOptions> options,
        ILogger<UserImagePipeline> logger)
    {
        _mediaProcessor = mediaProcessor;
        _objectStorage = objectStorage;
        _virusScan = virusScan;
        _concurrencyGate = concurrencyGate;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<UserImagePipelineResult> ProcessUploadAsync(
        Stream imageStream,
        string fileName,
        string contentType,
        UserImageUploadKind kind,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageStream);

        if (string.IsNullOrWhiteSpace(contentType) || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException("Định dạng file không được hỗ trợ. Chỉ nhận file ảnh.");
        }

        var rawBytes = await ReadStreamWithLimitAsync(imageStream, _options.MaxUploadBytes, cancellationToken);
        _logger.LogInformation(
            "User image pipeline: đầu vào sau đọc stream file={FileName} bytes={Bytes}",
            fileName,
            rawBytes.Length);

        await _virusScan.EnsureCleanAsync(rawBytes, cancellationToken);

        var maxEdge = kind == UserImageUploadKind.Avatar
            ? _options.AvatarMaxEdgePixels
            : _options.CommunityImageMaxEdgePixels;

        var keyPrefix = kind == UserImageUploadKind.Avatar ? "avatars" : "community";

        var (compressed, outMime) = await _mediaProcessor.ProcessAndCompressImageAsync(rawBytes, maxEdge, cancellationToken);
        _logger.LogInformation(
            "User image pipeline: sau nén BE mime={Mime} bytesBefore={Before} bytesAfter={After}",
            outMime,
            rawBytes.Length,
            compressed.Length);

        var ext = outMime.Contains("avif", StringComparison.OrdinalIgnoreCase) ? ".avif" : ".webp";

        await _concurrencyGate.WaitAsync(cancellationToken);
        try
        {
            var (objectKey, publicUrl) = await _objectStorage.PutBytesAsync(
                compressed,
                outMime,
                keyPrefix,
                ext,
                cancellationToken);

            return new UserImagePipelineResult(publicUrl, objectKey, outMime);
        }
        finally
        {
            _concurrencyGate.Release();
        }
    }

    private static async Task<byte[]> ReadStreamWithLimitAsync(Stream stream, long maxBytes, CancellationToken ct)
    {
        await using var ms = new MemoryStream();
        var buffer = new byte[65536]; // Tăng lên 64KB để tối ưu I/O throughput.
        long total = 0;
        int read;
        while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
        {
            total += read;
            if (total > maxBytes)
            {
                throw new ValidationException($"Ảnh vượt quá giới hạn {maxBytes} byte.");
            }

            ms.Write(buffer, 0, read);
        }

        if (ms.Length == 0)
        {
            throw new ValidationException("Dữ liệu ảnh không hợp lệ hoặc rỗng.");
        }

        return ms.ToArray();
    }
}
