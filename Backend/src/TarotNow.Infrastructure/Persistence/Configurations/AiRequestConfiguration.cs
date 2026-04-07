

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class AiRequestConfiguration : IEntityTypeConfiguration<AiRequest>
{
    public void Configure(EntityTypeBuilder<AiRequest> builder)
    {
        
        builder.ToTable("ai_requests");

        
        builder.HasKey(x => x.Id);

        
        builder.Property(x => x.UserId).HasColumnName("user_id");

        
        builder.Property(x => x.ReadingSessionRef)
            .HasColumnName("reading_session_ref")
            .IsRequired()
            .HasMaxLength(36);

        
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasDefaultValue("requested") 
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

        
        
        
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL")
            .HasDatabaseName("idx_ai_requests_idempotency");

        
        builder.HasIndex(x => x.ReadingSessionRef)
            .HasDatabaseName("idx_ai_requests_reading");

        
        builder.HasIndex(x => new { x.Status, x.CreatedAt })
            .HasDatabaseName("idx_ai_requests_status");
    }
}
