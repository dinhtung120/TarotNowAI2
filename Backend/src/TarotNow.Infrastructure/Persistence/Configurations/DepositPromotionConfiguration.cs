

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity DepositPromotion.
public class DepositPromotionConfiguration : IEntityTypeConfiguration<DepositPromotion>
{
    /// <summary>
    /// Cấu hình mapping bảng deposit_promotions.
    /// Luồng xử lý: map bảng/key, cột ngưỡng thưởng và index min amount phục vụ tra cứu.
    /// </summary>
    public void Configure(EntityTypeBuilder<DepositPromotion> builder)
    {
        builder.ToTable("deposit_promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.MinAmountVnd)
            .IsRequired();

        builder.Property(p => p.BonusDiamond)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.HasIndex(p => p.MinAmountVnd);
        // Index ngưỡng nạp giúp tìm promotion phù hợp nhanh hơn.
    }
}
