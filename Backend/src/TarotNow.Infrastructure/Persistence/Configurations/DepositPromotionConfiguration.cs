using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class DepositPromotionConfiguration : IEntityTypeConfiguration<DepositPromotion>
{
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
    }
}
