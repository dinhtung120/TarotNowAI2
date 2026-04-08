

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity GachaBannerItem.
public class GachaBannerItemConfiguration : IEntityTypeConfiguration<GachaBannerItem>
{
    /// <summary>
    /// Cấu hình mapping bảng gacha_banner_items.
    /// Luồng xử lý: map các cột rarity/reward/weight/display và index theo banner_id.
    /// </summary>
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
        // Index banner_id tối ưu truy vấn item theo từng banner.
    }
}
