using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF cho bảng user_gacha_pities.
/// </summary>
public sealed class UserGachaPityConfiguration : IEntityTypeConfiguration<UserGachaPity>
{
    /// <summary>
    /// Cấu hình schema cho entity <see cref="UserGachaPity"/>.
    /// </summary>
    public void Configure(EntityTypeBuilder<UserGachaPity> builder)
    {
        builder.ToTable(
            "user_gacha_pities",
            tableBuilder => tableBuilder.HasCheckConstraint("ck_user_gacha_pities_pull_count_non_negative", "\"pull_count\" >= 0"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PoolId).IsRequired();
        builder.Property(x => x.PullCount).IsRequired();
        builder.Property(x => x.LastPityResetAtUtc);
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => new { x.UserId, x.PoolId }).IsUnique();
    }
}
