using System.ComponentModel;

namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái một phiên cuộc gọi thoại / video (Voice/Video Call).
/// Luồng: Requested -> Accepted -> Ended | Requested -> Rejected | Requested -> Ended (timeout/cancel)
/// </summary>
public enum CallSessionStatus
{
    /// <summary>
    /// Đang chờ bên nhận phản hồi (timeout 60s).
    /// </summary>
    [Description("requested")]
    Requested = 0,

    /// <summary>
    /// Đã chấp nhận, WebRTC đang kết nối hoặc cuộc gọi đang diễn ra.
    /// </summary>
    [Description("accepted")]
    Accepted = 1,

    /// <summary>
    /// Bên nhận từ chối cuộc gọi.
    /// </summary>
    [Description("rejected")]
    Rejected = 2,

    /// <summary>
    /// Cuộc gọi kết thúc (bình thường, timeout, hoặc caller hủy trước khi accept).
    /// </summary>
    [Description("ended")]
    Ended = 3
}
