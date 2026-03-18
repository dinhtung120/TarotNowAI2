using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
{
    public void Configure(EntityTypeBuilder<EmailOtp> builder)
    {
        builder.ToTable("email_otps");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OtpCode)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Một User có thể yêu cầu nhiều OTP
        builder.HasOne(e => e.User)
               .WithMany()
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.Property(e => e.Type)

            .HasMaxLength(20);

        // Index để tăng tốc độ tìm kiếm OTP mới nhất, chưa sử dụng của 1 User
        builder.HasIndex(e => new { e.UserId, e.Type, e.IsUsed, e.ExpiresAt });
    }
}
