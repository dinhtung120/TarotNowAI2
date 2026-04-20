using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity DepositPromotion.
public class DepositPromotionConfiguration : IEntityTypeConfiguration<DepositPromotion>
{
    /// <summary>
    /// Cấu hình mapping bảng deposit_promotions.
    /// </summary>
    public void Configure(EntityTypeBuilder<DepositPromotion> builder)
    {
        builder.ToTable("deposit_promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.MinAmountVnd)
            .HasColumnName("min_amount_vnd")
            .IsRequired();

        builder.Property(p => p.BonusGold)
            .HasColumnName("bonus_gold")
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(p => p.MinAmountVnd)
            .HasDatabaseName("ix_deposit_promotions_min_amount_vnd");
    }
}
