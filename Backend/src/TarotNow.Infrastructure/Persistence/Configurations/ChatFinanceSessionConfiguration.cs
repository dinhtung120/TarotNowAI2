/*
 * ===================================================================
 * FILE: ChatFinanceSessionConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kẻ Index Unique Hãm Tránh Hai Cái Khay Tiền SQL Mọc Lên Cho Cùng 1 Vụ Chat Tạo Bản Sao Nhân Đôi Escrow Tài Sản Ảm Đạm.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mức Ép Cột Khóa ChatFinanceSession Đảm Bảo Đồng Nhất Với Bên Mạng Document Chuyên Chữa Chat MongoDB Cho SQL Nắm Đúng 1 Sợi.
/// </summary>
public class ChatFinanceSessionConfiguration : IEntityTypeConfiguration<ChatFinanceSession>
{
    public void Configure(EntityTypeBuilder<ChatFinanceSession> builder)
    {
        builder.ToTable("chat_finance_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.ConversationRef)
            .HasColumnName("conversation_ref")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.ReaderId)
            .HasColumnName("reader_id");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.TotalFrozen)
            .HasColumnName("total_frozen");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Chấn Lưới Index Tránh Gửi Mép Chat Gọi Bug Click 2 Lần Chát Giống Nhanh API Rẽ Đẻ Ra 2 Cái Ví Giữ Tiền Khác Nhau Đụng Mâu Thuẫn DB Cả Trận.
        builder.HasIndex(x => x.ConversationRef)
            .HasDatabaseName("ix_chat_finance_sessions_conversation_ref")
            .IsUnique();
    }
}
