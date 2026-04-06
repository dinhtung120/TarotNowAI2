/*
 * ===================================================================
 * FILE: GachaRewardLogConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class GachaRewardLogConfiguration : IEntityTypeConfiguration<GachaRewardLog>
{
    public void Configure(EntityTypeBuilder<GachaRewardLog> builder)
    {
        builder.ToTable("gacha_reward_logs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.BannerId).IsRequired();
        builder.Property(x => x.BannerItemId).IsRequired();
        
        builder.Property(x => x.OddsVersion).IsRequired().HasMaxLength(32);
        builder.Property(x => x.SpentDiamond).IsRequired();
        
        builder.Property(x => x.Rarity).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardValue).IsRequired().HasMaxLength(256);
        
        builder.Property(x => x.PityCountAtSpin).IsRequired();
        builder.Property(x => x.WasPityTriggered).IsRequired();
        
        builder.Property(x => x.RngSeed).HasMaxLength(128);
        
        builder.Property(x => x.IdempotencyKey).IsRequired().HasMaxLength(128);
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();

        builder.HasIndex(x => new { x.UserId, x.CreatedAt }).IsDescending(false, true);
        
        // Cụm Index tối ưu đếm Pity (Count) theo Rarity Legendary và Filter CreatedAt cực mượt.
        builder.HasIndex(x => new { x.UserId, x.BannerId, x.Rarity, x.CreatedAt });
    }
}
