

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity GachaRewardLog.
public class GachaRewardLogConfiguration : IEntityTypeConfiguration<GachaRewardLog>
{
    /// <summary>
    /// Cấu hình mapping bảng gacha_reward_logs.
    /// Luồng xử lý: map cột truy vết reward, tạo index idempotency và index phục vụ thống kê lịch sử quay.
    /// </summary>
    public void Configure(EntityTypeBuilder<GachaRewardLog> builder)
    {
        builder.ToTable("gacha_reward_logs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.BannerId).IsRequired();
        builder.Property(x => x.BannerItemId).IsRequired();

        builder.Property(x => x.OddsVersion).IsRequired().HasMaxLength(32);
        builder.Property(x => x.SpentDiamond).IsRequired();

        builder.Property(x => x.Rarity).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.RewardValue).IsRequired().HasMaxLength(256);

        builder.Property(x => x.PityCountAtSpin).IsRequired();
        builder.Property(x => x.WasPityTriggered).IsRequired();

        builder.Property(x => x.RngSeed).HasMaxLength(128);

        builder.Property(x => x.IdempotencyKey).IsRequired().HasMaxLength(128);
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
        // Unique idempotency key để chống ghi log trùng do retry request.

        builder.HasIndex(x => new { x.UserId, x.CreatedAt }).IsDescending(false, true);
        // Index timeline user để truy vấn lịch sử quay mới nhất.

        builder.HasIndex(x => new { x.UserId, x.BannerId, x.Rarity, x.CreatedAt });
        // Index chi tiết theo banner+rarity phục vụ thống kê pity/rarity.
    }
}
