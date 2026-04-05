using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

/// <summary>
/// Command dùng để Người nhận phản hồi một cuộc gọi đến (Accept hoặc Reject).
/// </summary>
public class RespondCallCommand : IRequest<CallSessionDto>
{
    /// <summary>
    /// ID của phiên gọi hiện tại.
    /// </summary>
    public string CallSessionId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của người phản hồi cuộc gọi.
    /// </summary>
    public Guid ResponderId { get; set; }

    /// <summary>
    /// Đồng ý nghe máy (True) hay Từ chối (False)?
    /// </summary>
    public bool Accept { get; set; }
}
