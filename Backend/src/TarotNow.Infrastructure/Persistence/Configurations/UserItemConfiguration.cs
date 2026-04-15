using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cấu hình EF mapping cho UserItem.
/// </summary>
public sealed class UserItemConfiguration : IEntityTypeConfiguration<UserItem>
{
    /// <summary>
    /// Cấu hình schema cho bảng user_items.
    /// </summary>
    public void Configure(EntityTypeBuilder<UserItem> builder)
    {
        builder.ToTable("user_items");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ItemDefinitionId).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.AcquiredAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasOne(x => x.ItemDefinition)
            .WithMany()
            .HasForeignKey(x => x.ItemDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.ItemDefinitionId })
            .IsUnique();

        builder.HasIndex(x => x.UserId);
    }
}
