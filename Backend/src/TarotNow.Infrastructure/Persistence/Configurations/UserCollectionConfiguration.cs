using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class UserCollectionConfiguration : IEntityTypeConfiguration<UserCollection>
{
    public void Configure(EntityTypeBuilder<UserCollection> builder)
    {
        builder.ToTable("user_collections");

        // Composite Key: A user can only have one tracking record per unique card
        builder.HasKey(e => new { e.UserId, e.CardId });

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.CardId)
            .HasColumnName("card_id")
            .IsRequired();

        builder.Property(e => e.Level)
            .HasColumnName("level")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(e => e.Copies)
            .HasColumnName("copies")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(e => e.ExpGained)
            .HasColumnName("exp_gained")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.LastDrawnAt)
            .HasColumnName("last_drawn_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(e => e.UserId);
    }
}
