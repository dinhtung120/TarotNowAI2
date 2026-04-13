namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Cấu hình object storage cho luồng upload trực tiếp R2.
/// </summary>
public sealed class ObjectStorageOptions
{
    /// <summary>Hiện chỉ hỗ trợ R2 cho upload media mới.</summary>
    public string Provider { get; set; } = "R2";

    /// <summary>Giới hạn kích thước upload tối đa (byte).</summary>
    public long MaxUploadBytes { get; set; } = 10 * 1024 * 1024;

    /// <summary>Thời gian sống presigned URL (phút).</summary>
    public int PresignExpiryMinutes { get; set; } = 10;

    /// <summary>TTL asset community chưa attach hoặc orphaned (giờ).</summary>
    public int CommunityOrphanTtlHours { get; set; } = 24;

    /// <summary>Số lượng object tối đa xử lý mỗi vòng cleanup.</summary>
    public int CleanupBatchSize { get; set; } = 200;

    /// <summary>Chu kỳ quét cleanup session/asset (phút).</summary>
    public int CleanupIntervalMinutes { get; set; } = 10;

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
