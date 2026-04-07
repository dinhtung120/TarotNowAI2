

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class EntitlementMappingRuleConfiguration : IEntityTypeConfiguration<EntitlementMappingRule>
{
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

        
        builder.HasIndex(x => new { x.SourceKey, x.TargetKey })
            .IsUnique()
            .HasDatabaseName("IX_mapping_rules_source_target");
            
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
