using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial mapper chuyển đổi ChatMessage DTO/document.
public partial class MongoChatMessageRepository
{
    /// <summary>
    /// Map ChatMessageDto sang document Mongo.
    /// Luồng xử lý: chuẩn hóa id, map các payload phụ (payment/media/call) và giữ metadata message.
    /// </summary>
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

    /// <summary>
    /// Map ChatMessageDocument sang DTO.
    /// Luồng xử lý: ánh xạ đầy đủ trường chính và các payload phụ trợ.
    /// </summary>
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

    /// <summary>
    /// Map payment payload DTO sang document.
    /// Luồng xử lý: null-in/null-out để biểu diễn đúng loại message không có thanh toán.
    /// </summary>
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

    /// <summary>
    /// Map payment payload document sang DTO.
    /// Luồng xử lý: giữ nguyên proposal và expires_at để xử lý nghiệp vụ offer.
    /// </summary>
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

    /// <summary>
    /// Map call payload DTO sang document.
    /// Luồng xử lý: chuẩn hóa enum call type thành chuỗi lowercase và fallback duration = 0 khi null.
    /// </summary>
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

    /// <summary>
    /// Map call payload document sang DTO.
    /// Luồng xử lý: parse call_type về enum và ánh xạ dữ liệu cuộc gọi cơ bản.
    /// </summary>
    private static CallSessionDto? MapCallPayloadToDto(ChatCallPayload? payload)
    {
        if (payload == null) return null;

        return new CallSessionDto
        {
            Id = payload.SessionId,
            Type = payload.CallType == "video" ? TarotNow.Domain.Enums.CallType.Video : TarotNow.Domain.Enums.CallType.Audio,
            // Edge case: giá trị khác "video" mặc định về audio để giữ khả năng tương thích dữ liệu cũ.
            EndReason = payload.EndReason,
            DurationSeconds = payload.DurationSeconds
        };
    }
}
