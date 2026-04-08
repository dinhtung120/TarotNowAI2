

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity UserSubscription.
public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    /// <summary>
    /// Cấu hình mapping bảng user_subscriptions.
    /// Luồng xử lý: map trạng thái/idempotency, cấu hình quan hệ user-plan và thêm index cho truy vấn active/expiry.
    /// </summary>
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

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
        // Không cho xóa plan khi còn subscription lịch sử để giữ toàn vẹn dữ liệu kế toán.

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // Xóa user sẽ xóa subscription liên quan theo chính sách dọn dữ liệu hiện tại.

        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("IX_user_subscriptions_idempotency_key");
        // Bảo vệ idempotency khi client retry thao tác subscribe.

        builder.HasIndex(x => new { x.UserId, x.Status })
            .HasDatabaseName("IX_user_subscriptions_user_id_status");
        // Tăng tốc truy vấn subscription theo user và trạng thái đang hoạt động.

        builder.HasIndex(x => new { x.Status, x.EndDate })
            .HasDatabaseName("IX_user_subscriptions_status_end_date");
        // Tối ưu job quét gói sắp/hết hạn theo trạng thái.

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
        // Chuẩn hóa timezone UTC để thống nhất tính toán hạn dùng giữa các môi trường.
    }
}
