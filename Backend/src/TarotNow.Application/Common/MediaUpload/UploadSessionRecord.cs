namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Bản ghi phiên upload tạm để validate token one-time.
/// </summary>
public sealed class UploadSessionRecord
{
    /// <summary>Token one-time do backend cấp.</summary>
    public string UploadToken { get; set; } = string.Empty;

    /// <summary>User sở hữu phiên upload.</summary>
    public Guid OwnerUserId { get; set; }

    /// <summary>Scope phiên upload (avatar/community/chat...).</summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>Object key dự kiến trên R2.</summary>
    public string ObjectKey { get; set; } = string.Empty;

    /// <summary>Public URL tương ứng object key.</summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>Content type đã khai báo lúc presign.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Kích thước file khai báo lúc presign.</summary>
    public long SizeBytes { get; set; }

    /// <summary>Conversation id (nếu scope là chat media).</summary>
    public string? ConversationId { get; set; }

    /// <summary>Context loại post/comment cho community.</summary>
    public string? ContextType { get; set; }

    /// <summary>Draft id do FE tạo trước khi entity thật tồn tại.</summary>
    public string? ContextDraftId { get; set; }

    /// <summary>Thời điểm tạo session.</summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>Thời điểm session hết hạn.</summary>
    public DateTime ExpiresAtUtc { get; set; }

    /// <summary>Thời điểm token đã consume.</summary>
    public DateTime? ConsumedAtUtc { get; set; }

    /// <summary>Thời điểm cleanup object hoàn tất khi session hết hạn.</summary>
    public DateTime? CleanedUpAtUtc { get; set; }
}
