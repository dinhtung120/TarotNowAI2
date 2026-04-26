using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public sealed class ReadingRevealSagaStateConfiguration : IEntityTypeConfiguration<ReadingRevealSagaState>
{
    public void Configure(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.ToTable("reading_reveal_saga_states");
        ConfigureIdentity(builder);
        ConfigureCoreColumns(builder);
        ConfigureChargeColumns(builder);
        ConfigureProgressColumns(builder);
        ConfigureTimingColumns(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureIdentity(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.SessionId)
            .HasColumnName("session_id")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
    }

    private static void ConfigureCoreColumns(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.Property(x => x.Language)
            .HasColumnName("language")
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.RevealedCardsJson)
            .HasColumnName("revealed_cards_json")
            .HasColumnType("text");
    }

    private static void ConfigureChargeColumns(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.Property(x => x.ChargeDebited)
            .HasColumnName("charge_debited")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.ChargeCurrency)
            .HasColumnName("charge_currency")
            .HasMaxLength(32);

        builder.Property(x => x.ChargeChangeType)
            .HasColumnName("charge_change_type")
            .HasMaxLength(64);

        builder.Property(x => x.ChargeAmount)
            .HasColumnName("charge_amount")
            .HasDefaultValue(0L)
            .IsRequired();

        builder.Property(x => x.ChargeReferenceId)
            .HasColumnName("charge_reference_id")
            .HasMaxLength(128);
    }

    private static void ConfigureProgressColumns(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.Property(x => x.CollectionApplied)
            .HasColumnName("collection_applied")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.ExpGranted)
            .HasColumnName("exp_granted")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.SessionCompleted)
            .HasColumnName("session_completed")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.RevealedEventPublished)
            .HasColumnName("revealed_event_published")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.RefundCompensated)
            .HasColumnName("refund_compensated")
            .HasDefaultValue(false)
            .IsRequired();
    }

    private static void ConfigureTimingColumns(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.Property(x => x.AttemptCount)
            .HasColumnName("attempt_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.LastAttemptAtUtc).HasColumnName("last_attempt_at_utc");
        builder.Property(x => x.NextRepairAtUtc).HasColumnName("next_repair_at_utc");
        builder.Property(x => x.LastError).HasColumnName("last_error").HasColumnType("text");
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc").IsRequired();
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<ReadingRevealSagaState> builder)
    {
        builder.HasIndex(x => x.SessionId)
            .IsUnique()
            .HasDatabaseName("ux_reading_reveal_saga_states_session_id");

        builder.HasIndex(x => new { x.Status, x.NextRepairAtUtc })
            .HasDatabaseName("ix_reading_reveal_saga_states_status_next_repair");
    }
}
