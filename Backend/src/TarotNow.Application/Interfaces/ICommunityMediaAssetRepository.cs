using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository quản lý vòng đời asset ảnh community.
/// </summary>
public interface ICommunityMediaAssetRepository
{
    /// <summary>
    /// Upsert trạng thái uploaded sau bước confirm.
    /// </summary>
    Task UpsertUploadedAsync(CommunityMediaAssetRecord asset, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gắn asset uploaded theo context draft vào entity đã tạo.
    /// </summary>
    Task AttachDraftAssetsAsync(
        Guid ownerUserId,
        string contextType,
        string contextDraftId,
        string contextEntityId,
        IReadOnlyCollection<string> objectKeys,
        DateTime attachedAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gắn asset theo object key vào entity hiện tại (dùng cho edit flow).
    /// </summary>
    Task AttachByObjectKeysAsync(
        Guid ownerUserId,
        string contextType,
        string contextEntityId,
        IReadOnlyCollection<string> objectKeys,
        DateTime attachedAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reconcile danh sách asset attached và chuyển key bị bỏ ra orphaned.
    /// </summary>
    Task ReconcileAttachedAssetsAsync(
        Guid ownerUserId,
        string contextType,
        string contextEntityId,
        IReadOnlyCollection<string> activeObjectKeys,
        DateTime reconciledAtUtc,
        DateTime orphanExpiresAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách asset đã quá hạn cần cleanup object trên R2.
    /// </summary>
    Task<IReadOnlyList<CommunityMediaAssetRecord>> GetCleanupCandidatesAsync(
        DateTime nowUtc,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu asset đã xóa object khỏi R2.
    /// </summary>
    Task<bool> MarkDeletedAsync(string objectKey, DateTime deletedAtUtc, CancellationToken cancellationToken = default);
}
