using MediatR;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

/// <summary>
/// Command chuyển đổi trạng thái online/offline/accepting của Reader.
///
/// Đây là core feature cho gate check (P2-READER-QA-1.2):
/// → Reader offline → user KHÔNG thể gửi chat.
/// → Reader accepting_questions → user CÓ THỂ gửi chat.
///
/// UserId từ JWT, Status từ request body.
/// </summary>
public class UpdateReaderStatusCommand : IRequest<bool>
{
    /// <summary>UUID reader — lấy từ JWT claims.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Trạng thái mới: "online" | "offline" | "accepting_questions".
    /// Validate bằng Enum checking trong Handler.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
