namespace TarotNow.Api.Constants;

/// <summary>
/// Tập trung khóa feature flag để bật/tắt tính năng có kiểm soát.
/// Lý do: tránh sai tên flag giữa code, cấu hình và hệ thống quản trị.
/// </summary>
public static class FeatureFlags
{
    // Cờ bật luồng streaming AI theo thời gian thực.
    public const string AiStreamingEnabled = "AiStreamingEnabled";

    // Cờ bật phiên bản chat V2.
    public const string ChatV2Enabled = "ChatV2Enabled";

    // Cờ bật kiểm duyệt từ khóa cho chat.
    public const string ChatKeywordModerationEnabled = "ChatKeywordModerationEnabled";
}
