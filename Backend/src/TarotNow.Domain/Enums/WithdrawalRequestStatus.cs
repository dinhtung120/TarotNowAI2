namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái yêu cầu rút tiền.
public static class WithdrawalRequestStatus
{
    // Yêu cầu mới tạo, đang chờ admin xử lý.
    public const string Pending = "pending";

    // Yêu cầu đã được admin phê duyệt.
    public const string Approved = "approved";

    // Yêu cầu bị admin từ chối.
    public const string Rejected = "rejected";

    // Trạng thái chi trả hoàn tất (giữ để tương thích dữ liệu cũ).
    public const string Paid = "paid";
}
