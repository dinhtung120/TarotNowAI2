namespace TarotNow.Domain.Enums;

// Trạng thái provisioning payment link cho đơn nạp.
public static class DepositPaymentLinkStatus
{
    // Đơn nạp đang chờ worker tạo payment link ở gateway.
    public const string Provisioning = "provisioning";

    // Payment link đã sẵn sàng để client điều hướng thanh toán.
    public const string Ready = "ready";

    // Worker tạo payment link thất bại, cần retry/reconcile.
    public const string Failed = "failed";
}
