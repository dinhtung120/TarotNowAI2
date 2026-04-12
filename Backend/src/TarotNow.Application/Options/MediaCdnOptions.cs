namespace TarotNow.Application.Options;

/// <summary>
/// CDN public cho ảnh catalog (lá bài): ghép với đường dẫn tương đối trong Mongo.
/// </summary>
public sealed class MediaCdnOptions
{
    /// <summary>VD https://cdn.example.com — không slash cuối.</summary>
    public string PublicBaseUrl { get; set; } = string.Empty;
}
