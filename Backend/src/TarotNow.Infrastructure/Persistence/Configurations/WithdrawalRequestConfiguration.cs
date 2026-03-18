using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public sealed class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
{
    public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.BusinessDateUtc })
            .HasDatabaseName("ix_withdrawal_one_per_day_active")
            .IsUnique()
            .HasFilter("status in ('pending','approved')");
    }
}
