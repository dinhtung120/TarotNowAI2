using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Cấu hình EF Core cho bảng outbox_inline_handler_states.
/// </summary>
public sealed class OutboxInlineHandlerStateConfiguration : IEntityTypeConfiguration<OutboxInlineHandlerState>
{
    /// <summary>
    /// Áp cấu hình schema/index cho inline handler idempotency state.
    /// </summary>
    public void Configure(EntityTypeBuilder<OutboxInlineHandlerState> builder)
    {
        builder.ToTable("outbox_inline_handler_states");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventKey)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.HandlerName)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(x => new
        {
            x.EventKey,
            x.HandlerName
        }).IsUnique().HasDatabaseName("ux_outbox_inline_handler_states_event_handler");

        builder.HasIndex(x => x.ProcessedAtUtc)
            .HasDatabaseName("ix_outbox_inline_handler_states_processed_at_utc");
    }
}
