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
        // Nhốt Trùng Lặp Thao Tác (Idempotency Key): Tránh Nát Database Nếu Code Trả Tiền / Rút Tiền Kêu 2 Lần Trên Cùng 1 Câu Hỏi (Lỗi Timeout Bấm Lại Gấp).
        builder.HasIndex(x => x.IdempotencyKey)
            .HasDatabaseName("ix_chat_question_items_idempotency_key")
            .IsUnique() // Ép Chết DB Báo Lỗi Nếu Insert Quả Cột String IdempotencyKey Giống Hệt Quả Cũ.
            .HasFilter("idempotency_key IS NOT NULL"); // Ngoại Lệ: Nó Là Null Thì Cho Lọt (SQL Bão Hòa Filter Này).
    }
}
