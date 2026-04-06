/*
 * ===================================================================
 * FILE: SubscriptionPlanConfiguration.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.Configurations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Ánh xạ Entity SubscriptionPlan xuống cấu trúc Table trong PostgreSQL.
 *   Xác định rõ ràng khóa chính (Id), kiểu dữ liệu (Varchar, Jsonb), độ dài tối đa giới hạn cho các cột, và Index.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("subscription_plans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Trường chứa danh sách các Quyền lợi (được định dạng kiểu cấu trúc chuỗi mảng JSON)
        // Để kiểu column là jsonb thay vì string thường sẽ giúp Postgres tối ưu việc đọc / tra cứu nội dung JSON nhanh hơn.
        builder.Property(x => x.EntitlementsJson)
            .IsRequired()
            .HasColumnType("jsonb");

        // Index tìm kiếm nhanh khi User load list Gói Đang Bán
        builder.HasIndex(x => x.IsActive)
            .HasFilter("is_active = true") // Lọc Filter giảm kích thước Index xuống cực nhỏ
            .HasDatabaseName("IX_subscription_plans_is_active");
            
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
