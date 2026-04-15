using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho entity <see cref="GachaPullRewardLog"/>.
/// </summary>
public sealed class GachaPullRewardLogConfiguration : IEntityTypeConfiguration<GachaPullRewardLog>
{
    /// <summary>
    /// Cấu hình schema cho bảng gacha_pull_reward_logs.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaPullRewardLog> builder)
    {
        builder.ToTable(
            "gacha_pull_reward_logs",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_reward_logs_quantity_positive",
                    "\"quantity_granted\" > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_reward_logs_pity_non_negative",
                    "\"pity_count_at_reward\" >= 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_reward_logs_kind_item",
                    "\"reward_kind\" <> 'item' OR (\"item_definition_id\" IS NOT NULL AND \"item_code\" IS NOT NULL AND \"currency\" IS NULL AND \"amount\" IS NULL)");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_reward_logs_kind_currency",
                    "\"reward_kind\" <> 'currency' OR (\"item_definition_id\" IS NULL AND \"item_code\" IS NULL AND \"currency\" IS NOT NULL AND \"amount\" > 0)");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PullOperationId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PoolId).IsRequired();
        builder.Property(x => x.PoolCode).IsRequired().HasMaxLength(64);

        builder.Property(x => x.RewardRateId).IsRequired();
        builder.Property(x => x.RewardKind).IsRequired().HasMaxLength(32);
        builder.Property(x => x.Rarity).IsRequired().HasMaxLength(32);

        builder.Property(x => x.ItemCode).HasMaxLength(64);
        builder.Property(x => x.ItemDefinitionId);
        builder.Property(x => x.Currency).HasMaxLength(32);
        builder.Property(x => x.Amount);
        builder.Property(x => x.QuantityGranted).IsRequired();

        builder.Property(x => x.IconUrl).HasMaxLength(500);
        builder.Property(x => x.NameVi).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameZh).IsRequired().HasMaxLength(256);

        builder.Property(x => x.IsHardPityReward).IsRequired();
        builder.Property(x => x.PityCountAtReward).IsRequired();
        builder.Property(x => x.RngSeed).HasMaxLength(128);
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.HasIndex(x => x.PullOperationId);
        builder.HasIndex(x => new { x.UserId, x.PoolId, x.CreatedAtUtc }).IsDescending(false, false, true);
        builder.HasIndex(x => new { x.UserId, x.PoolId, x.Rarity, x.CreatedAtUtc }).IsDescending(false, false, false, true);
    }
}
