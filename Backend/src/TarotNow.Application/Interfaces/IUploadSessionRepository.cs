using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository lưu phiên upload tạm và token one-time.
/// </summary>
public interface IUploadSessionRepository
{
    /// <summary>
    /// Tạo session mới sau khi presign.
    /// </summary>
    Task CreateAsync(UploadSessionRecord session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy session theo upload token.
    /// </summary>
    Task<UploadSessionRecord?> GetByTokenAsync(string uploadToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Consume token one-time theo điều kiện chưa hết hạn/chưa consume.
    /// </summary>
    Task<bool> ConsumeAsync(string uploadToken, DateTime consumedAtUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách session đã hết hạn nhưng chưa consume để cleanup object.
    /// </summary>
    Task<IReadOnlyList<UploadSessionRecord>> GetExpiredUnconsumedAsync(
        DateTime nowUtc,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu session đã cleanup object thành công.
    /// </summary>
    Task<bool> MarkCleanedAsync(string uploadToken, DateTime cleanedAtUtc, CancellationToken cancellationToken = default);
}
