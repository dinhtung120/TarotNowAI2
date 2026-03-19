/*
 * ===================================================================
 * FILE: UserConsentConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Sổ DB Xoay Khóa Cột Bằng Chữ Ký Pháp Lý Của Khách Tránh Việc Hủy Án (Giam IP Kêu Oan Ko Xin Mở User).
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Dao Cột Cho Bảng Document Legal Thư Pháp Chữ Kí Gửi Tường Kí Của Khách (TOS/Privacy).
/// </summary>
public class UserConsentConfiguration : IEntityTypeConfiguration<UserConsent>
{
    public void Configure(EntityTypeBuilder<UserConsent> builder)
    {
        // Phủ Trống Quét Bảng user_consents
        builder.ToTable("user_consents");

        // PK Hạt 1
        builder.HasKey(e => e.Id);

        // Văn Pháp ToS hay Điều Khoản Riêng Tư (Policy) Không Rỗng.
        builder.Property(e => e.DocumentType)
            .IsRequired()
            .HasMaxLength(50);

        // Mũi Đóng Áp Đít Version Gì Cũng Bắt Chặn Bản Luật Xài Chặt VD (v1.0.5) Nên Nếp Bắt Cứ Cứng Nút Tranh Cãi Kí Luật Hụt.
        builder.Property(e => e.Version)
            .IsRequired()
            .HasMaxLength(20);

        // Mã Quét Lường Truy Vết Địa Chỉ Internet Rút Mạng User Bẫy Bot Ký Đuổi
        builder.Property(e => e.IpAddress)
            .HasMaxLength(45); // Support IPv6

        // Mã Rượt Tạp Máy Thiết Bị Khách Gõ Cầm (Chrome/Safari Chặn Hack Mò).
        builder.Property(e => e.UserAgent)
            .HasMaxLength(500);

        // Ngọn Ngang Thắt Chỉ Mã Độc: Không Thể Đồng 1 Lỗ User Cùng Cắm Lập Ký Bản Update ToS v2 Cả Chục Lần Lấy Méo Sổ Khổ Đuọc Đóng. Tránh Spam Database Hủy Lồi Chỗ Méo Report File Tào Lao Rác Thùng Máy Chủ.
        builder.HasIndex(e => new { e.UserId, e.DocumentType, e.Version })
            .IsUnique();

        // Ép Cột Văng Trùng Lặp 
        builder.HasOne(e => e.User)
            .WithMany(u => u.Consents)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Trảm Account Thét Lôi Trảm Mảng Bảng Phá Cũ Giờ Khỏa Méo Đè App Report Cạn Rụng Khúc.
    }
}
