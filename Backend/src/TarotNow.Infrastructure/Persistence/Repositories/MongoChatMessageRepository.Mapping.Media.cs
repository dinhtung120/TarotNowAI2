using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial mapper chuyển đổi payload media giữa DTO và document.
public partial class MongoChatMessageRepository
{
    /// <summary>
    /// Map payload media DTO sang document Mongo.
    /// Luồng xử lý: trả null khi payload rỗng, ngược lại copy toàn bộ metadata media.
    /// </summary>
    private static ChatMediaPayload? MapMediaPayloadToDocument(MediaPayloadDto? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new ChatMediaPayload
        {
            Url = payload.Url,
            ObjectKey = payload.ObjectKey,
            MimeType = payload.MimeType,
            SizeBytes = payload.SizeBytes,
            DurationMs = payload.DurationMs,
            Width = payload.Width,
            Height = payload.Height,
            ThumbnailUrl = payload.ThumbnailUrl,
            Description = payload.Description,
            ProcessingStatus = payload.ProcessingStatus
        };
    }

    /// <summary>
    /// Map payload media document sang DTO.
    /// Luồng xử lý: trả null khi không có media_payload để tránh sinh object rỗng giả.
    /// </summary>
    private static MediaPayloadDto? MapMediaPayloadToDto(ChatMediaPayload? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new MediaPayloadDto
        {
            Url = payload.Url,
            ObjectKey = payload.ObjectKey,
            MimeType = payload.MimeType,
            SizeBytes = payload.SizeBytes,
            DurationMs = payload.DurationMs,
            Width = payload.Width,
            Height = payload.Height,
            ThumbnailUrl = payload.ThumbnailUrl,
            Description = payload.Description,
            ProcessingStatus = payload.ProcessingStatus
        };
    }
}
