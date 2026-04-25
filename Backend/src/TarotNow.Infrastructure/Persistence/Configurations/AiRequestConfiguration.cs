

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

// Cấu hình EF mapping cho entity AiRequest.
public class AiRequestConfiguration : IEntityTypeConfiguration<AiRequest>
{
    // Tên bảng lưu AiRequest.
    private const string TableName = "ai_requests";

    // Trạng thái mặc định khi tạo mới request.
    private const string DefaultStatus = "requested";

    // Tên index idempotency key.
    private const string IdempotencyIndexName = "idx_ai_requests_idempotency";

    // Tên index reading session ref.
    private const string ReadingIndexName = "idx_ai_requests_reading";

    // Tên index status + created_at.
    private const string StatusIndexName = "idx_ai_requests_status";

    // Filter index cho idempotency key khác null.
    private const string IdempotencyFilter = "idempotency_key IS NOT NULL";

    /// <summary>
    /// Cấu hình mapping tổng thể cho AiRequest.
    /// Luồng xử lý: set table/key rồi tách cấu hình cột và index sang helper methods.
    /// </summary>
    public void Configure(EntityTypeBuilder<AiRequest> builder)
    {
        builder.ToTable(TableName);
        builder.HasKey(x => x.Id);

        ConfigureColumns(builder);
        ConfigureIndexes(builder);
    }

    /// <summary>
    /// Cấu hình chi tiết cột của bảng ai_requests.
    /// Luồng xử lý: map column names, kiểu bắt buộc, max length và default values theo domain.
    /// </summary>
    private static void ConfigureColumns(EntityTypeBuilder<AiRequest> builder)
    {
        builder.Property(x => x.UserId).HasColumnName("user_id");

        builder.Property(x => x.ReadingSessionRef)
            .HasColumnName("reading_session_ref")
            .HasColumnType("uuid");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasDefaultValue(DefaultStatus)
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
    }

    /// <summary>
    /// Cấu hình index phục vụ truy vấn và bảo đảm idempotency.
    /// Luồng xử lý: tạo unique index idempotency có filter, cùng index reading ref và status+created.
    /// </summary>
    private static void ConfigureIndexes(EntityTypeBuilder<AiRequest> builder)
    {
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasFilter(IdempotencyFilter)
            .HasDatabaseName(IdempotencyIndexName);

        builder.HasIndex(x => x.ReadingSessionRef)
            .HasDatabaseName(ReadingIndexName);

        builder.HasIndex(x => new { x.Status, x.CreatedAt })
            .HasDatabaseName(StatusIndexName);
    }
}
