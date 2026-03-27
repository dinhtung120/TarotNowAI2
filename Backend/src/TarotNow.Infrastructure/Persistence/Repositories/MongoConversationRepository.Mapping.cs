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
            Confirm = dto.Confirm == null
                ? null
                : new ConversationConfirm
                {
                    UserAt = dto.Confirm.UserAt,
                    ReaderAt = dto.Confirm.ReaderAt,
                    RequestedBy = dto.Confirm.RequestedBy,
                    RequestedAt = dto.Confirm.RequestedAt,
                    AutoResolveAt = dto.Confirm.AutoResolveAt
                },
            LastMessageAt = dto.LastMessageAt,
            OfferExpiresAt = dto.OfferExpiresAt,
            SlaHours = dto.SlaHours,
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
            Confirm = doc.Confirm == null
                ? null
                : new ConversationConfirmDto
                {
                    UserAt = doc.Confirm.UserAt,
                    ReaderAt = doc.Confirm.ReaderAt,
                    RequestedBy = doc.Confirm.RequestedBy,
                    RequestedAt = doc.Confirm.RequestedAt,
                    AutoResolveAt = doc.Confirm.AutoResolveAt
                },
            LastMessageAt = doc.LastMessageAt,
            OfferExpiresAt = doc.OfferExpiresAt,
            SlaHours = doc.SlaHours,
            UnreadCountUser = doc.UnreadCount?.User ?? 0,
            UnreadCountReader = doc.UnreadCount?.Reader ?? 0,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
