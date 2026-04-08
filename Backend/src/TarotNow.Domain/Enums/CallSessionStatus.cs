using System.ComponentModel;

namespace TarotNow.Domain.Enums;

// Enum trạng thái phiên gọi để đồng bộ lifecycle cuộc gọi audio/video.
public enum CallSessionStatus
{
    // Phiên gọi vừa được yêu cầu.
    [Description("requested")]
    Requested = 0,

    // Phiên gọi đã được chấp nhận.
    [Description("accepted")]
    Accepted = 1,

    // Phiên gọi đã bị từ chối.
    [Description("rejected")]
    Rejected = 2,

    // Phiên gọi đã kết thúc.
    [Description("ended")]
    Ended = 3
}
