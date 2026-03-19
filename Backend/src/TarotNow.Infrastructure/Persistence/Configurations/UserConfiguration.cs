/*
 * ===================================================================
 * FILE: UserConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Ánh Xạ Thiết Lập Bảng Cốt Lõi `users`. Bảng Này Rất Phức Tạp Vì Gánh Cả Sinh Mệnh Bệnh Án Financial Của Hệ Thống (Owned Entity UserWallet Cắm Vào Đây Luôn).
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Trái Tim Gắn Kết User Với Database PostgreSQL Bằng Kiểu Xây FluentAPI.
/// Dịch Theo Cấu Trúc Khác Của C# Sang DB Bằng Tay Tránh Tuột Nhầm Dây.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Phủ Ném Bảng Lên Tên Root
        builder.ToTable("users"); 
        
        builder.HasKey(u => u.Id);

        // Kẹp Kích Cỡ Max Email Cữ Bơm Quá 255 DB Lủng Lỗ Hổng 1000 Chữ Giết Server Khác Dịch.
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        // Chặn Email Sinh Đôi 1 Email Khóa Mũi Tạo 2 Account Ở Postgre
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
            
        // Username Cũng Không Nhận Sinh Đôi Dùng Chung 
        builder.HasIndex(u => u.Username).IsUnique();

        // Kẹp Ráp Băm Chuỗi Mật Khẩu (Ai Cũng Không Đọc Được Ngay Cả Tụi Admin).
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.DisplayName)
            .HasMaxLength(100);

        builder.Property(u => u.DateOfBirth)
            .IsRequired();

        // Phủi Loại Bỏ Trường Logic Tính Toán Get Của C# "HasConsented" Đi. Không Đem Map Ném Mất Công Nó Sinh Thêm 1 Cột Tên Là HasConsented Ở Bảng Nhúng Rác DB . Dưới Dữ Liệu Gọi Thật Mới Có Trong UserConsent.
        builder.Ignore(u => u.HasConsented);

        builder.Property(u => u.Role)
            .HasMaxLength(20);

        builder.Property(u => u.ReaderStatus)
            .HasMaxLength(20);

        /*
         * Đẩy Nép Bẻ Thước Map (Cột Chịu Từ Chối Default Convert EF)
         * Cấp Độ Và Kinh Nghiệm Lưu Bằng Prefix "user_" Vì Trong C# Chỉ Cần Đặt 'Level'. Nếu DB Cứ Hút "level" Xuống PostgreSQL Là Die Bởi "level" Nghe Ghẻ Nên Sợ Dấu Chữ Keywords Rớt Từ Của Hệ Cỗ Máy DB Engine SQL.
         */
        builder.Property(u => u.Level)
            .HasColumnName("user_level")
            .HasDefaultValue(1);

        builder.Property(u => u.Exp)
            .HasColumnName("user_exp")
            .HasDefaultValue(0);

        builder.Property(u => u.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("pending");

        // Kiểu Bảng Mỏng Gói JSON Nghẽn Mảng OTP Lưu Giản DB SQL Nuốt Mã JSON Tắt List List<string> Xé Gọn Gàng Của C#.
        builder.Property(u => u.MfaBackupCodesHashJson)
            .HasColumnName("mfa_backup_codes_hash_json")
            .HasColumnType("jsonb");

        // Giờ Gắn Nhảy Mặt Lóc Gọi Băng Auto Insert Ngày
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        /*
         * ====================================================================
         * LUẬT ĐÈ BẢN ĐỘ OWNED ENTITY NHẠY CẢM MÓC NỐI: "UserWallet" Trong Gốc Ruột User
         * - Thay Vì Đẻ Bảng Tách Ví "user_wallets" Cồng Kềnh Phải Joint Nối Dây (Chậm Chạp Lúc Rút Query Gacha Tiền Lương Tức Bức)
         * - Tại Domain Tách Để Cắt Domain Cho Gọn SRP, DB Thiết Bảng Nó Ép Gom Chung Các Field Nó Là Property Sở Hữu Của Bảng User (Khách Nhập Bỏ Thẳng Túi Đóng Áo DB 1 Rổ Hốc Đi Thôi Dài Cửa Cho SQL Truy Cho Nhanh Nổ Mạng Đốt Transaction Cao Kịp).
         * ====================================================================
         */
        builder.OwnsOne(u => u.Wallet, wallet =>
        {
            // Ép Én Cữ Tên (Ép 1 Cục Map Sang Cột Riêng Của User Thôi Nhưng Logic Thằng Wallet Bên Domain Nó Ôm Hết)
            wallet.Property(w => w.GoldBalance)
                .HasColumnName("gold_balance")
                .HasDefaultValue(0L);

            wallet.Property(w => w.DiamondBalance)
                .HasColumnName("diamond_balance")
                .HasDefaultValue(0L);

            wallet.Property(w => w.FrozenDiamondBalance)
                .HasColumnName("frozen_diamond_balance")
                .HasDefaultValue(0L);

            wallet.Property(w => w.TotalDiamondsPurchased)
                .HasColumnName("total_diamonds_purchased")
                .HasDefaultValue(0L);
        });

        /*
         * HỦY MẠP BỌN GIẢ CẬY (Getter Vượt Tường Code Phế Xưa):
         * Lệ Bôi Mác Thằng EF Níu Ngốc Náo Nghĩ Đây Bọn Code Cót Lòi Property (Vì 4 Cái Này Gọi Núp Thằng Getter Mò Từ Thằng `.Wallet.`) Dẫn Tới EF Giăng Mắn Cần Bơm Nạp 4 Cột Nữa Sai Rỗng Trùng Hoàn Toàn 4 Code Lõi Của Ví Đã Thầu Ở Lưới OwnsOne).
         * Ép Không Map (Ignore) Để Thằng SQL Bọc Tít Mũ Không Đọc Và Không Sinh Thêm Cột Kèm Trong User Lữa.
         */
        builder.Ignore(u => u.GoldBalance);
        builder.Ignore(u => u.DiamondBalance);
        builder.Ignore(u => u.FrozenDiamondBalance);
        builder.Ignore(u => u.TotalDiamondsPurchased);
    }
}
