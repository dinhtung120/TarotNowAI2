

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity WalletTransaction.
public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    /// <summary>
    /// Cấu hình mapping bảng wallet_transactions.
    /// Luồng xử lý: map cột giao dịch ví, metadata truy vết và unique index idempotency có filter null.
    /// </summary>
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.ToTable("wallet_transactions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        // Sinh UUID tại DB để bảo đảm id luôn có ngay cả khi service không gán trước.

        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

        builder.Property(e => e.Currency)
            .HasColumnName("currency")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired();

        builder.Property(e => e.Amount).HasColumnName("amount").IsRequired();
        builder.Property(e => e.BalanceBefore).HasColumnName("balance_before").IsRequired();
        builder.Property(e => e.BalanceAfter).HasColumnName("balance_after").IsRequired();
        // Lưu đầy đủ số dư trước/sau để audit được mọi biến động số dư.

        builder.Property(e => e.ReferenceSource).HasColumnName("reference_source");
        builder.Property(e => e.ReferenceId).HasColumnName("reference_id");
        builder.Property(e => e.Description).HasColumnName("description");

        builder.Property(e => e.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
        builder.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");

        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.HasIndex(e => e.IdempotencyKey)
            .HasDatabaseName("ix_wallet_transactions_idempotency_key")
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL");
        // Chỉ enforce unique khi có idempotency_key để vừa chống duplicate retry vừa cho phép bản ghi hệ thống không key.
    }
}
