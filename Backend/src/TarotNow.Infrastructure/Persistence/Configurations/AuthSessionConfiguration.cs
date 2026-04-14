using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapping EF cho auth session.
/// </summary>
public sealed class AuthSessionConfiguration : IEntityTypeConfiguration<AuthSession>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AuthSession> builder)
    {
        builder.ToTable("auth_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DeviceId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.UserAgentHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.LastIpHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc");

        builder.Property(x => x.LastSeenAtUtc)
            .HasColumnName("last_seen_at_utc");

        builder.Property(x => x.RevokedAtUtc)
            .HasColumnName("revoked_at_utc");

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.UserId, x.DeviceId });
        builder.HasIndex(x => x.RevokedAtUtc);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
