/*
 * ===================================================================
 * FILE: DepositOrderConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Fluent API cấu hình Bảng Tờ Đơn Nạp Tiền (VNĐ → Kim Cương).
 *   Siết chặt Tên Cột, Khóa Ngoại và Các Đoạn Mã Giao Dịch Không Được Cho Phép Trùng (Tránh Nhận Tiền Gấp Đôi Phi Lý).
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Thiết Bị Đóng Khuôn Bảng Nạp Thạch (DepositOrder).
/// </summary>
public class DepositOrderConfiguration : IEntityTypeConfiguration<DepositOrder>
{
    public void Configure(EntityTypeBuilder<DepositOrder> builder)
    {
        // Chấn Tên Bảng Ném Vô Postgres
        builder.ToTable("deposit_orders");

        // Khóa Chính ID
        builder.HasKey(o => o.Id);

        // Bất Cứ Giá Nào Nạp Cũng Ko Thể Ném Null Vô Cột Tiền Này
        builder.Property(o => o.AmountVnd)
            .IsRequired();

        // 10 Viên Kim Cũng Phải Có Giá Trị Nhất Định Ko Null
        builder.Property(o => o.DiamondAmount)
            .IsRequired();

        // Trạng Thái Pending/Success Chấm Dữ Vừa Đủ 20 Ký Tự Để Ép DB Dẹt Nhẹ Xíu (Thay Vì MAX Rác Kéo Vết Byte)
        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(20);

        // Chuỗi Dấu Mã Code Bank Hay MoMo Trả Về Khống Chống Nổ Data Chặn Ở 100 Chữ
        builder.Property(o => o.TransactionId)
            .HasMaxLength(100);

        // Ghi Nháp Bản Sao Khuyến Mãi Ngay Thời Điểm Nạp Tránh Mất Vết Lịch Sử Dữ Liệu Bị Thay Đổi Trượt Thụt. Giữ Chặn Sổ Cho Kế Toán Tracking
        builder.Property(o => o.FxSnapshot)
            .HasMaxLength(1000);

        // Nối Cột UserId Của Bảng Này Ánh Xạ Về Khóa Phá Chủ FK Bảng User Mẹ Của Thằng Nạp Tiền.
        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cụt Sổ User Xóa Cả Án Đơn Nạp Tiền Của Nó Luôn.
            
        // Quan Trọng Nhất: Ép Độc Nhất TransactionID Tránh MoMo Trả Webhook Lên Server 2 Phát Gây Lỗi Trùng Tiền X2 Nạp Thạch Lủng Bank App.
        builder.HasIndex(o => o.TransactionId).IsUnique();
        
        // Quét Gáy Nhanh Đơn Nào Trạng Thái Status Chưa Hoàn Tất Để Cày Job Quét CleanUp Soát Rác Sớm.
        builder.HasIndex(o => o.Status);
    }
}
