using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho InventoryItemUseOperation.
/// </summary>
public sealed class InventoryItemUseOperationConfiguration : IEntityTypeConfiguration<InventoryItemUseOperation>
{
    /// <summary>
    /// Cấu hình schema cho bảng inventory_item_use_operations.
    /// </summary>
    public void Configure(EntityTypeBuilder<InventoryItemUseOperation> builder)
    {
        builder.ToTable("inventory_item_use_operations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.IdempotencyKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ItemCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.TargetCardId);
        builder.Property(x => x.ProcessedAtUtc).IsRequired();

        builder.HasIndex(x => new { x.UserId, x.IdempotencyKey })
            .HasDatabaseName("ix_inventory_item_use_operations_user_id_idempotency_key")
            .IsUnique();

        builder.HasIndex(x => new { x.UserId, x.ProcessedAtUtc });
    }
}
