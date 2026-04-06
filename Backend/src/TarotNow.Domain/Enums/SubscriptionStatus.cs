/*
 * ===================================================================
 * FILE: SubscriptionStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum định nghĩa các trạng thái vòng đời của một gói đăng ký (Subscription) của User.
 *   Lý do thiết kế: Dùng const string thay vì C# enum truyền thống để dễ dàng lưu trữ và map thẳng với PostgreSQL (lưu dưới dạng string) mà không sợ bị lệch index (0, 1, 2) khi thêm bớt trạng thái sau này.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái của một gói Subscription mà User đang sở hữu.
/// Sử dụng const string để đồng nhất Type Safety ở tầng Entity và giữ Database dễ đọc.
/// </summary>
public static class SubscriptionStatus
{
    /// <summary>
    /// Gói đang trong thời hạn hiệu lực, người dùng được phép sử dụng các quyền lợi (entitlements) hàng ngày.
    /// Trạng thái này được set ngay sau khi thanh toán mua Subscription thành công.
    /// </summary>
    public const string Active = "active";

    /// <summary>
    /// Gói đã hết thời gian sử dụng (EndDate < hiện tại). 
    /// Job nền (Background Service) sẽ quét và đánh dấu trạng thái này, sau đó thu hồi các entitlements.
    /// </summary>
    public const string Expired = "expired";

    /// <summary>
    /// Gói bị hủy trước hạn (do Admin can thiệp xử lý khiếu nại, user refund qua cổng thanh toán, v.v.).
    /// Lý do cần phân biệt với Expired: Để tracking lịch sử dispute và không cho user reactive mà không trả thêm tiền.
    /// </summary>
    public const string Cancelled = "cancelled";
}
