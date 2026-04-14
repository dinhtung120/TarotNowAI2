

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity RefreshToken.
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <summary>
    /// Cấu hình mapping bảng refresh_tokens.
    /// Luồng xử lý: map key/cột chính, unique token, metadata tạo token và quan hệ user.
    /// </summary>
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(rt => rt.Token).IsUnique();
        // Đảm bảo mỗi token chỉ xuất hiện một lần trong hệ thống.

        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(64);

        builder.Property(rt => rt.CreatedDeviceId)
            .HasMaxLength(128);

        builder.Property(rt => rt.CreatedUserAgentHash)
            .HasMaxLength(128);

        builder.Property(rt => rt.RevocationReason)
            .HasMaxLength(64);

        builder.Property(rt => rt.LastRotateIdempotencyKey)
            .HasMaxLength(128);

        builder.Property(rt => rt.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(rt => rt.SessionId);
        builder.HasIndex(rt => rt.FamilyId);
        builder.HasIndex(rt => new { rt.FamilyId, rt.ParentTokenId });
        builder.HasIndex(rt => rt.ParentTokenId);
        builder.HasIndex(rt => rt.ReplacedByTokenId);
        builder.HasIndex(rt => rt.UsedAtUtc);

        builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        // Xóa user sẽ xóa toàn bộ refresh token liên quan.
    }
}
