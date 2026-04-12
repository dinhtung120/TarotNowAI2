namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

/// <summary>
/// Kết quả upload ảnh bài viết: URL chèn markdown và publicId trên object storage.
/// </summary>
public sealed record UploadPostImageResult(string Url, string PublicId);
