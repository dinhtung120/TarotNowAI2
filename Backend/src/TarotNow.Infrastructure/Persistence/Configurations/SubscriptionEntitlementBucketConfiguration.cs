

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity SubscriptionEntitlementBucket.
public class SubscriptionEntitlementBucketConfiguration : IEntityTypeConfiguration<SubscriptionEntitlementBucket>
{
    /// <summary>
    /// Cấu hình mapping bảng subscription_entitlement_buckets.
    /// Luồng xử lý: map cột key, quan hệ subscription và các index phục vụ consume/reset theo ngày.
    /// </summary>
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
            .OnDelete(DeleteBehavior.Cascade);
        // Xóa subscription sẽ xóa bucket entitlement đi kèm.

        builder.HasIndex(x => new { x.UserId, x.EntitlementKey, x.BusinessDate, x.SubscriptionEndDate })
            .HasDatabaseName("IX_buckets_user_key_date");
        // Index chính cho truy vấn consume bucket theo user/key/ngày.

        builder.HasIndex(x => x.BusinessDate)
            .HasDatabaseName("IX_buckets_business_date");
        // Index phục vụ job reset quota theo ngày.

        builder.HasIndex(x => x.UserSubscriptionId)
            .HasDatabaseName("IX_buckets_subscription_id");
        // Index phục vụ truy vấn bucket theo subscription.
    }
}
