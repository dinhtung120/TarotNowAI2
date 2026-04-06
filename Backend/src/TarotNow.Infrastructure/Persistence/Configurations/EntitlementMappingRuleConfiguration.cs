/*
 * ===================================================================
 * FILE: EntitlementMappingRuleConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bảng Ma Trận Quy Đổi Quyền Lợi Chéo Nhau. Nơi quy định các cấu hình Rule nhảy bật (Cross-key Downgrade).
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class EntitlementMappingRuleConfiguration : IEntityTypeConfiguration<EntitlementMappingRule>
{
    public void Configure(EntityTypeBuilder<EntitlementMappingRule> builder)
    {
        builder.ToTable("entitlement_mapping_rules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TargetKey)
            .IsRequired()
            .HasMaxLength(50);

        // Thiết lập Hệ Số Tỷ Lệ Đổi - Tiền Kiểu Số Thực Chính Xác Sql Decimal (18 Số Thập, Lỗ Hổng 4).
        builder.Property(x => x.Ratio)
            .HasPrecision(18, 4);

        // Độc nhất Vô nhị. Không thể có Sinh 2 luật Tương Tự Nhau Trỏ Đồng (Chống Lỗi Thuật Toán Map Đệ Quy).
        builder.HasIndex(x => new { x.SourceKey, x.TargetKey })
            .IsUnique()
            .HasDatabaseName("IX_mapping_rules_source_target");
            
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
