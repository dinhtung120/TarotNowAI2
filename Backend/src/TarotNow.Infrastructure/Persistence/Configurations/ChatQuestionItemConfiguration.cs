

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class ChatQuestionItemConfiguration : IEntityTypeConfiguration<ChatQuestionItem>
{
    public void Configure(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.ToTable("chat_question_items");
        builder.HasKey(x => x.Id);
        ConfigureCoreColumns(builder);
        ConfigureTimingColumns(builder);
        ConfigureRelationship(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureCoreColumns(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FinanceSessionId).HasColumnName("finance_session_id");
        builder.Property(x => x.ConversationRef).HasColumnName("conversation_ref").IsRequired();
        builder.Property(x => x.PayerId).HasColumnName("payer_id");
        builder.Property(x => x.ReceiverId).HasColumnName("receiver_id");
        builder.Property(x => x.Type).HasColumnName("type").IsRequired();
        builder.Property(x => x.AmountDiamond).HasColumnName("amount_diamond");
        builder.Property(x => x.Status).HasColumnName("status").IsRequired();
        builder.Property(x => x.ProposalMessageRef).HasColumnName("proposal_message_ref");
        builder.Property(x => x.IdempotencyKey).HasColumnName("idempotency_key");
    }

    private static void ConfigureTimingColumns(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.Property(x => x.OfferExpiresAt).HasColumnName("offer_expires_at");
        builder.Property(x => x.AcceptedAt).HasColumnName("accepted_at");
        builder.Property(x => x.ReaderResponseDueAt).HasColumnName("reader_response_due_at");
        builder.Property(x => x.RepliedAt).HasColumnName("replied_at");
        builder.Property(x => x.AutoReleaseAt).HasColumnName("auto_release_at");
        builder.Property(x => x.AutoRefundAt).HasColumnName("auto_refund_at");
        builder.Property(x => x.ReleasedAt).HasColumnName("released_at");
        builder.Property(x => x.ConfirmedAt).HasColumnName("confirmed_at");
        builder.Property(x => x.RefundedAt).HasColumnName("refunded_at");
        builder.Property(x => x.DisputeWindowStart).HasColumnName("dispute_window_start");
        builder.Property(x => x.DisputeWindowEnd).HasColumnName("dispute_window_end");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }

    private static void ConfigureRelationship(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.HasOne(x => x.FinanceSession)
            .WithMany(x => x.QuestionItems)
            .HasForeignKey(x => x.FinanceSessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired()
            .HasConstraintName("fk_chat_question_items_chat_finance_sessions_finance_session_id");
    }

    private static void ConfigureIndexes(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.HasIndex(x => x.FinanceSessionId)
            .HasDatabaseName("ix_chat_question_items_finance_session_id");

        builder.HasIndex(x => x.IdempotencyKey)
            .HasDatabaseName("ix_chat_question_items_idempotency_key")
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL");
    }
}
