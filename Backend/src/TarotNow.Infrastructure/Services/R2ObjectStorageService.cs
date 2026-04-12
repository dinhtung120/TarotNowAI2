using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Upload/xóa object trên Cloudflare R2 (API tương thích S3).
/// </summary>
public sealed class R2ObjectStorageService : IObjectStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly R2ObjectStorageSettings _settings;
    private readonly ILogger<R2ObjectStorageService> _logger;

    public R2ObjectStorageService(IOptions<ObjectStorageOptions> options, ILogger<R2ObjectStorageService> logger)
    {
        var root = options.Value;
        _settings = root.R2;
        _logger = logger;
        _s3 = CreateS3Client(_settings);
    }

    private static IAmazonS3 CreateS3Client(R2ObjectStorageSettings r2)
    {
        var credentials = new BasicAWSCredentials(r2.AccessKeyId.Trim(), r2.SecretAccessKey.Trim());
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = $"https://{r2.AccountId.Trim()}.r2.cloudflarestorage.com",
            ForcePathStyle = true,
            AuthenticationRegion = "auto",
        };
        return new AmazonS3Client(credentials, config);
    }

    public async Task<(string ObjectKey, string PublicUrl)> PutBytesAsync(
        byte[] data,
        string contentType,
        string keyPrefix,
        string fileExtension,
        CancellationToken cancellationToken = default)
    {
        var key = $"{keyPrefix.TrimEnd('/')}/{Guid.NewGuid():N}{fileExtension}";
        await using var stream = new MemoryStream(data, writable: false);
        var put = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType,
            AutoResetStreamPosition = true,
            DisablePayloadSigning = true,
        };

        await _s3.PutObjectAsync(put, cancellationToken);
        var baseUrl = _settings.PublicBaseUrl.TrimEnd('/');
        var publicUrl = $"{baseUrl}/{key}";
        _logger.LogInformation("R2 PUT key={ObjectKey} bytes={Bytes}", key, data.Length);
        return (key, publicUrl);
    }

    public async Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(objectKey))
        {
            return;
        }

        await _s3.DeleteObjectAsync(_settings.BucketName, objectKey, null, cancellationToken);
        _logger.LogInformation("R2 DELETE key={ObjectKey}", objectKey);
    }
}
