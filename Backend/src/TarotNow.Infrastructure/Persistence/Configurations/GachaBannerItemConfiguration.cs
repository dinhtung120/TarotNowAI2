/*
 * ===================================================================
 * FILE: GachaBannerItemConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class GachaBannerItemConfiguration : IEntityTypeConfiguration<GachaBannerItem>
{
    public void Configure(EntityTypeBuilder<GachaBannerItem> builder)
    {
        builder.ToTable("gacha_banner_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rarity).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardValue).IsRequired().HasMaxLength(256);
        
        builder.Property(x => x.WeightBasisPoints).IsRequired();
        
        builder.Property(x => x.DisplayNameVi).IsRequired().HasMaxLength(256);
        builder.Property(x => x.DisplayNameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.DisplayIcon).HasMaxLength(512);

        builder.HasIndex(x => x.BannerId);
    }
}
