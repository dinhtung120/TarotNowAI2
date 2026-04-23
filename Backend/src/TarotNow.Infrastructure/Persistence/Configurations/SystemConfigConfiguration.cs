using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho entity <see cref="SystemConfig"/>.
/// </summary>
public sealed class SystemConfigConfiguration : IEntityTypeConfiguration<SystemConfig>
{
    /// <summary>
    /// Cấu hình schema cho bảng system_configs.
    /// </summary>
    public void Configure(EntityTypeBuilder<SystemConfig> builder)
    {
        builder.ToTable(
            "system_configs",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_system_configs_value_kind",
                    "\"value_kind\" IN ('scalar', 'json')");
            });

        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key)
            .HasColumnName("key")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.ValueKind)
            .HasColumnName("value_kind")
            .HasMaxLength(16)
            .HasDefaultValue("scalar")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.UpdatedAt)
            .HasDatabaseName("ix_system_configs_updated_at");
    }
}
