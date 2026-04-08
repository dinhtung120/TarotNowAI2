using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

// Command mở bài cho một reading session đã được khởi tạo.
public class RevealReadingSessionCommand : IRequest<RevealReadingSessionResult>
{
    // Định danh user sở hữu phiên reading cần reveal.
    public Guid UserId { get; set; }

    // Định danh session cần mở bài.
    public string SessionId { get; set; } = string.Empty;
}

// DTO kết quả reveal gồm danh sách lá bài đã rút.
public class RevealReadingSessionResult
{
    // Mảng id lá bài đã rút theo spread.
    public int[] Cards { get; set; } = Array.Empty<int>();
}
