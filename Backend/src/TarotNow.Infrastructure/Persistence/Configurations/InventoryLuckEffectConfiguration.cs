using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho InventoryLuckEffect.
/// </summary>
public sealed class InventoryLuckEffectConfiguration : IEntityTypeConfiguration<InventoryLuckEffect>
{
    /// <summary>
    /// Cấu hình schema cho bảng inventory_luck_effects.
    /// </summary>
    public void Configure(EntityTypeBuilder<InventoryLuckEffect> builder)
    {
        builder.ToTable(
            "inventory_luck_effects",
            tableBuilder => tableBuilder.HasCheckConstraint("ck_inventory_luck_effects_luck_value", "\"luck_value\" > 0"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.LuckValue).IsRequired();
        builder.Property(x => x.ExpiresAtUtc).IsRequired();
        builder.Property(x => x.SourceItemCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => x.ExpiresAtUtc);
    }
}
