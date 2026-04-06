/*
 * ===================================================================
 * FILE: SubscriptionEntitlementBucketConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bảng Nhạy Cảm - Đây là rổ đựng (Bucket) số lần Quyền lợi được sử dụng.
 *   Xảy ra thao tác Ghi (Update) liên tục Cực Kì Dày Đặc. Cần thiết kế Index rất cẩn thận dùng cho Row Lock của thuật toán Consume Earliest-Expiry-First.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class SubscriptionEntitlementBucketConfiguration : IEntityTypeConfiguration<SubscriptionEntitlementBucket>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntitlementBucket> builder)
    {
        builder.ToTable("subscription_entitlement_buckets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntitlementKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(x => x.UserSubscription)
            .WithMany()
            .HasForeignKey(x => x.UserSubscriptionId)
            .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Gói Hủy → Quăng luôn các rổ quyền lợi con đi kèm.

        // ==== CÁC CHỈ MỤC INDEX QUAN TRỌNG ====
        
        // Cần Nhất Để Tìm Bucket Phù Hợp Khi Bấm Rút Bói.
        // WHERE (UserId == X) AND (EntitlementKey == Y) AND (BusinessDate == Hôm_Nay) AND (UsedToday < DailyQuota)
        // OrderBy SubscriptionEndDate ASC (Sớm hạn trừ trước).
        builder.HasIndex(x => new { x.UserId, x.EntitlementKey, x.BusinessDate, x.SubscriptionEndDate })
            .HasDatabaseName("IX_buckets_user_key_date");

        // Hỗ trợ Job Quét Cập Nhật Quyền Lợi Hàng Ngày Lúc 0h Sáng UTC Đổ Reset rổ (UsedToDay = 0).
        // Phải tìm nhanh Where(BusinessDate < TodayUTC)
        builder.HasIndex(x => x.BusinessDate)
            .HasDatabaseName("IX_buckets_business_date");
            
        // Index để lúc Delete/Cascade FK không bị Full Table Scan
        builder.HasIndex(x => x.UserSubscriptionId)
            .HasDatabaseName("IX_buckets_subscription_id");
    }
}
