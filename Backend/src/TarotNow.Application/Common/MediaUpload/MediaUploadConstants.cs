namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Hằng số dùng chung cho luồng upload media trực tiếp R2.
/// </summary>
public static class MediaUploadConstants
{
    /// <summary>TTL mặc định của presigned URL.</summary>
    public static readonly TimeSpan PresignExpiry = TimeSpan.FromMinutes(10);

    /// <summary>TTL mặc định của community asset chưa được attach.</summary>
    public static readonly TimeSpan CommunityOrphanTtl = TimeSpan.FromHours(24);

    /// <summary>Giới hạn ảnh upload (byte).</summary>
    public const long MaxImageUploadBytes = 10 * 1024 * 1024;

    /// <summary>Giới hạn voice upload (byte).</summary>
    public const long MaxVoiceUploadBytes = 5 * 1024 * 1024;

    /// <summary>MIME ảnh bắt buộc khi upload media image.</summary>
    public const string RequiredImageMimeType = "image/webp";

    /// <summary>Scope upload avatar.</summary>
    public const string ScopeAvatar = "avatar";

    /// <summary>Scope upload ảnh cộng đồng.</summary>
    public const string ScopeCommunityImage = "community_image";

    /// <summary>Scope upload ảnh chat.</summary>
    public const string ScopeChatImage = "chat_image";

    /// <summary>Scope upload voice chat.</summary>
    public const string ScopeChatVoice = "chat_voice";

    /// <summary>ContextType bài viết cộng đồng.</summary>
    public const string ContextPost = "post";

    /// <summary>ContextType bình luận cộng đồng.</summary>
    public const string ContextComment = "comment";

    /// <summary>Trạng thái asset đã upload thành công.</summary>
    public const string AssetStatusUploaded = "uploaded";

    /// <summary>Trạng thái asset đã được gắn với entity.</summary>
    public const string AssetStatusAttached = "attached";

    /// <summary>Trạng thái asset mồ côi chờ cleanup.</summary>
    public const string AssetStatusOrphaned = "orphaned";

    /// <summary>Trạng thái asset đã xóa object khỏi R2.</summary>
    public const string AssetStatusDeleted = "deleted";

    /// <summary>Trạng thái attach media: không có media cần attach.</summary>
    public const string EntityMediaAttachStatusNone = "none";

    /// <summary>Trạng thái attach media: đang chờ worker attach.</summary>
    public const string EntityMediaAttachStatusPending = "pending";

    /// <summary>Trạng thái attach media: attach đã hoàn tất.</summary>
    public const string EntityMediaAttachStatusCompleted = "completed";

    /// <summary>Trạng thái attach media: attach thất bại.</summary>
    public const string EntityMediaAttachStatusFailed = "failed";

    /// <summary>
    /// Kiểm tra contextType community hợp lệ.
    /// </summary>
    public static bool IsCommunityContextType(string value)
    {
        return string.Equals(value, ContextPost, StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, ContextComment, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra trạng thái attach media entity community hợp lệ.
    /// </summary>
    public static bool IsCommunityEntityMediaAttachStatus(string value)
    {
        return string.Equals(value, EntityMediaAttachStatusNone, StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, EntityMediaAttachStatusPending, StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, EntityMediaAttachStatusCompleted, StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, EntityMediaAttachStatusFailed, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra media kind chat hợp lệ.
    /// </summary>
    public static bool IsChatMediaKind(string value)
    {
        return string.Equals(value, "image", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "voice", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Chuẩn hóa context type về lowercase invariant.
    /// </summary>
    public static string NormalizeContextType(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}
