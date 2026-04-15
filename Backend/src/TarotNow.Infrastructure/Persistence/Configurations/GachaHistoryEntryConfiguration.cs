using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF cho bảng gacha_history_entries.
/// </summary>
public sealed class GachaHistoryEntryConfiguration : IEntityTypeConfiguration<GachaHistoryEntry>
{
    /// <summary>
    /// Cấu hình schema cho entity <see cref="GachaHistoryEntry"/>.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaHistoryEntry> builder)
    {
        builder.ToTable(
            "gacha_history_entries",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pull_count_positive", "\"pull_count\" > 0");
                tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pity_before_non_negative", "\"pity_before\" >= 0");
                tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pity_after_non_negative", "\"pity_after\" >= 0");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PullOperationId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PoolId).IsRequired();
        builder.Property(x => x.PoolCode).IsRequired().HasMaxLength(64);
        builder.Property(x => x.PullCount).IsRequired();
        builder.Property(x => x.PityBefore).IsRequired();
        builder.Property(x => x.PityAfter).IsRequired();
        builder.Property(x => x.WasPityReset).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.HasIndex(x => x.PullOperationId).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.CreatedAtUtc }).IsDescending(false, true);
        builder.HasIndex(x => new { x.UserId, x.PoolId, x.CreatedAtUtc }).IsDescending(false, false, true);
    }
}
