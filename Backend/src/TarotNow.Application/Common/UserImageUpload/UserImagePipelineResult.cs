namespace TarotNow.Application.Common.UserImageUpload;

/// <summary>
/// Kết quả pipeline: URL public và publicId (object key) để xóa trên object storage.
/// </summary>
public sealed record UserImagePipelineResult(string PublicUrl, string PublicId, string ContentType);
