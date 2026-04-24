namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static IEnumerable<SystemConfigDefinition> BuildRuntimeOperationalScalarDefinitions()
    {
        return
        [
            Scalar("chat.payment_offer.default_amount", "10", "Diamond mặc định đề xuất trong payment offer.", ValidateLong(1, 1_000_000)),
            Scalar("chat.payment_offer.max_note_length", "100", "Độ dài tối đa ghi chú payment offer.", ValidateInt(1, 4000)),
            Scalar("chat.history.page_size", "50", "Page size lịch sử message trong chat room.", ValidateInt(1, 200)),
            Scalar("chat.participants.default_page_size", "50", "Page size mặc định khi query conversation ids theo participant.", ValidateInt(1, 500)),
            Scalar("chat.participants.max_page_size", "200", "Page size tối đa khi query conversation ids theo participant.", ValidateInt(1, 1000)),
            Scalar("realtime.negotiation_timeout_ms", "8000", "Timeout negotiation SignalR theo ms.", ValidateInt(500, 120000)),
            Scalar("realtime.presence_negotiation_cooldown_ms", "45000", "Cooldown retry presence realtime khi lỗi negotiation.", ValidateInt(0, 600000)),
            Scalar("realtime.chat_unauthorized_cooldown_ms", "60000", "Cooldown retry chat realtime khi unauthorized.", ValidateInt(0, 600000)),
            Scalar("realtime.server_timeout_ms", "120000", "SignalR server timeout theo ms.", ValidateInt(5000, 600000)),
            Scalar("realtime.chat.typing_clear_ms", "2500", "Thời gian clear typing indicator (ms).", ValidateInt(100, 60000)),
            Scalar("realtime.chat.invalidate_debounce_ms", "1000", "Debounce invalidate query realtime (ms).", ValidateInt(0, 10000)),
            Scalar("realtime.chat.initial_load_guard_ms", "2000", "Guard ms sau initial load trước khi refetch conversation.", ValidateInt(0, 60000)),
            Scalar("realtime.chat.app_start_guard_ms", "3000", "Guard ms trong vài giây đầu app start để giảm invalidate dồn dập.", ValidateInt(0, 60000)),
            Scalar("operational.http.client_timeout_ms", "8000", "Timeout mặc định client fetch (ms).", ValidateInt(500, 120000)),
            Scalar("operational.http.server_timeout_ms", "8000", "Timeout mặc định server proxy request (ms).", ValidateInt(500, 120000)),
            Scalar("operational.http.min_timeout_ms", "1000", "Timeout tối thiểu cho HTTP helpers (ms).", ValidateInt(100, 60000)),
            Scalar("operational.runtime_policies.timeout_ms", "8000", "Timeout fetch runtime policies (ms).", ValidateInt(500, 120000)),
            Scalar("operational.runtime_policies.stale_ms", "15000", "Stale time runtime policies cache (ms).", ValidateInt(1000, 600000)),
            Scalar("operational.redis.connect_timeout_ms", "2000", "Redis connect timeout (ms).", ValidateInt(100, 60000)),
            Scalar("operational.redis.sync_timeout_ms", "2000", "Redis sync timeout (ms).", ValidateInt(100, 60000)),
            Scalar("operational.redis.connect_retry", "1", "Số lần retry kết nối Redis.", ValidateInt(0, 20)),
            Scalar("operational.ai.timeout_seconds", "30", "Timeout request AI (giây).", ValidateInt(1, 600)),
            Scalar("operational.ai.max_retries", "2", "Số lần retry tối đa request AI.", ValidateInt(0, 20)),
            Scalar("operational.ai.streaming_retry_base_delay_ms", "200", "Base delay retry streaming AI (ms).", ValidateInt(10, 60000)),
            Scalar("operational.ai.streaming_temperature", "0.7", "Temperature mặc định cho streaming AI.", ValidateDecimal(0m, 2m)),
            Scalar("operational.outbox.batch_size", "50", "Số message outbox claim mỗi batch.", ValidateInt(1, 5000)),
            Scalar("operational.outbox.max_retry_attempts", "12", "Số lần retry outbox trước dead-letter.", ValidateInt(1, 100)),
            Scalar("operational.outbox.lock_timeout_seconds", "120", "Outbox processing lock timeout (giây).", ValidateInt(30, 3600)),
            Scalar("operational.outbox.max_backoff_seconds", "300", "Outbox retry backoff tối đa (giây).", ValidateInt(1, 3600)),
            Scalar("operational.outbox.poll_interval_seconds", "5", "Chu kỳ poll outbox worker (giây).", ValidateInt(1, 300))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildUiAndMediaScalarDefinitions()
    {
        return
        [
            Scalar("ui.readers.directory_page_size", "12", "Page size mặc định readers directory FE.", ValidateInt(1, 100)),
            Scalar("ui.readers.featured_limit", "4", "Số reader featured mặc định FE.", ValidateInt(1, 50)),
            Scalar("ui.search.debounce_ms", "300", "Debounce search input FE (ms).", ValidateInt(0, 5000)),
            Scalar("ui.readers.directory_stale_ms", "30000", "Stale time readers directory FE (ms).", ValidateInt(0, 600000)),
            Scalar("ui.prefetch.chat_inbox_stale_ms", "30000", "Stale time prefetch inbox FE (ms).", ValidateInt(0, 600000)),
            Scalar("media.upload.max_image_bytes", "10485760", "Giới hạn dung lượng ảnh upload (bytes).", ValidateLong(1024, 100_000_000)),
            Scalar("media.upload.max_voice_bytes", "5242880", "Giới hạn dung lượng voice upload (bytes).", ValidateLong(1024, 100_000_000)),
            Scalar("media.upload.max_voice_duration_ms", "600000", "Giới hạn thời lượng voice upload (ms).", ValidateInt(1000, 3_600_000)),
            Scalar("media.upload.image_compression_target_bytes", "81920", "Mục tiêu kích thước ảnh sau nén (bytes).", ValidateLong(1024, 50_000_000)),
            Scalar("media.upload.retry_attempts", "3", "Số lần retry upload mặc định.", ValidateInt(0, 10)),
            Scalar("media.upload.retry_delay_ms", "350", "Delay retry upload mặc định (ms).", ValidateInt(0, 10000))
        ];
    }

    private static object[] BuildDefaultImageCompressionSteps()
    {
        return
        [
            new { initialQuality = 0.68, maxSizeMb = 0.15, maxWidthOrHeight = 1440 },
            new { initialQuality = 0.58, maxSizeMb = 0.10, maxWidthOrHeight = 1200 },
            new { initialQuality = 0.48, maxSizeMb = 0.06, maxWidthOrHeight = 960 },
            new { initialQuality = 0.38, maxSizeMb = 0.03, maxWidthOrHeight = 640 }
        ];
    }
}
