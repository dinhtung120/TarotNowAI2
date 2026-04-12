namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

/// <summary>
/// Kết quả upload avatar: URL hiển thị và publicId (object key) trên storage.
/// </summary>
public sealed record UploadAvatarResult(string AvatarUrl, string PublicId);
