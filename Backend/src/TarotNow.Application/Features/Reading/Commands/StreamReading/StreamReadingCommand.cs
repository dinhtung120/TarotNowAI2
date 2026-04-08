using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// Command yêu cầu stream kết quả giải bài AI cho một reading session.
public class StreamReadingCommand : IRequest<StreamReadingResult>
{
    // Định danh user gửi yêu cầu stream.
    public Guid UserId { get; set; }

    // Định danh reading session đã reveal bài.
    public string ReadingSessionId { get; set; } = string.Empty;

    // Câu hỏi follow-up (nếu có); rỗng nghĩa là yêu cầu giải bài ban đầu.
    public string? FollowupQuestion { get; set; }

    // Ngôn ngữ phản hồi mong muốn của AI (vi/en/zh).
    public string Language { get; set; } = "vi";
}
