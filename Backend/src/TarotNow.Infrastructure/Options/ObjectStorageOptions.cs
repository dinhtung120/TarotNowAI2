namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Cấu hình object storage (R2 hoặc fallback local).
/// </summary>
public sealed class ObjectStorageOptions
{
    /// <summary>Local | R2</summary>
    public string Provider { get; set; } = "Local";

    public int MaxConcurrentUploads { get; set; } = 4;

    public long MaxUploadBytes { get; set; } = 10 * 1024 * 1024;

    public int AvatarMaxEdgePixels { get; set; } = 512;

    public int CommunityImageMaxEdgePixels { get; set; } = 1024;

    public R2ObjectStorageSettings R2 { get; set; } = new();
}

/// <summary>
/// Thông tin kết nối Cloudflare R2 (S3-compatible API).
/// </summary>
public sealed class R2ObjectStorageSettings
{
    public string AccountId { get; set; } = string.Empty;

    public string AccessKeyId { get; set; } = string.Empty;

    public string SecretAccessKey { get; set; } = string.Empty;

    public string BucketName { get; set; } = string.Empty;

    /// <summary>URL public (custom domain hoặc r2.dev), không có slash cuối.</summary>
    public string PublicBaseUrl { get; set; } = string.Empty;
}
