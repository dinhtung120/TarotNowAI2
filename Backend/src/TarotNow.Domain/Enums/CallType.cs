using System.ComponentModel;

namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại cuộc gọi (Audio hoặc Video).
/// </summary>
public enum CallType
{
    /// <summary>
    /// Gọi thoại (chỉ sử dụng microphone, không có camera).
    /// </summary>
    [Description("audio")]
    Audio = 0,

    /// <summary>
    /// Gọi video (sử dụng cả camera và microphone).
    /// </summary>
    [Description("video")]
    Video = 1
}
