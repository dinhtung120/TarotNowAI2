using MediatR;
using System.IO;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

public class UploadPostImageCommand : IRequest<string>
{
    public required Stream ImageStream { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
}
