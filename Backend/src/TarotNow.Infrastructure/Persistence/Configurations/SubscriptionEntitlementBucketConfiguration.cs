

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
            .OnDelete(DeleteBehavior.Cascade); 

        
        
        
        
        
        builder.HasIndex(x => new { x.UserId, x.EntitlementKey, x.BusinessDate, x.SubscriptionEndDate })
            .HasDatabaseName("IX_buckets_user_key_date");

        
        
        builder.HasIndex(x => x.BusinessDate)
            .HasDatabaseName("IX_buckets_business_date");
            
        
        builder.HasIndex(x => x.UserSubscriptionId)
            .HasDatabaseName("IX_buckets_subscription_id");
    }
}
