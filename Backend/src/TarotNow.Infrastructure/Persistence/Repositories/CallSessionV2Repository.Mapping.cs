using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CallSessionV2Repository
{
    private static CallSessionV2Document ToDocument(CallSessionV2Dto dto)
    {
        return new CallSessionV2Document
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ConversationId = dto.ConversationId,
            RoomName = dto.RoomName,
            InitiatorId = dto.InitiatorId,
            CalleeId = dto.CalleeId,
            Type = dto.Type,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            AcceptedAt = dto.AcceptedAt,
            ConnectedAt = dto.ConnectedAt,
            EndedAt = dto.EndedAt,
            EndReason = dto.EndReason,
            InitiatorJoinedAt = dto.InitiatorJoinedAt,
            CalleeJoinedAt = dto.CalleeJoinedAt,
            IsLogCreated = dto.IsLogCreated,
        };
    }

    private static CallSessionV2Dto ToDto(CallSessionV2Document document)
    {
        return new CallSessionV2Dto
        {
            Id = document.Id,
            ConversationId = document.ConversationId,
            RoomName = document.RoomName,
            InitiatorId = document.InitiatorId,
            CalleeId = document.CalleeId,
            Type = document.Type,
            Status = document.Status,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt,
            AcceptedAt = document.AcceptedAt,
            ConnectedAt = document.ConnectedAt,
            EndedAt = document.EndedAt,
            EndReason = document.EndReason,
            InitiatorJoinedAt = document.InitiatorJoinedAt,
            CalleeJoinedAt = document.CalleeJoinedAt,
            IsLogCreated = document.IsLogCreated,
        };
    }
}
