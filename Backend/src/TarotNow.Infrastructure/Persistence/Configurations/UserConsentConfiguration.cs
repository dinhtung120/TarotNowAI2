

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class UserConsentConfiguration : IEntityTypeConfiguration<UserConsent>
{
    public void Configure(EntityTypeBuilder<UserConsent> builder)
    {
        
        builder.ToTable("user_consents");

        
        builder.HasKey(e => e.Id);

        
        builder.Property(e => e.DocumentType)
            .IsRequired()
            .HasMaxLength(50);

        
        builder.Property(e => e.Version)
            .IsRequired()
            .HasMaxLength(20);

        
        builder.Property(e => e.IpAddress)
            .HasMaxLength(45); 

        
        builder.Property(e => e.UserAgent)
            .HasMaxLength(500);

        
        builder.HasIndex(e => new { e.UserId, e.DocumentType, e.Version })
            .IsUnique();

        
        builder.HasOne(e => e.User)
            .WithMany(u => u.Consents)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
