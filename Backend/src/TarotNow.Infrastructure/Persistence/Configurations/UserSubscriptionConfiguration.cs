/*
 * ===================================================================
 * FILE: UserSubscriptionConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cấu hình bảng "user_subscriptions" lưu trạng thái việc Người Dùng Sở Hữu Gói.
 *   Chú trọng Index nhiều nhất vì hàm tìm kiếm "Gói nào còn Hạn" hoặc "Gói nào Hết Hạn Cần Queets" sẽ chạy rất thường xuyên.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.ToTable("user_subscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);

        // Ràng buộc Foreign Key với Bảng SubscriptionPlans
        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Gói Gốc nếu đang có Khách Hàng xài

        // Ràng buộc Khóa Ngoại trỏ về khách (User)
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Nếu Admin xóa tài khoản Khách → Xóa luôn đống Biên Lai.

        // ==== CÁC CHỈ MỤC INDEX ====
        // 1. Chống Mua Trùng bằng IdempotencyKey (Rất quan trọng - Prevent Double Charge Stripe)
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("IX_user_subscriptions_idempotency_key");

        // 2. Tối ưu Truy vấn Client Fetch List "Gói Đang Gắn Trên Người": Where(UserId == X && Status == "active")
        builder.HasIndex(x => new { x.UserId, x.Status })
            .HasDatabaseName("IX_user_subscriptions_user_id_status");
            
        // 3. Tối ưu Quét định kì của Job System Background: Where(Status == "active" && EndDate <= UtcNow)
        builder.HasIndex(x => new { x.Status, x.EndDate })
            .HasDatabaseName("IX_user_subscriptions_status_end_date");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
