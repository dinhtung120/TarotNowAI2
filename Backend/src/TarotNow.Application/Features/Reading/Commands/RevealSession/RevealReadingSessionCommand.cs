using MediatR;
using System;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

// Command mở bài cho một reading session đã được khởi tạo.
public class RevealReadingSessionCommand : IRequest<RevealReadingSessionResult>
{
    // Định danh user sở hữu phiên reading cần reveal.
    public Guid UserId { get; set; }

    // Định danh session cần mở bài.
    public string SessionId { get; set; } = string.Empty;

    // Ngôn ngữ ưu tiên dùng cho luồng precompute AI hậu reveal.
    public string Language { get; set; } = "vi";
}

// DTO kết quả reveal gồm danh sách lá bài đã rút.
public class RevealReadingSessionResult
{
    // Danh sách lá đã rút kèm vị trí và orientation.
    public ReadingDrawnCard[] Cards { get; set; } = Array.Empty<ReadingDrawnCard>();
}
