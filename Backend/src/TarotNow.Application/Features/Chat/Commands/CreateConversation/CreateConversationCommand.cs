using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

/// <summary>
/// Command tạo conversation chat 1-1 mới.
///
/// Business rule:
/// → Chỉ cho phép tạo nếu chưa có conversation active giữa 2 user.
/// → Reader phải đang online/accepting_questions.
/// → Status ban đầu: pending (chờ reader join).
/// </summary>
public class CreateConversationCommand : IRequest<ConversationDto>
{
    /// <summary>UUID user (người đặt câu hỏi).</summary>
    public Guid UserId { get; set; }

    /// <summary>UUID reader (nhà xem bài).</summary>
    public Guid ReaderId { get; set; }
}
