using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoConversationRepository
{
    private static ConversationDocument ToDocument(ConversationDto dto)
    {
        return new ConversationDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            ReaderId = dto.ReaderId,
            Status = dto.Status,
            LastMessageAt = dto.LastMessageAt,
            OfferExpiresAt = dto.OfferExpiresAt,
            UnreadCount = new UnreadCount { User = dto.UnreadCountUser, Reader = dto.UnreadCountReader },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    private static ConversationDto ToDto(ConversationDocument doc)
    {
        return new ConversationDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            ReaderId = doc.ReaderId,
            Status = doc.Status,
            LastMessageAt = doc.LastMessageAt,
            OfferExpiresAt = doc.OfferExpiresAt,
            UnreadCountUser = doc.UnreadCount.User,
            UnreadCountReader = doc.UnreadCount.Reader,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
