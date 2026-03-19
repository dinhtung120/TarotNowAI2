/*
 * ===================================================================
 * FILE: DepositPromotionConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cấu hình Bảng Khuyến Mãi Nạp Tiền (Phần mềm Marketing).
 *   Sét Nét Cho Đủ Chuẩn Entity Ra Bảng Nạp SQL.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Nẹp Bảng DepositPromotion Cào Nền Cấu Hình PostgreSQL.
/// </summary>
public class DepositPromotionConfiguration : IEntityTypeConfiguration<DepositPromotion>
{
    public void Configure(EntityTypeBuilder<DepositPromotion> builder)
    {
        // 1. Phết Tên Bảng
        builder.ToTable("deposit_promotions");

        // 2. Chấm Tọa Độ PK ID
        builder.HasKey(p => p.Id);

        // Buộc Cột Chặn Mức Tiền Tối Thiểu Phải Nạp Không Rỗng
        builder.Property(p => p.MinAmountVnd)
            .IsRequired();

        // Buộc Cột Quà Thưởng Cứng
        builder.Property(p => p.BonusDiamond)
            .IsRequired();

        // Buộc Bool Tắt Bật Banner Marketing
        builder.Property(p => p.IsActive)
            .IsRequired();
            
        // Cắm Index Chạy Thủng Cột Tìm Sớm Mức Giá Rẻ Nào Nạp Web Òa Phù Hợp Nhanh Chóng Để Kéo DB Load Kịp Bắn FrontEnd.
        builder.HasIndex(p => p.MinAmountVnd);
    }
}
