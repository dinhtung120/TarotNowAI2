using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Mapper chuyển đổi giữa CallSessionDto và CallSessionDocument.
public static class CallSessionDocumentMapper
{
    /// <summary>
    /// Map DTO sang document Mongo để lưu trữ.
    /// Luồng xử lý: chuẩn hóa id, ánh xạ enum sang chuỗi và gán timestamp mặc định khi thiếu dữ liệu.
    /// </summary>
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

    /// <summary>
    /// Map document Mongo sang DTO dùng ở tầng Application.
    /// Luồng xử lý: fallback tính duration từ started/ended nếu document chưa có duration_seconds.
    /// </summary>
    public static CallSessionDto ToDto(CallSessionDocument doc)
    {
        int? duration = doc.DurationSeconds;
        if (!duration.HasValue && doc.StartedAt.HasValue && doc.EndedAt.HasValue)
        {
            duration = (int)(doc.EndedAt.Value - doc.StartedAt.Value).TotalSeconds;
            // Edge case dữ liệu cũ chưa có DurationSeconds, tính lại để không mất thông tin hiển thị.
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

    /// <summary>
    /// Ánh xạ enum trạng thái cuộc gọi sang chuỗi lưu Mongo.
    /// Luồng xử lý: dùng switch để cố định giá trị canonical, fallback requested khi gặp trạng thái lạ.
    /// </summary>
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

    /// <summary>
    /// Parse chuỗi trạng thái từ Mongo về enum.
    /// Luồng xử lý: ánh xạ các giá trị hợp lệ, fallback Requested để đảm bảo backward compatibility.
    /// </summary>
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
