/*
 * ===================================================================
 * FILE: WithdrawalRequestConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kiểm soát Lược Rút Tiền Từ Wallet Reader.
 *   Xoáy Chặt Giới Hạn (1 Người Chỉ Đoạt 1 Đơn Trượt Trống Nằm Cấp/Pending).
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Khóa Ngàm Rút Ví (Cấm Rút X2 Khi Chưa Giải Quyết Nhất Khoát Ở Bảng Thanh Toán Withdraw Này).
/// </summary>
public sealed class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
{
    public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.ToTable("withdrawal_requests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.BusinessDateUtc)
            .HasColumnName("business_date_utc");

        builder.Property(x => x.AmountDiamond)
            .HasColumnName("amount_diamond");

        builder.Property(x => x.AmountVnd)
            .HasColumnName("amount_vnd");

        builder.Property(x => x.FeeVnd)
            .HasColumnName("fee_vnd");

        builder.Property(x => x.NetAmountVnd)
            .HasColumnName("net_amount_vnd");

        builder.Property(x => x.BankName)
            .HasColumnName("bank_name")
            .IsRequired();

        builder.Property(x => x.BankAccountName)
            .HasColumnName("bank_account_name")
            .IsRequired();

        builder.Property(x => x.BankAccountNumber)
            .HasColumnName("bank_account_number")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.AdminId)
            .HasColumnName("admin_id");

        builder.Property(x => x.AdminNote)
            .HasColumnName("admin_note");

        builder.Property(x => x.ProcessedAt)
            .HasColumnName("processed_at");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Phá Xát Kép Cứng Ngắc Index Unique: 1 Đứa Mõm User Phải Nghỉ Chờ Hoặc Được Trả Hoặc Không 1 Ngày Chỉ Rụt Rút Xin 1 Lần Nếu Tờ Đơn Của Nó Báo Cục ("pending" Đang Chờ Admin Mắt Nửa / "approved" Duyệt Trả Nhưng Bank Chuyển Pending).
        // Vứt Của Bỏ Khách Cứ Bấm Rút Tiền Nửa Đi Cưa 1 Tỷ Lần Vào Nén Report Admin Server Kêu Trống Giờ Mẻ Báo Nút Xin Lỗi Cắt.
        builder.HasIndex(x => new { x.UserId, x.BusinessDateUtc })
            .HasDatabaseName("ix_withdrawal_one_per_day_active")
            .IsUnique()
            .HasFilter("status in ('pending','approved')");
    }
}
