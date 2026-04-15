namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập message chuẩn cho command inventory.
/// </summary>
public static class InventoryCommandMessages
{
    /// <summary>
    /// Trạng thái command được tiếp nhận xử lý.
    /// </summary>
    public const string Accepted = "accepted";

    /// <summary>
    /// Trạng thái command là replay và không thực thi tiêu hao mới.
    /// </summary>
    public const string Replayed = "replayed";
}
