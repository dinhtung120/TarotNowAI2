using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình bảng users trong PostgreSQL bằng FluentAPI.
///
/// LƯU Ý QUAN TRỌNG:
/// Tên cột trong bảng PostgreSQL sử dụng quy ước snake_case (ví dụ: user_level),
/// trong khi Entity dùng PascalCase (ví dụ: Level).
/// EF Core mặc định map Property sang lowercase (Level → level),
/// nhưng DB schema dùng prefix "user_" cho Level và Exp.
/// Vì vậy PHẢI khai báo HasColumnName() rõ ràng.
///
/// REFACTORED: User.Wallet (UserWallet) được cấu hình dưới dạng Owned Entity.
/// Các cột gold_balance, diamond_balance, frozen_diamond_balance, total_diamonds_purchased 
/// vẫn nằm trong bảng "users", nhưng logic code thuộc class UserWallet riêng biệt.
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

        /*
         * Level và Exp: DB dùng tên user_level và user_exp (có prefix "user_")
         * để tránh trùng tên reserved keyword. EF mặc định sẽ map thành
         * "level" và "exp" (lowercase) → gây lỗi "column level does not exist".
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

        builder.Property(u => u.MfaBackupCodesHashJson)
            .HasColumnName("mfa_backup_codes_hash_json")
            .HasColumnType("jsonb");

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        /*
         * ====================================================================
         * OWNED ENTITY: UserWallet
         * Cấu hình EF Core Owned Entity pattern cho UserWallet.
         *
         * Tại sao dùng OwnsOne thay vì bảng riêng?
         * → Wallet data luôn đi kèm với User (1:1, không có vòng đời độc lập).
         * → Giữ nguyên schema DB hiện tại (các cột wallet nằm trong bảng users).
         * → Hiệu năng tốt hơn: không cần JOIN khi load User.
         *
         * HasColumnName: map tên property PascalCase → snake_case trong DB.
         * ====================================================================
         */
        builder.OwnsOne(u => u.Wallet, wallet =>
        {
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
         * BACKWARD COMPATIBILITY: Ignore các computed properties delegate
         * từ User (GoldBalance, DiamondBalance, FrozenDiamondBalance, TotalDiamondsPurchased).
         * Những property này chỉ là getter delegate sang Wallet.*,
         * nếu không Ignore, EF Core sẽ cố map thêm 4 cột trùng lặp.
         */
        builder.Ignore(u => u.GoldBalance);
        builder.Ignore(u => u.DiamondBalance);
        builder.Ignore(u => u.FrozenDiamondBalance);
        builder.Ignore(u => u.TotalDiamondsPurchased);
    }
}
