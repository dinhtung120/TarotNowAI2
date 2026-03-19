/*
 * ===================================================================
 * FILE: WalletTransactionConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Sổ DB Ledgers (Sổ Cái Phi Tập Trung): Khóa Nối Chuỗi Các Bảng Tường Ghi Giao Dịch Không Bao Giờ Thay Đổi Của Túi Tiền. Không Rõ Rách Update.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Dao Ép Cứng Bảng Lưới Kế Toán Lược Trình Ledger. 
/// Gắn Ép Sắc Sạch Giới Hạn UUID Mã Chuối Không Đi Đọc Gọng Bạc Tít Thòng Tịt Lỗi Lộ 1 Ráp Méo Nhau Của Khách Trừng Tít Refund.
/// </summary>
public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        // 1. Phác Thảo Cục
        builder.ToTable("wallet_transactions");

        // 2. Ép Gốc Khóa Gọt Trả
        builder.HasKey(e => e.Id);
        
        // Buộc Máy PostgreSQL Gen Cho Mình 1 Cái ID Giả Gắn Mã UUID Auto Bọt Cho Bảng Khỏi Mất Cốt Client Đẩy Cố Bẩm Lỗ 
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        // Chỉ Thằng Mẹ Xéo Cha Nó Xài Tiền Mới Được Ghi Sổ Này (Ép Không Rỗng).
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

        // Ném Xòe Lưu Lọt 2 Cột Dạng Enum Sang Chuỗi Chữ Xong Dấu Vào DB Cho Thằng Web Khác Hoặc Data Analyst Mòn MySQL PostgreSQL Nó Đọc Đuổi Dễ (ko lưu Enum kiểu Số 1 2 3 Nát Đo Đáo Trình Nát DB Xóa Nghịch Code Enum Xoắn Quảy Error Null App).
        builder.Property(e => e.Currency)
            .HasColumnName("currency")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired();

        // 3 Cột Kinh Tế Rạch Trắng Rành Mạch: Amount (Mấy Đồng), Rớt Nước Balance Trước Và Rớt Phọt Ra Balance Cân Đối Mới Nhất Đoán Quét Cuối Transaction Nhíu App Không Bớt Của Nhọn Báo Biến (Cấm Null).
        builder.Property(e => e.Amount).HasColumnName("amount").IsRequired();
        builder.Property(e => e.BalanceBefore).HasColumnName("balance_before").IsRequired();
        builder.Property(e => e.BalanceAfter).HasColumnName("balance_after").IsRequired();
        
        // Trường Tham Chiếu Gọi Giữa Mua Kim Cương Hoặc Đập Ai Câu Hỏi Ngắm Vòng Cục Cớ Giao Cấu (Cho Bỏ Chống Lỗi Kẹp)
        builder.Property(e => e.ReferenceSource).HasColumnName("reference_source");
        builder.Property(e => e.ReferenceId).HasColumnName("reference_id");
        builder.Property(e => e.Description).HasColumnName("description");
        
        // Mũ Bọc Thêm Phụ Json Để Mai Mốt Tráo Bài Phát Cho Lỗ Chứa Log Dẹp 2 Kiểu Transaction Mã Hoán Code Không Rườm Bảng To
        builder.Property(e => e.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
        builder.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
        
        // Khoá Thời Tiền Vặn Kẹp Check Đòi Sớm Mở Bờ.
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");

        // KHóa Kéo Khẩu (Index). Idempotency Rất Mạnh: Khi Thằng Application Có Nhỡ Gây Trượt Bấm Request "Rút Tiền Phạt Bói" 2 Lần. Lần Khay 2 Sẽ Đánh Unique Thục Phát PostgreSQL Báo Ngoại Lệ Crash Nghẹn Đánh Lệnh Lỗi Giao Tranh Chết Thả Rolback DB Trả Sạch (Khống Trừ X2 Lượt Ở Tiền User Cấm Hack Mãi Kim Cương Âm Ví).
        builder.HasIndex(e => e.IdempotencyKey)
            .HasDatabaseName("ix_wallet_transactions_idempotency_key")
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL");
    }
}
