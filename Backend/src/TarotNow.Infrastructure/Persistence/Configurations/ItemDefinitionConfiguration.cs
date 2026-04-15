using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho ItemDefinition.
/// </summary>
public sealed class ItemDefinitionConfiguration : IEntityTypeConfiguration<ItemDefinition>
{
    /// <summary>
    /// Cấu hình schema cho bảng item_definitions.
    /// </summary>
    public void Configure(EntityTypeBuilder<ItemDefinition> builder)
    {
        builder.ToTable("item_definitions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(64).IsRequired();
        builder.Property(x => x.EnhancementType).HasMaxLength(64);
        builder.Property(x => x.Rarity).HasMaxLength(32).IsRequired();

        builder.Property(x => x.IsConsumable).IsRequired();
        builder.Property(x => x.IsPermanent).IsRequired();
        builder.Property(x => x.EffectValue).IsRequired();
        builder.Property(x => x.SuccessRatePercent).HasColumnType("numeric(5,2)").IsRequired();

        builder.Property(x => x.NameVi).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameZh).HasMaxLength(200).IsRequired();

        builder.Property(x => x.DescriptionVi).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.DescriptionEn).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.DescriptionZh).HasMaxLength(1000).IsRequired();

        builder.Property(x => x.IconUrl).HasMaxLength(500);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.Type, x.IsActive });
    }
}
