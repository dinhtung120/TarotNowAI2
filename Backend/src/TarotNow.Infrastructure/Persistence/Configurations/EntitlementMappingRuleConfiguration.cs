

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity EntitlementMappingRule.
public class EntitlementMappingRuleConfiguration : IEntityTypeConfiguration<EntitlementMappingRule>
{
    /// <summary>
    /// Cấu hình mapping bảng entitlement_mapping_rules.
    /// Luồng xử lý: map source/target/ratio, tạo unique index source-target và default created_at.
    /// </summary>
    public void Configure(EntityTypeBuilder<EntitlementMappingRule> builder)
    {
        builder.ToTable("entitlement_mapping_rules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TargetKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Ratio)
            .HasPrecision(18, 4);
        // Dùng precision cố định để tránh sai số quy đổi entitlement.

        builder.HasIndex(x => new { x.SourceKey, x.TargetKey })
            .IsUnique()
            .HasDatabaseName("IX_mapping_rules_source_target");
        // Chặn trùng rule ánh xạ theo cặp source-target.

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
