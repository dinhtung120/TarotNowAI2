

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity DepositOrder.
public class DepositOrderConfiguration : IEntityTypeConfiguration<DepositOrder>
{
    /// <summary>
    /// Cấu hình mapping bảng deposit_orders.
    /// Luồng xử lý: map bảng/key/cột chính, quan hệ user và các index truy vấn trạng thái/giao dịch.
    /// </summary>
    public void Configure(EntityTypeBuilder<DepositOrder> builder)
    {
        builder.ToTable("deposit_orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.AmountVnd)
            .IsRequired();

        builder.Property(o => o.DiamondAmount)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.TransactionId)
            .HasMaxLength(100);

        builder.Property(o => o.FxSnapshot)
            .HasMaxLength(1000);

        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // Xóa user sẽ xóa luôn các deposit order liên quan để tránh orphan data.

        builder.HasIndex(o => o.TransactionId).IsUnique();
        // Unique transaction id để bảo vệ idempotency callback từ cổng thanh toán.

        builder.HasIndex(o => o.Status);
        // Index trạng thái giúp tối ưu job quét pending/failed.
    }
}
