using MediatR;
using System;
using System.IO;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

// Command upload avatar mới cho người dùng.
public class UploadAvatarCommand : IRequest<string>
{
    // Định danh user cần cập nhật avatar.
    public Guid UserId { get; set; }

    // Luồng dữ liệu ảnh gốc người dùng tải lên.
    public Stream ImageStream { get; set; } = Stream.Null;

    // Tên file gốc để sinh tên file lưu trữ.
    public string FileName { get; set; } = string.Empty;

    // MIME type của ảnh tải lên.
    public string ContentType { get; set; } = string.Empty;
}
