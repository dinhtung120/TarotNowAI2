using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class ReadingSessionConfiguration : IEntityTypeConfiguration<ReadingSession>
{
    public void Configure(EntityTypeBuilder<ReadingSession> builder)
    {
        builder.ToTable("reading_sessions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        // Enum -> String conversion
        builder.Property(e => e.SpreadType)
            .HasColumnName("spread_type")
            .IsRequired();

        // Câu hỏi tùy chọn từ người dùng (nullable)
        builder.Property(e => e.Question)
            .HasColumnName("question")
            .HasMaxLength(500);

        builder.Property(e => e.CardsDrawn)
            .HasColumnName("cards_drawn")
            .HasColumnType("jsonb");

        // Loại tiền sử dụng (Gold/Diamond), nullable vì daily_1 có thể miễn phí
        builder.Property(e => e.CurrencyUsed)
            .HasColumnName("currency_used")
            .HasMaxLength(20);

        // Số tiền đã trừ, mặc định 0 cho các phiên miễn phí
        builder.Property(e => e.AmountCharged)
            .HasColumnName("amount_charged")
            .HasDefaultValue(0L);

        builder.Property(e => e.IsCompleted)
            .HasColumnName("is_completed")
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.CompletedAt)
            .HasColumnName("completed_at");

        // Index for performance
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => new { e.UserId, e.CreatedAt });
    }
}
