namespace TarotNow.Application.Common;

// Trạng thái thực thi của một thao tác ví theo cơ chế idempotent.
public enum WalletOperationStatus
{
    // Thao tác đã được thực thi thành công ở lần gọi hiện tại.
    Executed = 0,
    // Thao tác đã được xử lý trước đó, lần gọi này chỉ nhận lại kết quả idempotent.
    AlreadyHandled = 1
}

// Value object gói kết quả thao tác ví để chuẩn hóa nhánh executed/already-handled.
public sealed class WalletOperationResult
{
    /// <summary>
    /// Khởi tạo kết quả thao tác ví với trạng thái chỉ định.
    /// Luồng xử lý: lưu trạng thái nền để expose qua các thuộc tính tĩnh tiện dụng.
    /// </summary>
    private WalletOperationResult(WalletOperationStatus status)
    {
        Status = status;
    }

    // Trạng thái kết quả thao tác ví.
    public WalletOperationStatus Status { get; }

    // Cờ tiện dụng cho biết thao tác có thực sự được thực thi ở lần gọi này hay không.
    public bool Executed => Status == WalletOperationStatus.Executed;

    // Singleton kết quả cho nhánh thực thi thành công.
    public static WalletOperationResult ExecutedResult { get; } =
        new(WalletOperationStatus.Executed);

    // Singleton kết quả cho nhánh đã xử lý trước đó.
    public static WalletOperationResult AlreadyHandledResult { get; } =
        new(WalletOperationStatus.AlreadyHandled);
}
