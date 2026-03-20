/*
 * ===================================================================
 * FILE: ChatQuestionItemConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Fluent API cấu hình Bảng Giam Tiền Chat Câu Hỏi Phụ (PostgreSQL).
 *   Đảm bảo Index Quét Nhanh Tránh Refund Trùng, Giữ Giao Dịch Không Thủng Ví.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Quyền Lực DB Xoạc Bảng Câu Hỏi (ChatQuestionItem) Lên SQL Bằng Fluent API.
/// </summary>
public class ChatQuestionItemConfiguration : IEntityTypeConfiguration<ChatQuestionItem>
{
    public void Configure(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.ToTable("chat_question_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.FinanceSessionId)
            .HasColumnName("finance_session_id");

        builder.Property(x => x.ConversationRef)
            .HasColumnName("conversation_ref")
            .IsRequired();

        builder.Property(x => x.PayerId)
            .HasColumnName("payer_id");

        builder.Property(x => x.ReceiverId)
            .HasColumnName("receiver_id");

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired();

        builder.Property(x => x.AmountDiamond)
            .HasColumnName("amount_diamond");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.ProposalMessageRef)
            .HasColumnName("proposal_message_ref");

        builder.Property(x => x.OfferExpiresAt)
            .HasColumnName("offer_expires_at");

        builder.Property(x => x.AcceptedAt)
            .HasColumnName("accepted_at");

        builder.Property(x => x.ReaderResponseDueAt)
            .HasColumnName("reader_response_due_at");

        builder.Property(x => x.RepliedAt)
            .HasColumnName("replied_at");

        builder.Property(x => x.AutoReleaseAt)
            .HasColumnName("auto_release_at");

        builder.Property(x => x.AutoRefundAt)
            .HasColumnName("auto_refund_at");

        builder.Property(x => x.ReleasedAt)
            .HasColumnName("released_at");

        builder.Property(x => x.ConfirmedAt)
            .HasColumnName("confirmed_at");

        builder.Property(x => x.RefundedAt)
            .HasColumnName("refunded_at");

        builder.Property(x => x.DisputeWindowStart)
            .HasColumnName("dispute_window_start");

        builder.Property(x => x.DisputeWindowEnd)
            .HasColumnName("dispute_window_end");

        builder.Property(x => x.IdempotencyKey)
            .HasColumnName("idempotency_key");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(x => x.FinanceSession)
            .WithMany(x => x.QuestionItems)
            .HasForeignKey(x => x.FinanceSessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired()
            .HasConstraintName("fk_chat_question_items_chat_finance_sessions_finance_session_id");

        builder.HasIndex(x => x.FinanceSessionId)
            .HasDatabaseName("ix_chat_question_items_finance_session_id");

        // Nhốt Trùng Lặp Thao Tác (Idempotency Key): Tránh Nát Database Nếu Code Trả Tiền / Rút Tiền Kêu 2 Lần Trên Cùng 1 Câu Hỏi (Lỗi Timeout Bấm Lại Gấp).
        builder.HasIndex(x => x.IdempotencyKey)
            .HasDatabaseName("ix_chat_question_items_idempotency_key")
            .IsUnique() // Ép Chết DB Báo Lỗi Nếu Insert Quả Cột String IdempotencyKey Giống Hệt Quả Cũ.
            .HasFilter("idempotency_key IS NOT NULL"); // Ngoại Lệ: Nó Là Null Thì Cho Lọt (SQL Bão Hòa Filter Này).
    }
}
