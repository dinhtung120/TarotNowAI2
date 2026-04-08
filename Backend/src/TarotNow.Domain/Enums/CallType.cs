using System.ComponentModel;

namespace TarotNow.Domain.Enums;

// Enum loại cuộc gọi hỗ trợ trong hệ thống.
public enum CallType
{
    // Cuộc gọi chỉ âm thanh.
    [Description("audio")]
    Audio = 0,

    // Cuộc gọi có hình ảnh.
    [Description("video")]
    Video = 1
}
