namespace TarotNow.Api.Constants;

/// <summary>
/// Khai báo các loại message dùng thống nhất giữa API và client realtime.
/// Lý do: tránh sai chính tả chuỗi literal khi publish/consume sự kiện.
/// </summary>
public static class ApiMessageTypes
{
    // Loại message dành cho bản ghi cuộc gọi.
    public const string CallLog = "call_log";
}
