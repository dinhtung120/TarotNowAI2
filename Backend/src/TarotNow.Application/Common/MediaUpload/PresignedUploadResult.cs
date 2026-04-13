namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Kết quả presign trả về cho client upload trực tiếp lên R2.
/// </summary>
public sealed record PresignedUploadResult(
    string UploadUrl,
    string ObjectKey,
    string PublicUrl,
    string UploadToken,
    DateTime ExpiresAtUtc);
