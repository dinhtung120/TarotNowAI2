

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity SubscriptionPlan.
public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    /// <summary>
    /// Cấu hình mapping bảng subscription_plans.
    /// Luồng xử lý: map cột thông tin gói, cột entitlements JSON và index lọc gói active.
    /// </summary>
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
        // Dùng jsonb để lưu entitlement linh hoạt theo từng gói.

        builder.HasIndex(x => x.IsActive)
            .HasFilter("is_active = true")
            .HasDatabaseName("IX_subscription_plans_is_active");
        // Index có filter để tối ưu truy vấn danh sách gói đang active.

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
