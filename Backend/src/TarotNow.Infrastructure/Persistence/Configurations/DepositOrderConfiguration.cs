using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity DepositOrder.
public class DepositOrderConfiguration : IEntityTypeConfiguration<DepositOrder>
{
    /// <summary>
    /// Cấu hình mapping bảng deposit_orders.
    /// </summary>
    public void Configure(EntityTypeBuilder<DepositOrder> builder)
    {
        builder.ToTable("deposit_orders");
        builder.HasKey(o => o.Id);
        ConfigureColumns(builder);
        ConfigureRelation(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureColumns(EntityTypeBuilder<DepositOrder> builder)
    {
        builder.Property(o => o.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(o => o.PackageCode).HasColumnName("package_code").HasMaxLength(64).IsRequired();
        builder.Property(o => o.AmountVnd).HasColumnName("amount_vnd").IsRequired();
        builder.Property(o => o.BaseDiamondAmount).HasColumnName("base_diamond_amount").IsRequired();
        builder.Property(o => o.BonusGoldAmount).HasColumnName("bonus_gold_amount").IsRequired();
        builder.Property(o => o.DiamondAmount).HasColumnName("diamond_amount").IsRequired();
        builder.Property(o => o.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
        builder.Property(o => o.ClientRequestKey).HasColumnName("client_request_key").HasMaxLength(128).IsRequired();
        builder.Property(o => o.PayOsOrderCode).HasColumnName("payos_order_code").IsRequired();
        builder.Property(o => o.PayOsPaymentLinkId).HasColumnName("payos_payment_link_id").HasMaxLength(128).IsRequired();
        builder.Property(o => o.CheckoutUrl).HasColumnName("checkout_url").HasMaxLength(1000).IsRequired();
        builder.Property(o => o.QrCode).HasColumnName("qr_code").HasMaxLength(4000).IsRequired();
        builder.Property(o => o.TransactionId).HasColumnName("transaction_id").HasMaxLength(200);
        builder.Property(o => o.FailureReason).HasColumnName("failure_reason").HasMaxLength(500);
        builder.Property(o => o.ExpiresAtUtc).HasColumnName("expires_at_utc");
        builder.Property(o => o.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(o => o.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(o => o.ProcessedAt).HasColumnName("processed_at");
        builder.Property(o => o.WalletGrantedAtUtc).HasColumnName("wallet_granted_at_utc");
    }

    private static void ConfigureRelation(EntityTypeBuilder<DepositOrder> builder)
    {
        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureIndexes(EntityTypeBuilder<DepositOrder> builder)
    {
        builder.HasIndex(o => o.ClientRequestKey)
            .IsUnique()
            .HasDatabaseName("ix_deposit_orders_client_request_key");

        builder.HasIndex(o => o.PayOsOrderCode)
            .IsUnique()
            .HasDatabaseName("ix_deposit_orders_payos_order_code");

        builder.HasIndex(o => o.Status)
            .HasDatabaseName("ix_deposit_orders_status");
    }
}
