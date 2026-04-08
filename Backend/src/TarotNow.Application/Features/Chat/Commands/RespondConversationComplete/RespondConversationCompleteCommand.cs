using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

// Command để participant phản hồi yêu cầu hoàn thành conversation.
public class RespondConversationCompleteCommand : IRequest<ConversationCompleteRespondResult>
{
    // Định danh conversation đang có yêu cầu hoàn thành.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh participant thực hiện phản hồi.
    public Guid RequesterId { get; set; }

    // Cờ phản hồi chấp thuận hay từ chối yêu cầu hoàn thành.
    public bool Accept { get; set; }
}
