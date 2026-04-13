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
/// Adapter thao tác Cloudflare R2 cho luồng upload trực tiếp.
/// </summary>
public sealed class R2UploadService : IR2UploadService
{
    private readonly IAmazonS3? _s3;
    private readonly R2ObjectStorageSettings _settings;
    private readonly ILogger<R2UploadService> _logger;
    private readonly Uri? _publicBaseUri;

    /// <summary>
    /// Khởi tạo service thao tác R2.
    /// </summary>
    public R2UploadService(IOptions<ObjectStorageOptions> options, ILogger<R2UploadService> logger)
    {
        _settings = options.Value.R2;
        _logger = logger;

        IsEnabled = string.Equals(options.Value.Provider, "R2", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(_settings.AccountId)
            && !string.IsNullOrWhiteSpace(_settings.AccessKeyId)
            && !string.IsNullOrWhiteSpace(_settings.SecretAccessKey)
            && !string.IsNullOrWhiteSpace(_settings.BucketName)
            && !string.IsNullOrWhiteSpace(_settings.PublicBaseUrl)
            && Uri.TryCreate(_settings.PublicBaseUrl, UriKind.Absolute, out _publicBaseUri);

        if (IsEnabled)
        {
            _s3 = CreateS3Client(_settings);
        }
    }

    /// <inheritdoc />
    public bool IsEnabled { get; }

    /// <inheritdoc />
    public Task<string> GeneratePresignedPutUrlAsync(
        string objectKey,
        string contentType,
        DateTime expiresAtUtc,
        CancellationToken cancellationToken = default)
    {
        EnsureEnabled();

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = NormalizeObjectKey(objectKey),
            Verb = HttpVerb.PUT,
            Expires = expiresAtUtc,
            ContentType = contentType,
            Protocol = Protocol.HTTPS,
        };

        var uploadUrl = _s3!.GetPreSignedURL(request);
        return Task.FromResult(uploadUrl);
    }

    /// <inheritdoc />
    public async Task DeleteObjectAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled || string.IsNullOrWhiteSpace(objectKey))
        {
            return;
        }

        var request = new DeleteObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = NormalizeObjectKey(objectKey),
        };

        await _s3!.DeleteObjectAsync(request, cancellationToken);
        _logger.LogInformation("R2 DELETE key={ObjectKey}", objectKey);
    }

    /// <inheritdoc />
    public string BuildPublicUrl(string objectKey)
    {
        EnsureEnabled();
        var baseUrl = _settings.PublicBaseUrl.TrimEnd('/');
        return $"{baseUrl}/{NormalizeObjectKey(objectKey)}";
    }

    /// <inheritdoc />
    public bool TryExtractObjectKey(string publicUrl, out string objectKey)
    {
        objectKey = string.Empty;
        if (!IsEnabled || _publicBaseUri is null)
        {
            return false;
        }

        if (!Uri.TryCreate(publicUrl, UriKind.Absolute, out var parsedUri))
        {
            return false;
        }

        if (!HasSameOrigin(parsedUri, _publicBaseUri))
        {
            return false;
        }

        var basePath = _publicBaseUri.AbsolutePath.TrimEnd('/');
        var fullPath = parsedUri.AbsolutePath;
        if (!string.IsNullOrEmpty(basePath) && !string.Equals(basePath, "/", StringComparison.Ordinal))
        {
            if (!fullPath.StartsWith(basePath + "/", StringComparison.Ordinal))
            {
                return false;
            }

            fullPath = fullPath[basePath.Length..];
        }

        var normalized = NormalizeObjectKey(fullPath);
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return false;
        }

        objectKey = normalized;
        return true;
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

    private void EnsureEnabled()
    {
        if (IsEnabled == false)
        {
            throw new InvalidOperationException("R2 upload service chưa được cấu hình đầy đủ.");
        }
    }

    private static bool HasSameOrigin(Uri left, Uri right)
    {
        return string.Equals(left.Scheme, right.Scheme, StringComparison.OrdinalIgnoreCase)
               && string.Equals(left.Host, right.Host, StringComparison.OrdinalIgnoreCase)
               && left.Port == right.Port;
    }

    private static string NormalizeObjectKey(string raw)
    {
        return raw.Trim().TrimStart('/');
    }
}
