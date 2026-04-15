using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho entity <see cref="GachaPoolRewardRate"/>.
/// </summary>
public sealed class GachaPoolRewardRateConfiguration : IEntityTypeConfiguration<GachaPoolRewardRate>
{
    /// <summary>
    /// Cấu hình schema cho bảng gacha_pool_reward_rates.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaPoolRewardRate> builder)
    {
        builder.ToTable(
            "gacha_pool_reward_rates",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pool_reward_rates_probability_positive",
                    "\"probability_basis_points\" > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pool_reward_rates_quantity_positive",
                    "\"quantity_granted\" > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pool_reward_rates_kind_item",
                    "\"reward_kind\" <> 'item' OR (\"item_definition_id\" IS NOT NULL AND \"currency\" IS NULL AND \"amount\" IS NULL)");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pool_reward_rates_kind_currency",
                    "\"reward_kind\" <> 'currency' OR (\"item_definition_id\" IS NULL AND \"currency\" IS NOT NULL AND \"amount\" > 0)");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PoolId).IsRequired();
        builder.Property(x => x.RewardKind).IsRequired().HasMaxLength(32);
        builder.Property(x => x.Rarity).IsRequired().HasMaxLength(32);
        builder.Property(x => x.ProbabilityBasisPoints).IsRequired();

        builder.Property(x => x.ItemDefinitionId);
        builder.Property(x => x.Currency).HasMaxLength(32);
        builder.Property(x => x.Amount);
        builder.Property(x => x.QuantityGranted).IsRequired();

        builder.Property(x => x.IconUrl).HasMaxLength(500);
        builder.Property(x => x.NameVi).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameZh).IsRequired().HasMaxLength(256);
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasOne(x => x.Pool)
            .WithMany()
            .HasForeignKey(x => x.PoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PoolId, x.Rarity });
        builder.HasIndex(x => new { x.PoolId, x.IsActive });
    }
}
