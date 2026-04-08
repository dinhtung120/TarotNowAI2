
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái vòng đời subscription người dùng.
public static class SubscriptionStatus
{
    // Subscription đang hiệu lực.
    public const string Active = "active";

    // Subscription đã hết hạn.
    public const string Expired = "expired";

    // Subscription đã bị hủy.
    public const string Cancelled = "cancelled";
}
