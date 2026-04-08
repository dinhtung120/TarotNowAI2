using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

// Command khởi tạo phiên gọi mới trong một conversation.
public class InitiateCallCommand : IRequest<CallSessionDto>
{
    // Định danh conversation muốn bắt đầu cuộc gọi.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh người khởi tạo cuộc gọi.
    public Guid InitiatorId { get; set; }

    // Loại cuộc gọi (audio/video).
    public string Type { get; set; } = "audio";
}
