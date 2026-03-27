using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository
{
    private static ChatMediaPayload? MapMediaPayloadToDocument(MediaPayloadDto? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new ChatMediaPayload
        {
            Url = payload.Url,
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

    private static MediaPayloadDto? MapMediaPayloadToDto(ChatMediaPayload? payload)
    {
        if (payload == null)
        {
            return null;
        }

        return new MediaPayloadDto
        {
            Url = payload.Url,
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
