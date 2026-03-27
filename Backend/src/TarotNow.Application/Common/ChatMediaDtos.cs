namespace TarotNow.Application.Common;

public class MediaPayloadDto
{
    public string Url { get; set; } = string.Empty;

    public string? MimeType { get; set; }

    public long? SizeBytes { get; set; }

    public int? DurationMs { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? Description { get; set; }

    public string? ProcessingStatus { get; set; }
}
