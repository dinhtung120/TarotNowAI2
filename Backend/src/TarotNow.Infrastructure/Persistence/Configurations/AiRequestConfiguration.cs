/*
 * ===================================================================
 * FILE: AiRequestConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bảng Fluent API cấu hình Cột Cứng Mật Định Các Trường Bảng `ai_requests` Ở EF Core Nhằm Thiết Lập Chỉ Mục Indexes Nhanh Ánh Cột Tên.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Dao Cấu Hình Bảng ai_requests Đính Trong Khung PostgreSQL (Gói Quy Tắc Rời Để Context Đỡ Đẹp Bụng).
/// </summary>
public class AiRequestConfiguration : IEntityTypeConfiguration<AiRequest>
{
    public void Configure(EntityTypeBuilder<AiRequest> builder)
    {
        // 1. Áp Mọc Tên Bảng Sẽ Đổ Khung
        builder.ToTable("ai_requests");

        // 2. Chốt Khóa Chính Trụ 
        builder.HasKey(x => x.Id);

        // 3. Giáp Nét Tên Cột Rời Ra Cho Tường Minh Mặc Dù Context Dịch Tự Chống Mặc Gây Cãi.
        builder.Property(x => x.UserId).HasColumnName("user_id");

        // Reference Chỉ Tới Mongo String Nặc Danh Lõi Khác Nhau Mạch
        builder.Property(x => x.ReadingSessionRef)
            .HasColumnName("reading_session_ref")
            .IsRequired()
            .HasMaxLength(36);

        // Nẹp Lõi Cố Mặc Định Type Rải Thường
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasDefaultValue("requested") // Điền Nấp Thấy Là Gọi Requested Bắt Đầu Status Nhai
            .HasMaxLength(50);

        builder.Property(x => x.FollowupSequence).HasColumnName("followup_sequence").IsRequired(false);
        builder.Property(x => x.FinishReason).HasColumnName("finish_reason").HasMaxLength(50);
        builder.Property(x => x.PromptVersion).HasColumnName("prompt_version").HasMaxLength(20);
        builder.Property(x => x.PolicyVersion).HasColumnName("policy_version").HasMaxLength(20);
        builder.Property(x => x.TraceId).HasColumnName("trace_id").HasMaxLength(64);
        builder.Property(x => x.CorrelationId).HasColumnName("correlation_id");
        builder.Property(x => x.FirstTokenAt).HasColumnName("first_token_at");
        builder.Property(x => x.CompletionMarkerAt).HasColumnName("completion_marker_at");
        
        builder.Property(x => x.ChargeGold).HasColumnName("charge_gold").HasDefaultValue(0);
        builder.Property(x => x.ChargeDiamond).HasColumnName("charge_diamond").HasDefaultValue(0);
        
        builder.Property(x => x.RequestedLocale).HasColumnName("requested_locale").HasMaxLength(10);
        builder.Property(x => x.ReturnedLocale).HasColumnName("returned_locale").HasMaxLength(10);
        
        builder.Property(x => x.IdempotencyKey).HasColumnName("idempotency_key").HasMaxLength(100);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        // 4. Các Index Thọc Hiểm Truy Vết Query Mạng DB Không Bị Mờ Sáng Gắt Toàn Bảng (Full Table Scan Đốt Gạo).
        
        // Mũ Index Ép Độc Nhất Biến Bắn Idempotency Check Gốc Dừng Bắn Refund Trùng Nhau Thất Thế Nạn DB Kêu Khống Phá.
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL")
            .HasDatabaseName("idx_ai_requests_idempotency");

        // Ám Mớ Tracking Cho Nhanh Truy Đơn Nào Trả Về Mớ Tới Lấy Session History Nào Gọn.
        builder.HasIndex(x => x.ReadingSessionRef)
            .HasDatabaseName("idx_ai_requests_reading");

        // Trích Trùng Cột Kép: Status Đẩy Thời Gian Tìm Ai Chết Giữa Lỗi Đọc Đang Requested Qua Kì Date Lọc Xóa Tịt Hoàn Mạng.
        builder.HasIndex(x => new { x.Status, x.CreatedAt })
            .HasDatabaseName("idx_ai_requests_status");
    }
}
