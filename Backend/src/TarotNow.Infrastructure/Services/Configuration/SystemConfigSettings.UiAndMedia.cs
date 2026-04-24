using System.Text.Json;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Page size readers directory FE.
    public int UiReadersDirectoryPageSize => ClampInt(
        ReadInt(["ui.readers.directory_page_size"], _options.Ui.Readers.DirectoryPageSize),
        min: 1,
        max: 100);

    // Số readers featured FE.
    public int UiReadersFeaturedLimit => ClampInt(
        ReadInt(["ui.readers.featured_limit"], _options.Ui.Readers.FeaturedLimit),
        min: 1,
        max: 50);

    // Debounce search FE.
    public int UiSearchDebounceMs => ClampInt(
        ReadInt(["ui.search.debounce_ms"], _options.Ui.Search.DebounceMs),
        min: 0,
        max: 5000);

    // Stale time readers directory FE.
    public int UiReadersDirectoryStaleMs => ClampInt(
        ReadInt(["ui.readers.directory_stale_ms"], _options.Ui.Readers.DirectoryStaleMs),
        min: 0,
        max: 600_000);

    // Stale time prefetch inbox FE.
    public int UiPrefetchChatInboxStaleMs => ClampInt(
        ReadInt(["ui.prefetch.chat_inbox_stale_ms"], _options.Ui.Prefetch.ChatInboxStaleMs),
        min: 0,
        max: 600_000);

    // Giới hạn ảnh upload (bytes).
    public long MediaUploadMaxImageBytes => ClampLong(
        ReadLong(["media.upload.max_image_bytes"], _options.MediaUpload.MaxImageBytes),
        min: 1024,
        max: 100_000_000);

    // Giới hạn voice upload (bytes).
    public long MediaUploadMaxVoiceBytes => ClampLong(
        ReadLong(["media.upload.max_voice_bytes"], _options.MediaUpload.MaxVoiceBytes),
        min: 1024,
        max: 100_000_000);

    // Giới hạn thời lượng voice upload (ms).
    public int MediaUploadMaxVoiceDurationMs => ClampInt(
        ReadInt(["media.upload.max_voice_duration_ms"], _options.MediaUpload.MaxVoiceDurationMs),
        min: 1000,
        max: 3_600_000);

    // Mục tiêu kích thước ảnh sau nén (bytes).
    public long MediaUploadImageCompressionTargetBytes => ClampLong(
        ReadLong(["media.upload.image_compression_target_bytes"], _options.MediaUpload.ImageCompressionTargetBytes),
        min: 1024,
        max: 50_000_000);

    // Danh sách bước nén ảnh theo thứ tự fallback.
    public IReadOnlyList<MediaImageCompressionStep> MediaUploadImageCompressionSteps
    {
        get
        {
            var parsed = TryReadImageCompressionStepsFromDb();
            if (parsed.Count > 0)
            {
                return parsed;
            }

            return BuildImageCompressionFallbackFromOptions();
        }
    }

    // Retry upload attempts.
    public int MediaUploadRetryAttempts => ClampInt(
        ReadInt(["media.upload.retry_attempts"], _options.MediaUpload.RetryAttempts),
        min: 0,
        max: 10);

    // Retry upload delay.
    public int MediaUploadRetryDelayMs => ClampInt(
        ReadInt(["media.upload.retry_delay_ms"], _options.MediaUpload.RetryDelayMs),
        min: 0,
        max: 10_000);

    private IReadOnlyList<MediaImageCompressionStep> TryReadImageCompressionStepsFromDb()
    {
        var raw = ReadString("media.upload.image_compression_steps");
        if (string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        try
        {
            var parsed = JsonSerializer.Deserialize<List<MediaImageCompressionStep>>(raw, JsonOptions);
            return NormalizeImageCompressionSteps(parsed);
        }
        catch
        {
            // Bỏ qua parse lỗi và fallback sang options.
            return [];
        }
    }

    private IReadOnlyList<MediaImageCompressionStep> BuildImageCompressionFallbackFromOptions()
    {
        var fallback = _options.MediaUpload.ImageCompressionSteps
            .Select(step => new MediaImageCompressionStep
            {
                InitialQuality = step.InitialQuality,
                MaxSizeMb = step.MaxSizeMb,
                MaxWidthOrHeight = step.MaxWidthOrHeight
            })
            .ToArray();

        return NormalizeImageCompressionSteps(fallback);
    }

    private static IReadOnlyList<MediaImageCompressionStep> NormalizeImageCompressionSteps(
        IEnumerable<MediaImageCompressionStep>? rawSteps)
    {
        if (rawSteps is null)
        {
            return [];
        }

        var normalized = rawSteps
            .Select(step => new MediaImageCompressionStep
            {
                InitialQuality = ClampDouble(step.InitialQuality, 0.05, 1.0),
                MaxSizeMb = ClampDouble(step.MaxSizeMb, 0.01, 10.0),
                MaxWidthOrHeight = ClampInt(step.MaxWidthOrHeight, 128, 4096)
            })
            .Take(10)
            .ToArray();

        return normalized.Length > 0 ? normalized : [];
    }
}
