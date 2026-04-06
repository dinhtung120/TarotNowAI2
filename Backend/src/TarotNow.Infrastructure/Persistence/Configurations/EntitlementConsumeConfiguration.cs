/*
 * ===================================================================
 * FILE: EntitlementConsumeConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bảng ghi nhận Lịch sử Đốt quyền Lợi ("Sổ cái"). 
 *   Thiết kế chuẩn Append-Only, đảm bảo không ai đớp được quyền mà không rơi lại dấu vết.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class EntitlementConsumeConfiguration : IEntityTypeConfiguration<EntitlementConsume>
{
    public void Configure(EntityTypeBuilder<EntitlementConsume> builder)
    {
        builder.ToTable("entitlement_consumes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntitlementKey)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ReferenceSource)
            .HasMaxLength(50);

        builder.Property(x => x.ReferenceId)
            .HasMaxLength(100);

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);

        // Map Foreign Keys
        builder.HasOne(x => x.Bucket)
            .WithMany()
            .HasForeignKey(x => x.BucketId)
            .OnDelete(DeleteBehavior.Restrict); // Cấm Xóa Rổ Khi Đã Có Ghi Vết Trong Này (Bảo Lãnh Dữ Liệu Ledger)

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Mất Khách thì Mất Hết.

        // ==== CÁC CHỈ MỤC INDEX ====
        // 1. NGĂN CHẶN CHẮC CHẮN NẠP TRÙNG LẦN 2 TỪ MOBILE BẤM LOẠN
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("IX_entitlement_consumes_idempotency_key");
            
        // 2. Chăm sóc Admin Report Truy Sổ - Liệt kê Khách vừa đốt Trái Bài nào
        builder.HasIndex(x => new { x.UserId, x.ConsumedAt })
            .HasDatabaseName("IX_entitlement_consumes_user_time");
    }
}
