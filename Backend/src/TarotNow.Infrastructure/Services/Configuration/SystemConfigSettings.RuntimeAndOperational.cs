using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Lịch reconnect SignalR theo milliseconds.
    public IReadOnlyList<int> RealtimeReconnectScheduleMs
    {
        get
        {
            var fromConfig = NormalizeReconnectSchedule(ReadIntArray("realtime.reconnect_schedule_ms"));
            if (fromConfig.Count > 0)
            {
                return fromConfig;
            }

            var fromOptions = NormalizeReconnectSchedule(_options.Realtime.ReconnectScheduleMs);
            return fromOptions.Count > 0 ? fromOptions : [0, 2000, 5000, 10000, 30000];
        }
    }

    // Timeout negotiation realtime theo ms.
    public int RealtimeNegotiationTimeoutMs => ClampInt(
        ReadInt(["realtime.negotiation_timeout_ms"], _options.Realtime.NegotiationTimeoutMs),
        min: 500,
        max: 120_000);

    // Cooldown retry presence realtime theo ms.
    public int RealtimePresenceNegotiationCooldownMs => ClampInt(
        ReadInt(["realtime.presence_negotiation_cooldown_ms"], _options.Realtime.PresenceNegotiationCooldownMs),
        min: 0,
        max: 600_000);

    // Cooldown retry chat realtime unauthorized theo ms.
    public int RealtimeChatUnauthorizedCooldownMs => ClampInt(
        ReadInt(["realtime.chat_unauthorized_cooldown_ms"], _options.Realtime.ChatUnauthorizedCooldownMs),
        min: 0,
        max: 600_000);

    // Server timeout SignalR theo ms.
    public int RealtimeServerTimeoutMs => ClampInt(
        ReadInt(["realtime.server_timeout_ms"], _options.Realtime.ServerTimeoutMs),
        min: 5_000,
        max: 600_000);

    // Thời gian clear typing indicator (ms).
    public int RealtimeChatTypingClearMs => ClampInt(
        ReadInt(["realtime.chat.typing_clear_ms"], _options.Realtime.ChatTypingClearMs),
        min: 100,
        max: 60_000);

    // Debounce invalidate realtime (ms).
    public int RealtimeChatInvalidateDebounceMs => ClampInt(
        ReadInt(["realtime.chat.invalidate_debounce_ms"], _options.Realtime.ChatInvalidateDebounceMs),
        min: 0,
        max: 10_000);

    // Guard sau initial load trước khi refetch conversation (ms).
    public int RealtimeChatInitialLoadGuardMs => ClampInt(
        ReadInt(["realtime.chat.initial_load_guard_ms"], _options.Realtime.ChatInitialLoadGuardMs),
        min: 0,
        max: 60_000);

    // Guard invalidate trong vài giây đầu app start (ms).
    public int RealtimeChatAppStartGuardMs => ClampInt(
        ReadInt(["realtime.chat.app_start_guard_ms"], _options.Realtime.ChatAppStartGuardMs),
        min: 0,
        max: 60_000);

    // Timeout mặc định client fetch (ms).
    public int OperationalHttpClientTimeoutMs => ClampInt(
        ReadInt(["operational.http.client_timeout_ms"], _options.Operational.Http.ClientTimeoutMs),
        min: 500,
        max: 120_000);

    // Timeout mặc định server-side proxy (ms).
    public int OperationalHttpServerTimeoutMs => ClampInt(
        ReadInt(["operational.http.server_timeout_ms"], _options.Operational.Http.ServerTimeoutMs),
        min: 500,
        max: 120_000);

    // Timeout tối thiểu của HTTP helper (ms).
    public int OperationalHttpMinTimeoutMs => ClampInt(
        ReadInt(["operational.http.min_timeout_ms"], _options.Operational.Http.MinTimeoutMs),
        min: 100,
        max: 60_000);

    // Timeout fetch runtime policies (ms).
    public int OperationalRuntimePoliciesTimeoutMs => ClampInt(
        ReadInt(["operational.runtime_policies.timeout_ms"], _options.Operational.RuntimePolicies.TimeoutMs),
        min: 500,
        max: 120_000);

    // Stale time runtime policies (ms).
    public int OperationalRuntimePoliciesStaleMs => ClampInt(
        ReadInt(["operational.runtime_policies.stale_ms"], _options.Operational.RuntimePolicies.StaleMs),
        min: 1_000,
        max: 600_000);

    // Redis connect timeout (ms).
    public int OperationalRedisConnectTimeoutMs => ClampInt(
        ReadInt(["operational.redis.connect_timeout_ms"], _options.Operational.Redis.ConnectTimeoutMs),
        min: 100,
        max: 60_000);

    // Redis sync timeout (ms).
    public int OperationalRedisSyncTimeoutMs => ClampInt(
        ReadInt(["operational.redis.sync_timeout_ms"], _options.Operational.Redis.SyncTimeoutMs),
        min: 100,
        max: 60_000);

    // Redis connect retry count.
    public int OperationalRedisConnectRetry => ClampInt(
        ReadInt(["operational.redis.connect_retry"], _options.Operational.Redis.ConnectRetry),
        min: 0,
        max: 20);

    // AI timeout theo giây.
    public int OperationalAiTimeoutSeconds => ClampInt(
        ReadInt(["operational.ai.timeout_seconds"], _options.Operational.Ai.TimeoutSeconds),
        min: 1,
        max: 600);

    // AI max retries.
    public int OperationalAiMaxRetries => ClampInt(
        ReadInt(["operational.ai.max_retries"], _options.Operational.Ai.MaxRetries),
        min: 0,
        max: 20);

    // AI streaming retry base delay.
    public int OperationalAiStreamingRetryBaseDelayMs => ClampInt(
        ReadInt(["operational.ai.streaming_retry_base_delay_ms"], _options.Operational.Ai.StreamingRetryBaseDelayMs),
        min: 10,
        max: 60_000);

    // AI streaming temperature.
    public double OperationalAiStreamingTemperature
    {
        get
        {
            var configured = ReadDouble("operational.ai.streaming_temperature")
                             ?? _options.Operational.Ai.StreamingTemperature;
            return ClampDouble(configured, 0, 2);
        }
    }

    // Outbox batch size.
    public int OperationalOutboxBatchSize => ClampInt(
        ReadInt(["operational.outbox.batch_size"], _options.Operational.Outbox.BatchSize),
        min: 1,
        max: 5000);

    // Outbox max retry attempts.
    public int OperationalOutboxMaxRetryAttempts => ClampInt(
        ReadInt(["operational.outbox.max_retry_attempts"], _options.Operational.Outbox.MaxRetryAttempts),
        min: 1,
        max: 100);

    // Outbox lock timeout (giây).
    public int OperationalOutboxLockTimeoutSeconds => ClampInt(
        ReadInt(["operational.outbox.lock_timeout_seconds"], _options.Operational.Outbox.LockTimeoutSeconds),
        min: 30,
        max: 3600);

    // Outbox max backoff (giây).
    public int OperationalOutboxMaxBackoffSeconds => ClampInt(
        ReadInt(["operational.outbox.max_backoff_seconds"], _options.Operational.Outbox.MaxBackoffSeconds),
        min: 1,
        max: 3600);

    // Outbox poll interval (giây).
    public int OperationalOutboxPollIntervalSeconds => ClampInt(
        ReadInt(["operational.outbox.poll_interval_seconds"], _options.Operational.Outbox.PollIntervalSeconds),
        min: 1,
        max: 300);

    private static IReadOnlyList<int> NormalizeReconnectSchedule(IEnumerable<int> values)
    {
        return values
            .Where(v => v >= 0 && v <= 300_000)
            .Take(10)
            .ToArray();
    }

}
