

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        
        builder.HasKey(rt => rt.Id);

        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        
        builder.HasIndex(rt => rt.Token).IsUnique();

        
        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(45); 

        
        builder.Property(rt => rt.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        
        builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
