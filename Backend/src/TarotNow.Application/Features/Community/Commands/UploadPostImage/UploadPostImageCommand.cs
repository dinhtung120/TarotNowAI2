using MediatR;
using System.IO;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

// Command upload ảnh cho bài viết community.
public class UploadPostImageCommand : IRequest<UploadPostImageResult>
{
    // Stream ảnh đầu vào.
    public required Stream ImageStream { get; set; }

    // Tên file gốc từ client.
    public required string FileName { get; set; }

    // MIME type của ảnh đầu vào.
    public required string ContentType { get; set; }
}
