using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho entity <see cref="GachaPool"/>.
/// </summary>
public sealed class GachaPoolConfiguration : IEntityTypeConfiguration<GachaPool>
{
    /// <summary>
    /// Cấu hình schema cho bảng gacha_pools.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaPool> builder)
    {
        builder.ToTable(
            "gacha_pools",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pools_cost_amount_positive",
                    "\"cost_amount\" > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pools_hard_pity_non_negative",
                    "\"hard_pity_count\" >= 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pools_hard_pity_when_enabled",
                    "(\"pity_enabled\" = false AND \"hard_pity_count\" = 0) OR (\"pity_enabled\" = true AND \"hard_pity_count\" > 0)");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
        builder.Property(x => x.PoolType).IsRequired().HasMaxLength(32);

        builder.Property(x => x.NameVi).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameZh).IsRequired().HasMaxLength(256);

        builder.Property(x => x.DescriptionVi).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.DescriptionEn).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.DescriptionZh).IsRequired().HasMaxLength(1024);

        builder.Property(x => x.CostCurrency).IsRequired().HasMaxLength(32);
        builder.Property(x => x.CostAmount).IsRequired();
        builder.Property(x => x.OddsVersion).IsRequired().HasMaxLength(32);

        builder.Property(x => x.PityEnabled).IsRequired();
        builder.Property(x => x.HardPityCount).IsRequired();

        builder.Property(x => x.EffectiveFrom).IsRequired();
        builder.Property(x => x.EffectiveTo);
        builder.Property(x => x.IsActive).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => new { x.EffectiveFrom, x.EffectiveTo });
    }
}
