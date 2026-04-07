

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class DepositOrderConfiguration : IEntityTypeConfiguration<DepositOrder>
{
    public void Configure(EntityTypeBuilder<DepositOrder> builder)
    {
        
        builder.ToTable("deposit_orders");

        
        builder.HasKey(o => o.Id);

        
        builder.Property(o => o.AmountVnd)
            .IsRequired();

        
        builder.Property(o => o.DiamondAmount)
            .IsRequired();

        
        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(20);

        
        builder.Property(o => o.TransactionId)
            .HasMaxLength(100);

        
        builder.Property(o => o.FxSnapshot)
            .HasMaxLength(1000);

        
        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
            
        
        builder.HasIndex(o => o.TransactionId).IsUnique();
        
        
        builder.HasIndex(o => o.Status);
    }
}
