using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Cấu hình EF Core cho bảng outbox_handler_states.
/// </summary>
public sealed class OutboxHandlerStateConfiguration : IEntityTypeConfiguration<OutboxHandlerState>
{
    /// <summary>
    /// Áp cấu hình schema/index/constraint cho outbox handler state.
    /// </summary>
    public void Configure(EntityTypeBuilder<OutboxHandlerState> builder)
    {
        builder.ToTable("outbox_handler_states");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HandlerName)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasOne<OutboxMessage>()
            .WithMany()
            .HasForeignKey(x => x.OutboxMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.OutboxMessageId,
            x.HandlerName
        }).IsUnique().HasDatabaseName("ux_outbox_handler_states_message_handler");

        builder.HasIndex(x => x.ProcessedAtUtc)
            .HasDatabaseName("ix_outbox_handler_states_processed_at_utc");
    }
}
