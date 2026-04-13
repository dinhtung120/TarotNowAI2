namespace TarotNow.Application.Interfaces;

/// <summary>
/// Dịch vụ đồng bộ asset community theo nội dung markdown post/comment.
/// </summary>
public interface ICommunityMediaAttachmentService
{
    /// <summary>
    /// Attach asset cho entity mới tạo bằng context draft.
    /// </summary>
    Task AttachForNewEntityAsync(
        Guid ownerUserId,
        string contextType,
        string? contextDraftId,
        string contextEntityId,
        string markdownContent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reconcile asset cho entity đã tồn tại khi update nội dung.
    /// </summary>
    Task ReconcileForExistingEntityAsync(
        Guid ownerUserId,
        string contextType,
        string contextEntityId,
        string markdownContent,
        CancellationToken cancellationToken = default);
}
