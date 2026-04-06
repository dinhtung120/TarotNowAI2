/*
 * ===================================================================
 * FILE: GachaBannerConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class GachaBannerConfiguration : IEntityTypeConfiguration<GachaBanner>
{
    public void Configure(EntityTypeBuilder<GachaBanner> builder)
    {
        builder.ToTable("gacha_banners");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.NameVi).IsRequired().HasMaxLength(256);
        builder.Property(x => x.NameEn).IsRequired().HasMaxLength(256);
        builder.Property(x => x.DescriptionVi).HasMaxLength(1024);
        builder.Property(x => x.DescriptionEn).HasMaxLength(1024);

        builder.Property(x => x.CostDiamond).IsRequired();
        builder.Property(x => x.OddsVersion).IsRequired().HasMaxLength(32);
        
        builder.Property(x => x.EffectiveFrom).IsRequired();
        builder.Property(x => x.EffectiveTo);
        
        builder.Property(x => x.PityEnabled).IsRequired();
        builder.Property(x => x.HardPityCount).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => x.IsActive);

        builder.HasMany(x => x.Items)
               .WithOne(x => x.Banner)
               .HasForeignKey(x => x.BannerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
