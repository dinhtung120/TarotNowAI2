

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

// Command tạo conversation mới giữa user và reader.
public class CreateConversationCommand : IRequest<ConversationDto>
{
    // Định danh người dùng tạo conversation.
    public Guid UserId { get; set; }

    // Định danh reader nhận conversation.
    public Guid ReaderId { get; set; }

    // SLA giờ áp dụng cho main question.
    public int SlaHours { get; set; }
}
