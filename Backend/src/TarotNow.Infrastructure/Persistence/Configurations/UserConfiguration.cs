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
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        ConfigureIdentityColumns(builder);
        ConfigureProfileColumns(builder);
        ConfigureStatusColumns(builder);
        ConfigureWalletColumns(builder);
        IgnoreComputedColumns(builder);
    }

    private static void ConfigureIdentityColumns(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
    }

    private static void ConfigureProfileColumns(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.DisplayName).HasMaxLength(100);
        builder.Property(u => u.DateOfBirth).IsRequired();
        builder.Property(u => u.Role).HasMaxLength(20);
        builder.Property(u => u.ReaderStatus).HasMaxLength(20);
        builder.Property(u => u.Level).HasColumnName("user_level").HasDefaultValue(1);
        builder.Property(u => u.Exp).HasColumnName("user_exp").HasDefaultValue(0);
        builder.Ignore(u => u.HasConsented);
    }

    private static void ConfigureStatusColumns(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("pending");

        builder.Property(u => u.MfaBackupCodesHashJson)
            .HasColumnName("mfa_backup_codes_hash_json")
            .HasColumnType("jsonb");

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    private static void ConfigureWalletColumns(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(u => u.Wallet, wallet =>
        {
            wallet.Property(w => w.GoldBalance).HasColumnName("gold_balance").HasDefaultValue(0L);
            wallet.Property(w => w.DiamondBalance).HasColumnName("diamond_balance").HasDefaultValue(0L);
            wallet.Property(w => w.FrozenDiamondBalance).HasColumnName("frozen_diamond_balance").HasDefaultValue(0L);
            wallet.Property(w => w.TotalDiamondsPurchased).HasColumnName("total_diamonds_purchased").HasDefaultValue(0L);
        });
    }

    private static void IgnoreComputedColumns(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(u => u.GoldBalance);
        builder.Ignore(u => u.DiamondBalance);
        builder.Ignore(u => u.FrozenDiamondBalance);
        builder.Ignore(u => u.TotalDiamondsPurchased);
    }
}
