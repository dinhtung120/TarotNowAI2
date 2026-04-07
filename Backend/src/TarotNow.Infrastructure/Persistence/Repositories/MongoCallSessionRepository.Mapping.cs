using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public static class CallSessionDocumentMapper
{
    public static CallSessionDocument ToDocument(CallSessionDto dto)
    {
        return new CallSessionDocument
        {
            Id = !string.IsNullOrEmpty(dto.Id) ? dto.Id : MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            ConversationId = dto.ConversationId,
            InitiatorId = dto.InitiatorId,
            Type = dto.Type == CallType.Audio ? "audio" : "video",
            Status = MapStatus(dto.Status),
            StartedAt = dto.StartedAt,
            EndedAt = dto.EndedAt,
            EndReason = dto.EndReason,
            DurationSeconds = dto.DurationSeconds,
            CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt == default ? DateTime.UtcNow : dto.UpdatedAt
        };
    }

    public static CallSessionDto ToDto(CallSessionDocument doc)
    {
        
        
        int? duration = doc.DurationSeconds;
        if (!duration.HasValue && doc.StartedAt.HasValue && doc.EndedAt.HasValue)
        {
            duration = (int)(doc.EndedAt.Value - doc.StartedAt.Value).TotalSeconds;
        }

        return new CallSessionDto
        {
            Id = doc.Id,
            ConversationId = doc.ConversationId,
            InitiatorId = doc.InitiatorId,
            Type = doc.Type == "video" ? CallType.Video : CallType.Audio,
            Status = ParseStatus(doc.Status),
            StartedAt = doc.StartedAt,
            EndedAt = doc.EndedAt,
            EndReason = doc.EndReason,
            DurationSeconds = duration,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }

    public static string MapStatus(CallSessionStatus status)
    {
        return status switch
        {
            CallSessionStatus.Requested => "requested",
            CallSessionStatus.Accepted => "accepted",
            CallSessionStatus.Rejected => "rejected",
            CallSessionStatus.Ended => "ended",
            _ => "requested"
        };
    }

    public static CallSessionStatus ParseStatus(string status)
    {
        return status switch
        {
            "accepted" => CallSessionStatus.Accepted,
            "rejected" => CallSessionStatus.Rejected,
            "ended" => CallSessionStatus.Ended,
            _ => CallSessionStatus.Requested
        };
    }
}
