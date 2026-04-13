namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Trạng thái lifecycle của outbox message.
/// </summary>
public static class OutboxMessageStatus
{
    /// <summary>
    /// Message mới tạo, chờ xử lý.
    /// </summary>
    public const string Pending = "pending";

    /// <summary>
    /// Message đang được worker xử lý.
    /// </summary>
    public const string Processing = "processing";

    /// <summary>
    /// Message xử lý lỗi, sẽ retry.
    /// </summary>
    public const string Failed = "failed";

    /// <summary>
    /// Message đã xử lý thành công.
    /// </summary>
    public const string Processed = "processed";

    /// <summary>
    /// Message lỗi quá số lần retry, dừng xử lý.
    /// </summary>
    public const string DeadLetter = "dead_letter";
}
