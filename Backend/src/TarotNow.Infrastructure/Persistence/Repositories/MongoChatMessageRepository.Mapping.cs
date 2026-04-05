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
            CallPayload = MapCallPayloadToDocument(dto.CallPayload),
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
            CallPayload = MapCallPayloadToDto(doc.CallPayload),
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

    private static ChatCallPayload? MapCallPayloadToDocument(CallSessionDto? payload)
    {
        if (payload == null) return null;

        return new ChatCallPayload
        {
            SessionId = payload.Id,
            CallType = payload.Type.ToString().ToLower(),
            EndReason = payload.EndReason,
            DurationSeconds = payload.DurationSeconds ?? 0
        };
    }

    private static CallSessionDto? MapCallPayloadToDto(ChatCallPayload? payload)
    {
        if (payload == null) return null;

        return new CallSessionDto
        {
            Id = payload.SessionId,
            Type = payload.CallType == "video" ? TarotNow.Domain.Enums.CallType.Video : TarotNow.Domain.Enums.CallType.Audio,
            EndReason = payload.EndReason,
            DurationSeconds = payload.DurationSeconds
        };
    }
}
