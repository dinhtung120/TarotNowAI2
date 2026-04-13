namespace TarotNow.Application.Interfaces;

/// <summary>
/// Adapter thao tác R2 cho luồng upload trực tiếp (presign + delete + URL mapping).
/// </summary>
public interface IR2UploadService
{
    /// <summary>
    /// Trạng thái backend đã cấu hình R2 hợp lệ.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Sinh presigned URL cho thao tác PUT object.
    /// </summary>
    Task<string> GeneratePresignedPutUrlAsync(
        string objectKey,
        string contentType,
        DateTime expiresAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa object khỏi R2 theo key.
    /// </summary>
    Task DeleteObjectAsync(string objectKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dựng public URL từ object key.
    /// </summary>
    string BuildPublicUrl(string objectKey);

    /// <summary>
    /// Thử trích object key từ public URL thuộc media domain whitelist.
    /// </summary>
    bool TryExtractObjectKey(string publicUrl, out string objectKey);
}
