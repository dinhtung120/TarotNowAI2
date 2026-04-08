using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

// Command kết thúc một phiên gọi đang diễn ra/chờ phản hồi.
public class EndCallCommand : IRequest<CallSessionDto>
{
    // Định danh phiên gọi cần kết thúc.
    public string CallSessionId { get; set; } = string.Empty;

    // Định danh user thực hiện thao tác kết thúc.
    public Guid UserId { get; set; }

    // Lý do kết thúc cuộc gọi.
    public string Reason { get; set; } = "normal";
}
