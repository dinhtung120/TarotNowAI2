

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity ChatQuestionItem.
public class ChatQuestionItemConfiguration : IEntityTypeConfiguration<ChatQuestionItem>
{
    /// <summary>
    /// Cấu hình mapping tổng thể cho chat_question_items.
    /// Luồng xử lý: set table/key và tách cấu hình core columns, timing columns, relationship, indexes.
    /// </summary>
    public void Configure(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.ToTable("chat_question_items");
        builder.HasKey(x => x.Id);
        ConfigureCoreColumns(builder);
        ConfigureTimingColumns(builder);
        ConfigureRelationship(builder);
        ConfigureIndexes(builder);
    }

    /// <summary>
    /// Cấu hình các cột nghiệp vụ cốt lõi của item.
    /// Luồng xử lý: map id, khóa liên kết, trạng thái, số tiền và idempotency key.
    /// </summary>
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

    /// <summary>
    /// Cấu hình các cột mốc thời gian của vòng đời item.
    /// Luồng xử lý: map toàn bộ các timestamp offer/accept/reply/release/refund/dispute.
    /// </summary>
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
        builder.Property(x => x.DisputeReason).HasColumnName("dispute_reason").HasMaxLength(1000);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }

    /// <summary>
    /// Cấu hình quan hệ item -> finance session.
    /// Luồng xử lý: thiết lập one-to-many, foreign key bắt buộc và cascade delete.
    /// </summary>
    private static void ConfigureRelationship(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.HasOne(x => x.FinanceSession)
            .WithMany(x => x.QuestionItems)
            .HasForeignKey(x => x.FinanceSessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired()
            .HasConstraintName("fk_chat_question_items_chat_finance_sessions_finance_session_id");
    }

    /// <summary>
    /// Cấu hình index phục vụ truy vấn và chống ghi trùng idempotency.
    /// Luồng xử lý: tạo index finance_session_id và unique index idempotency_key có filter not null.
    /// </summary>
    private static void ConfigureIndexes(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.HasIndex(x => x.FinanceSessionId)
            .HasDatabaseName("ix_chat_question_items_finance_session_id");

        builder.HasIndex(x => x.IdempotencyKey)
            .HasDatabaseName("ix_chat_question_items_idempotency_key")
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL");

        builder.HasIndex(x => new { x.Status, x.DisputeWindowEnd })
            .HasDatabaseName("ix_chat_question_items_status_dispute_window_end");
    }
}
