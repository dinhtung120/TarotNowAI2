using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

// Command phản hồi cuộc gọi đang ở trạng thái requested.
public class RespondCallCommand : IRequest<CallSessionDto>
{
    // Định danh phiên gọi cần phản hồi.
    public string CallSessionId { get; set; } = string.Empty;

    // Định danh người phản hồi cuộc gọi.
    public Guid ResponderId { get; set; }

    // Cờ true để nhận cuộc gọi, false để từ chối.
    public bool Accept { get; set; }
}
