using System.ComponentModel.DataAnnotations;

namespace TarotNow.Api.Contracts.Requests;

// Payload yêu cầu presign upload avatar.
public sealed class AvatarPresignRequest
{
    [Required]
    [MaxLength(64)]
    public string ContentType { get; set; } = string.Empty;

    [Range(1, long.MaxValue)]
    public long SizeBytes { get; set; }
}

// Payload xác nhận avatar upload thành công.
public sealed class AvatarConfirmRequest
{
    [Required]
    [MaxLength(512)]
    public string ObjectKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string PublicUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string UploadToken { get; set; } = string.Empty;
}

// Payload yêu cầu presign ảnh community cho post/comment.
public sealed class CommunityImagePresignRequest
{
    [Required]
    [MaxLength(32)]
    public string ContextType { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string ContextDraftId { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string ContentType { get; set; } = string.Empty;

    [Range(1, long.MaxValue)]
    public long SizeBytes { get; set; }
}

// Payload xác nhận ảnh community upload thành công.
public sealed class CommunityImageConfirmRequest
{
    [Required]
    [MaxLength(32)]
    public string ContextType { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string ContextDraftId { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string ObjectKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string PublicUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string UploadToken { get; set; } = string.Empty;
}

// Payload yêu cầu presign media chat.
public sealed class ConversationMediaPresignRequest
{
    [Required]
    [MaxLength(16)]
    public string MediaKind { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string ContentType { get; set; } = string.Empty;

    [Range(1, long.MaxValue)]
    public long SizeBytes { get; set; }

    public int? DurationMs { get; set; }
}

// Response chuẩn trả về cho endpoint presign.
public sealed record PresignedUploadResponse(
    string UploadUrl,
    string ObjectKey,
    string PublicUrl,
    string UploadToken,
    DateTime ExpiresAtUtc);
