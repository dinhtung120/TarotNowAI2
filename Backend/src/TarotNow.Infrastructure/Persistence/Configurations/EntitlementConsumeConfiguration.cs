

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
            .HasMaxLength(255); 

        
        builder.HasOne(x => x.Bucket)
            .WithMany()
            .HasForeignKey(x => x.BucketId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        
        
        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("IX_entitlement_consumes_idempotency_key");
            
        
        builder.HasIndex(x => new { x.UserId, x.ConsumedAt })
            .HasDatabaseName("IX_entitlement_consumes_user_time");
    }
}
