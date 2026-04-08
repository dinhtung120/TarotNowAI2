
using System;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Entity subscription của người dùng để theo dõi vòng đời gói từ active tới expired/cancelled.
public class UserSubscription
{
    // Định danh subscription.
    public Guid Id { get; private set; }

    // Người dùng sở hữu subscription.
    public Guid UserId { get; private set; }

    // Gói subscription đã mua.
    public Guid PlanId { get; private set; }

    // Trạng thái hiện tại của subscription.
    public string Status { get; private set; } = string.Empty;

    // Thời điểm bắt đầu hiệu lực.
    public DateTime StartDate { get; private set; }

    // Thời điểm kết thúc hiệu lực.
    public DateTime EndDate { get; private set; }

    // Giá Diamond đã thanh toán.
    public long PricePaidDiamond { get; private set; }

    // Khóa idempotency giao dịch mua gói.
    public string IdempotencyKey { get; private set; } = string.Empty;

    // Thời điểm tạo subscription.
    public DateTime CreatedAt { get; private set; }

    // Navigation tới plan.
    public SubscriptionPlan Plan { get; private set; } = null!;

    // Navigation tới user.
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu database.
    /// </summary>
    protected UserSubscription() { }

    /// <summary>
    /// Khởi tạo subscription mới ở trạng thái Active sau khi thanh toán thành công.
    /// Luồng xử lý: sinh id, gán liên kết user/plan, set thời gian hiệu lực, giá và idempotency key.
    /// </summary>
    public UserSubscription(
        Guid userId,
        Guid planId,
        DateTime startDate,
        DateTime endDate,
        long pricePaidDiamond,
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        PricePaidDiamond = pricePaidDiamond;
        IdempotencyKey = idempotencyKey;
        Status = SubscriptionStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    // Subscription còn hiệu lực khi trạng thái Active và chưa quá EndDate.
    public bool IsActive => Status == SubscriptionStatus.Active && EndDate > DateTime.UtcNow;

    /// <summary>
    /// Đánh dấu subscription hết hạn khi đã qua thời gian hiệu lực.
    /// Luồng xử lý: chỉ cho phép đổi từ trạng thái Active sang Expired.
    /// </summary>
    public void Expire()
    {
        if (Status != SubscriptionStatus.Active)
        {
            // Business rule: chỉ luồng active mới được chuyển sang expired để giữ lifecycle rõ ràng.
            throw new InvalidOperationException($"Không thể hết hạn một gói đang trong trạng thái: {Status}");
        }

        Status = SubscriptionStatus.Expired;
        // Chốt trạng thái expired để ngăn entitlement tiếp tục được sử dụng.
    }

    /// <summary>
    /// Hủy subscription theo yêu cầu nghiệp vụ trước khi hết hạn tự nhiên.
    /// Luồng xử lý: cập nhật trạng thái sang Cancelled.
    /// </summary>
    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
        // Đánh dấu canceled để các luồng entitlement xử lý theo chính sách hủy.
    }
}
