namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Bản ghi asset ảnh community để quản lý trạng thái uploaded/attached/orphaned/deleted.
/// </summary>
public sealed class CommunityMediaAssetRecord
{
    /// <summary>Object key trên R2.</summary>
    public string ObjectKey { get; set; } = string.Empty;

    /// <summary>Public URL của object.</summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>User sở hữu asset.</summary>
    public Guid OwnerUserId { get; set; }

    /// <summary>Loại context (post/comment).</summary>
    public string ContextType { get; set; } = string.Empty;

    /// <summary>Draft id do FE cấp trước khi tạo entity thật.</summary>
    public string? ContextDraftId { get; set; }

    /// <summary>Id entity đã attach (postId/commentId).</summary>
    public string? ContextEntityId { get; set; }

    /// <summary>Trạng thái asset hiện tại.</summary>
    public string Status { get; set; } = MediaUploadConstants.AssetStatusUploaded;

    /// <summary>Thời điểm tạo record.</summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>Thời điểm cập nhật gần nhất.</summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>Thời điểm attach vào entity.</summary>
    public DateTime? AttachedAtUtc { get; set; }

    /// <summary>Thời điểm chuyển orphaned.</summary>
    public DateTime? OrphanedAtUtc { get; set; }

    /// <summary>Thời điểm object đã bị xóa khỏi R2.</summary>
    public DateTime? DeletedAtUtc { get; set; }

    /// <summary>Hạn cleanup khi asset chưa attach hoặc bị orphaned.</summary>
    public DateTime ExpiresAtUtc { get; set; }
}
