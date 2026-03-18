using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Configurations;

public class ChatQuestionItemConfiguration : IEntityTypeConfiguration<ChatQuestionItem>
{
    public void Configure(EntityTypeBuilder<ChatQuestionItem> builder)
    {
        builder.HasIndex(x => x.IdempotencyKey)
            .HasDatabaseName("ix_chat_question_items_idempotency_key")
            .IsUnique()
            .HasFilter("idempotency_key IS NOT NULL");
    }
}
