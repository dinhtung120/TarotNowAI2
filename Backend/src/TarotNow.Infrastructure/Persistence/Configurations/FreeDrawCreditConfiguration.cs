using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho FreeDrawCredit.
/// </summary>
public sealed class FreeDrawCreditConfiguration : IEntityTypeConfiguration<FreeDrawCredit>
{
    /// <summary>
    /// Cấu hình schema cho bảng free_draw_credits.
    /// </summary>
    public void Configure(EntityTypeBuilder<FreeDrawCredit> builder)
    {
        builder.ToTable(
            "free_draw_credits",
            tableBuilder => tableBuilder.HasCheckConstraint("ck_free_draw_credits_available_count", "\"available_count\" >= 0"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.AvailableCount).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}
