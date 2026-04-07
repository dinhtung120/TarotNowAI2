

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class ChatFinanceSessionConfiguration : IEntityTypeConfiguration<ChatFinanceSession>
{
    public void Configure(EntityTypeBuilder<ChatFinanceSession> builder)
    {
        builder.ToTable("chat_finance_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.ConversationRef)
            .HasColumnName("conversation_ref")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.ReaderId)
            .HasColumnName("reader_id");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.TotalFrozen)
            .HasColumnName("total_frozen");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        
        builder.HasIndex(x => x.ConversationRef)
            .HasDatabaseName("ix_chat_finance_sessions_conversation_ref")
            .IsUnique();
    }
}
