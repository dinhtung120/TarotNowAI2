/*
 * ===================================================================
 * FILE: RefreshTokenConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Khóa DB Trữ Phiếu Phát Vé "Refresh Token" Cho Thằng User Ôm Thay Phiếu Login Nhằm Lấy Tiếp Cục "JWT Token" Rẽ Vô Mạng Mà Ko Bắt Gõ Cộc PassWord. 
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Dao Cột Bảng Lưu Phiếu Đổi Dài Hạn (Refresh Token).
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        
        builder.HasKey(rt => rt.Id);

        // Lọng Rích Băm Chuỗi Token Rất Cứng Mềm Tối Đa Dài Còng Cho Mã Hóa Băm 2 Đoạn Token Chuẩn 
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        // Kẹp Cột Này Thành Đỉnh Cột Index Unique Để Khách Ném Refresh Token Lên Refresh API Là Chọt Đúng Lỗ Tìm Ra Nằm Dó Không Dupe Lặp Khóa Lỗi Nhảy Hỏng Văng DB Tụi Server Ác Lên Load Xoáy DB.
        builder.HasIndex(rt => rt.Token).IsUnique();

        // Gom Dấu Rắn Chứa Lọc Max Length IPv6 String Độ Cong Kéo Đo Ác
        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(45); // IPv6 max length

        // Ném Thần Auto Clock Tự Sinh Giờ Lỗ DB Trám 
        builder.Property(rt => rt.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Một User Trải Cò Kéo Vô Đã Sinh Dày Chục Cục Refresh Token Do Log Lắm Lần/Nhiều Dế Laptop, Lúc Cắt Cổ User Thì Quét Hủy Thuân Sạch Mọi Token Quăng DB Vớt DB Sạch Chống Kền Cáp Lưu Xót.
        builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
