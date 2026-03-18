using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class ChatFinanceSessionConfiguration : IEntityTypeConfiguration<ChatFinanceSession>
{
    public void Configure(EntityTypeBuilder<ChatFinanceSession> builder)
    {
        builder.HasIndex(x => x.ConversationRef)
            .HasDatabaseName("ix_chat_finance_sessions_conversation_ref")
            .IsUnique();
    }
}
