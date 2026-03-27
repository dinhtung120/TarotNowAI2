using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository
{
    private static ChatMessageDocument ToDocument(ChatMessageDto dto)
    {
        return new ChatMessageDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            Type = dto.Type,
            Content = dto.Content,
            PaymentPayload = MapPaymentPayloadToDocument(dto.PaymentPayload),
            MediaPayload = MapMediaPayloadToDocument(dto.MediaPayload),
            IsRead = dto.IsRead,
            CreatedAt = dto.CreatedAt
        };
    }

    private static ChatMessageDto ToDto(ChatMessageDocument doc)
    {
        return new ChatMessageDto
        {
            Id = doc.Id,
            ConversationId = doc.ConversationId,
            SenderId = doc.SenderId,
            Type = doc.Type,
            Content = doc.Content,
            PaymentPayload = MapPaymentPayloadToDto(doc.PaymentPayload),
            MediaPayload = MapMediaPayloadToDto(doc.MediaPayload),
            IsRead = doc.IsRead,
            CreatedAt = doc.CreatedAt
        };
    }

    private static ChatPaymentPayload? MapPaymentPayloadToDocument(PaymentPayloadDto? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new ChatPaymentPayload
        {
            AmountDiamond = payload.AmountDiamond,
            ProposalId = payload.ProposalId,
            ExpiresAt = payload.ExpiresAt
        };
    }

    private static PaymentPayloadDto? MapPaymentPayloadToDto(ChatPaymentPayload? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new PaymentPayloadDto
        {
            AmountDiamond = payload.AmountDiamond,
            ProposalId = payload.ProposalId,
            ExpiresAt = payload.ExpiresAt
        };
    }
}
