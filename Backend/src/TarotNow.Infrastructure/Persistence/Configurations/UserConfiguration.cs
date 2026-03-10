using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình bảng users trong PostgreSQL bằng FluentAPI.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users"); // Match với bảng schema.sql
        
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.DisplayName)
            .HasMaxLength(100);

        builder.Property(u => u.DateOfBirth)
            .IsRequired();

        builder.Ignore(u => u.HasConsented);

        builder.Property(u => u.Role)

            .HasMaxLength(20);

        builder.Property(u => u.ReaderStatus)

            .HasMaxLength(20);

        builder.Property(u => u.Level)
            .HasDefaultValue(1);

        builder.Property(u => u.Exp)
            .HasDefaultValue(0);

        builder.Property(u => u.Status)

            .HasMaxLength(20);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
