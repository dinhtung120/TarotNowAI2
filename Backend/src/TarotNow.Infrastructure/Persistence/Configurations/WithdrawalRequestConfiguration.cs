

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity WithdrawalRequest.
public sealed class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
{
    /// <summary>
    /// Cấu hình mapping tổng thể bảng withdrawal_requests.
    /// Luồng xử lý: gán table/key, tách cấu hình cột nghiệp vụ và index one-per-day cho request đang mở.
    /// </summary>
    public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.ToTable("withdrawal_requests");
        builder.HasKey(x => x.Id);
        ConfigureColumns(builder);
        ConfigureIndexes(builder);
    }

    /// <summary>
    /// Cấu hình toàn bộ cột dữ liệu rút tiền.
    /// Luồng xử lý: map field định danh, thông tin ngân hàng, trạng thái xử lý admin và mốc thời gian.
    /// </summary>
    private static void ConfigureColumns(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.BusinessWeekStartUtc).HasColumnName("business_week_start_utc");
        builder.Property(x => x.AmountDiamond).HasColumnName("amount_diamond");
        builder.Property(x => x.AmountVnd).HasColumnName("amount_vnd");
        builder.Property(x => x.FeeVnd).HasColumnName("fee_vnd");
        builder.Property(x => x.NetAmountVnd).HasColumnName("net_amount_vnd");
        builder.Property(x => x.BankName).HasColumnName("bank_name").IsRequired();
        builder.Property(x => x.BankAccountName).HasColumnName("bank_account_name").IsRequired();
        builder.Property(x => x.BankAccountNumber).HasColumnName("bank_account_number").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").IsRequired();
        builder.Property(x => x.RequestIdempotencyKey)
            .HasColumnName("request_idempotency_key")
            .HasMaxLength(WithdrawalPolicyConstants.IdempotencyKeyMaxLength)
            .IsRequired();
        builder.Property(x => x.ProcessIdempotencyKey)
            .HasColumnName("process_idempotency_key")
            .HasMaxLength(WithdrawalPolicyConstants.IdempotencyKeyMaxLength);
        builder.Property(x => x.UserNote)
            .HasColumnName("user_note")
            .HasMaxLength(WithdrawalPolicyConstants.NoteMaxLength);
        builder.Property(x => x.AdminId).HasColumnName("admin_id");
        builder.Property(x => x.AdminNote)
            .HasColumnName("admin_note")
            .HasMaxLength(WithdrawalPolicyConstants.NoteMaxLength);
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }

    /// <summary>
    /// Cấu hình index phục vụ kiểm soát nghiệp vụ rút tiền theo ngày.
    /// Luồng xử lý: tạo unique filtered index chỉ áp dụng cho trạng thái pending/approved để chặn nhiều lệnh mở cùng ngày.
    /// </summary>
    private static void ConfigureIndexes(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.BusinessWeekStartUtc })
            .HasDatabaseName("ix_withdrawal_one_per_week")
            .IsUnique();

        builder.HasIndex(x => x.ProcessIdempotencyKey)
            .HasDatabaseName("ix_withdrawal_process_idempotency_key")
            .IsUnique()
            .HasFilter("process_idempotency_key IS NOT NULL");
    }
}
