using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

/// <summary>
/// Command dùng để một thành viên kết thúc cuộc gọi.
/// Có thể dùng cho trường hợp Hết timeout 60s (Auto End) hoặc 1 bên tự huỷ (Cancel).
/// </summary>
public class EndCallCommand : IRequest<CallSessionDto>
{
    /// <summary>
    /// ID của phiên gọi hiện tại.
    /// </summary>
    public string CallSessionId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của người gửi yêu cầu kết thúc cuộc gọi.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Lý do kết thúc: "normal" (nút End Call được bấm), "timeout" (hết 60s), "cancelled" (người gọi tự dừng).
    /// </summary>
    public string Reason { get; set; } = "normal";
}
