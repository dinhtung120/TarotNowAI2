using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho entity <see cref="GachaPullOperation"/>.
/// </summary>
public sealed class GachaPullOperationConfiguration : IEntityTypeConfiguration<GachaPullOperation>
{
    /// <summary>
    /// Cấu hình schema cho bảng gacha_pull_operations.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaPullOperation> builder)
    {
        builder.ToTable(
            "gacha_pull_operations",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_operations_pull_count_positive",
                    "\"pull_count\" > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_operations_current_pity_non_negative",
                    "\"current_pity_count\" >= 0");
                tableBuilder.HasCheckConstraint(
                    "ck_gacha_pull_operations_hard_pity_non_negative",
                    "\"hard_pity_threshold\" >= 0");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PoolId).IsRequired();
        builder.Property(x => x.IdempotencyKey).IsRequired().HasMaxLength(128);

        builder.Property(x => x.PullCount).IsRequired();
        builder.Property(x => x.CurrentPityCount).IsRequired();
        builder.Property(x => x.HardPityThreshold).IsRequired();
        builder.Property(x => x.WasPityTriggered).IsRequired();
        builder.Property(x => x.IsCompleted).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.CompletedAtUtc);

        builder.HasIndex(x => new { x.UserId, x.IdempotencyKey }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.PoolId, x.CreatedAtUtc }).IsDescending(false, false, true);
    }
}
