
using System;

namespace TarotNow.Domain.Entities;

// Entity bucket entitlement theo ngày nghiệp vụ để quản lý quota còn lại của subscription.
public class SubscriptionEntitlementBucket
{
    // Định danh bucket.
    public Guid Id { get; private set; }

    // Định danh subscription phát sinh bucket.
    public Guid UserSubscriptionId { get; private set; }

    // Định danh người dùng.
    public Guid UserId { get; private set; }

    // Khóa entitlement của bucket.
    public string EntitlementKey { get; private set; } = string.Empty;

    // Hạn mức sử dụng trong ngày.
    public int DailyQuota { get; private set; }

    // Số lượng đã dùng trong ngày hiện tại.
    public int UsedToday { get; private set; }

    // Ngày nghiệp vụ hiện tại của bucket.
    public DateOnly BusinessDate { get; private set; }

    // Ngày kết thúc subscription sở hữu bucket.
    public DateTime SubscriptionEndDate { get; private set; }

    // Navigation tới subscription cha.
    public UserSubscription UserSubscription { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu đã lưu.
    /// </summary>
    protected SubscriptionEntitlementBucket() { }

    /// <summary>
    /// Khởi tạo bucket entitlement mới cho người dùng tại một ngày nghiệp vụ cụ thể.
    /// Luồng xử lý: sinh id, gán liên kết subscription/user, đặt quota ban đầu và UsedToday = 0.
    /// </summary>
    public SubscriptionEntitlementBucket(
        Guid userSubscriptionId,
        Guid userId,
        string entitlementKey,
        int dailyQuota,
        DateOnly currentDate,
        DateTime subscriptionEndDate)
    {
        Id = Guid.NewGuid();
        UserSubscriptionId = userSubscriptionId;
        UserId = userId;
        EntitlementKey = entitlementKey;
        DailyQuota = dailyQuota;
        UsedToday = 0;
        BusinessDate = currentDate;
        SubscriptionEndDate = subscriptionEndDate;
    }

    /// <summary>
    /// Kiểm tra bucket còn có thể tiêu thụ trong ngày hiện tại hay không.
    /// Luồng xử lý: xác thực đúng BusinessDate và so sánh UsedToday với DailyQuota.
    /// </summary>
    public bool CanConsume(DateOnly todayUtc)
    {
        // Chỉ được consume khi đúng ngày nghiệp vụ và chưa chạm trần quota.
        return BusinessDate == todayUtc && UsedToday < DailyQuota;
    }

    /// <summary>
    /// Tiêu thụ một đơn vị quota từ bucket.
    /// Luồng xử lý: kiểm tra điều kiện tiêu thụ hợp lệ, sau đó tăng UsedToday thêm 1.
    /// </summary>
    public void Consume(DateOnly todayUtc)
    {
        if (!CanConsume(todayUtc))
        {
            // Business rule: không cho consume khi sai ngày hoặc đã hết quota.
            throw new InvalidOperationException($"Không thể consume entitlement {EntitlementKey}. Quota: {UsedToday}/{DailyQuota}. Date: {BusinessDate} vs {todayUtc}");
        }

        UsedToday++;
        // Cập nhật state bucket sau mỗi lượt tiêu thụ để đồng bộ hạn mức còn lại.
    }

    /// <summary>
    /// Reset bộ đếm tiêu thụ khi chuyển sang ngày nghiệp vụ mới.
    /// Luồng xử lý: bỏ qua nếu ngày mới không lớn hơn ngày hiện tại, ngược lại reset UsedToday và đổi BusinessDate.
    /// </summary>
    public void ResetForNewDay(DateOnly newDate)
    {
        if (newDate <= BusinessDate)
        {
            // Edge case: yêu cầu reset trùng/ngược ngày, bỏ qua để tránh làm sai dữ liệu đã chốt.
            return;
        }

        UsedToday = 0;
        BusinessDate = newDate;
        // Đặt lại quota ngày mới sau khi xác nhận đã rollover sang ngày tiếp theo.
    }
}
