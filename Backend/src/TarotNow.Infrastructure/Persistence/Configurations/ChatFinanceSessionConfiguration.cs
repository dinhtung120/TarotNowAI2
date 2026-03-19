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
        // Chấn Lưới Index Tránh Gửi Mép Chat Gọi Bug Click 2 Lần Chát Giống Nhanh API Rẽ Đẻ Ra 2 Cái Ví Giữ Tiền Khác Nhau Đụng Mâu Thuẫn DB Cả Trận.
        builder.HasIndex(x => x.ConversationRef)
            .HasDatabaseName("ix_chat_finance_sessions_conversation_ref")
            .IsUnique();
    }
}
