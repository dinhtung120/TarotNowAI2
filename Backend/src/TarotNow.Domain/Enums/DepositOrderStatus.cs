namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái của lệnh nạp tiền.
public static class DepositOrderStatus
{
    // Lệnh nạp đã tạo, đang chờ thanh toán.
    public const string Pending = "pending";

    // Lệnh nạp đã thanh toán thành công.
    public const string Success = "success";

    // Lệnh nạp đã thất bại hoặc bị từ chối.
    public const string Failed = "failed";
}
