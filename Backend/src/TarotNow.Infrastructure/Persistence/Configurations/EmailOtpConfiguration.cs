

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity EmailOtp.
public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
{
    /// <summary>
    /// Cấu hình mapping bảng email_otps.
    /// Luồng xử lý: map bảng/key, ràng buộc otp/type/time và index phục vụ truy vấn OTP active.
    /// </summary>
    public void Configure(EntityTypeBuilder<EmailOtp> builder)
    {
        builder.ToTable("email_otps");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.OtpCode)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(e => e.User)
               .WithMany()
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        // Cascade delete để dọn OTP khi user bị xóa.

        builder.Property(e => e.Type)
            .HasMaxLength(20);

        builder.HasIndex(e => new { e.UserId, e.Type, e.IsUsed, e.ExpiresAt });
        // Index tối ưu truy vấn OTP mới nhất theo user/type/trạng thái sử dụng/hết hạn.
    }
}
