

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("subscription_plans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        
        
        builder.Property(x => x.EntitlementsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        
        builder.HasIndex(x => x.IsActive)
            .HasFilter("is_active = true") 
            .HasDatabaseName("IX_subscription_plans_is_active");
            
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
