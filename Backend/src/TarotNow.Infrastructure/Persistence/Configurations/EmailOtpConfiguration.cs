/*
 * ===================================================================
 * FILE: EmailOtpConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cấu Hình Gương Bảng Lọc Mã Code OTP Gửi Qua Email.
 *   Xoáy Tìm Nhanh Để Kiểm Tra Hạn Code Của Khách / Check Null Hạn Lôi Cuốn Ngay Vào SQL Engine Thay Vì C# LINQ Nặng Server.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Niềng Răng Rác Lọc Bảng EmailOtp Lưu Mã Số Rơi Rụng Rác Thảo Của Hệ Thống Xác Thực.
/// </summary>
public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
{
    public void Configure(EntityTypeBuilder<EmailOtp> builder)
    {
        // Phủ Tên Chữ Bảng Thể Thuần Chế Đuôi Ngắn Dọn
        builder.ToTable("email_otps");
        
        // Pk Root
        builder.HasKey(e => e.Id);

        // Kẹp Mã Mã Hóa Số OTP Chống Khóa Rỗng Lỗi Vặn Chuỗi Dễ Parse (Dài 128 Do Mã Được Băm Hash Bảo Mật Chặn Leak Lồ Server).
        builder.Property(e => e.OtpCode)
            .IsRequired()
            .HasMaxLength(128);

        // Quất Giờ Khởi Động Tự Lặp Hàm SQL CURRENT_TIMESTAMP Nếu Áp Ẩn Kịp 
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Móc Nối Xóa Dấu Tích: Một Con User Đẻ Nhiều OTP Ra, Xóa Acc Thì Rớt Cả Bảng Của Nó Cho Rạch Lọc Đỡ Kẹt Tắc Kho.
        builder.HasOne(e => e.User)
               .WithMany()
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.Property(e => e.Type)
            .HasMaxLength(20); // Ép Loại Kiểu Rác Đừng Sinh Dài Tốn Sổ DB (ResetPassWord, VerifyEmail).

        // Cắn Kìm Đỉnh Của Chóp (Composite Index Cực Gắt Quét Tốc Độ Tức Chớp Át Phép Thử Where (UserId && Type && IsUsed && ExpiresAt)). Tránh Khách Check OTP Load Lâu Mà Cháy Database Rút Scan Chậm Do Cả Triệu User OTP Rác Trong Bảng. Thụt Phát DB Móc Lên Ngay Đúng 1 Còng Index.
        builder.HasIndex(e => new { e.UserId, e.Type, e.IsUsed, e.ExpiresAt });
    }
}
