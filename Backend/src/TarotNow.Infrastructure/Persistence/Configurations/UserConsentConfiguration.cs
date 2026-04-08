

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity UserConsent.
public class UserConsentConfiguration : IEntityTypeConfiguration<UserConsent>
{
    /// <summary>
    /// Cấu hình mapping bảng user_consents.
    /// Luồng xử lý: map cột pháp lý, tạo unique index chống ghi trùng phiên bản consent và thiết lập quan hệ User.
    /// </summary>
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
        // Giới hạn 45 ký tự để hỗ trợ cả IPv4/IPv6 mà không phình kích thước cột.

        builder.Property(e => e.UserAgent)
            .HasMaxLength(500);

        builder.HasIndex(e => new { e.UserId, e.DocumentType, e.Version })
            .IsUnique();
        // Chặn ghi trùng một loại tài liệu + phiên bản cho cùng user để audit pháp lý nhất quán.

        builder.HasOne(e => e.User)
            .WithMany(u => u.Consents)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // Khi user bị xóa thì consent liên quan phải bị dọn theo để tránh orphan record.
    }
}
