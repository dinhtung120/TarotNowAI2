using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Cấu hình EF Core cho bảng outbox_messages.
/// </summary>
public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    /// Áp cấu hình schema/index/constraint cho outbox message.
    /// </summary>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(x => x.PayloadJson)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.LastError)
            .HasMaxLength(4000);

        builder.Property(x => x.LockOwner)
            .HasMaxLength(128);

        builder.HasIndex(x => new
        {
            x.Status,
            x.NextAttemptAtUtc,
            x.CreatedAtUtc
        }).HasDatabaseName("ix_outbox_messages_polling");

        builder.HasIndex(x => x.OccurredAtUtc)
            .HasDatabaseName("ix_outbox_messages_occurred_at_utc");
    }
}
