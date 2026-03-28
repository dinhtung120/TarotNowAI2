using MediatR;
using System;
using System.IO;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

public class UploadAvatarCommand : IRequest<string>
{
    public Guid UserId { get; set; }
    public Stream ImageStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
