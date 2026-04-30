namespace TarotNow.Infrastructure.Options;

public sealed partial class SystemConfigOptions
{
    public sealed class RealtimeOptions
    {
        public List<int> ReconnectScheduleMs { get; set; } = [0, 2_000, 5_000, 10_000, 30_000];
        public int NegotiationTimeoutMs { get; set; } = 8_000;
        public int PresenceNegotiationCooldownMs { get; set; } = 45_000;
        public int ChatUnauthorizedCooldownMs { get; set; } = 60_000;
        public int ServerTimeoutMs { get; set; } = 120_000;
        public int ChatTypingClearMs { get; set; } = 2_500;
        public int ChatInvalidateDebounceMs { get; set; } = 1_000;
        public int ChatInitialLoadGuardMs { get; set; } = 2_000;
        public int ChatAppStartGuardMs { get; set; } = 3_000;
    }

    public sealed class OperationalOptions
    {
        public HttpOptions Http { get; set; } = new();
        public RuntimePoliciesOptions RuntimePolicies { get; set; } = new();
        public RedisOptions Redis { get; set; } = new();
        public AiOptions Ai { get; set; } = new();
        public OutboxOptions Outbox { get; set; } = new();
        public EscrowTimerOptions EscrowTimer { get; set; } = new();

        public sealed class HttpOptions
        {
            public int ClientTimeoutMs { get; set; } = 8_000;
            public int ServerTimeoutMs { get; set; } = 8_000;
            public int MinTimeoutMs { get; set; } = 1_000;
        }

        public sealed class RuntimePoliciesOptions
        {
            public int TimeoutMs { get; set; } = 8_000;
            public int StaleMs { get; set; } = 15_000;
        }

        public sealed class RedisOptions
        {
            public int ConnectTimeoutMs { get; set; } = 2_000;
            public int SyncTimeoutMs { get; set; } = 2_000;
            public int ConnectRetry { get; set; } = 1;
        }

        public sealed class AiOptions
        {
            public int TimeoutSeconds { get; set; } = 30;
            public int MaxRetries { get; set; } = 2;
            public int StreamingRetryBaseDelayMs { get; set; } = 200;
            public double StreamingTemperature { get; set; } = 0.7;
            public int QuotaReservationLeaseSeconds { get; set; } = 30;
            public string PromptVersion { get; set; } = "v1.5";
        }

        public sealed class OutboxOptions
        {
            public int BatchSize { get; set; } = 50;
            public int Parallelism { get; set; } = 4;
            public int MaxRetryAttempts { get; set; } = 12;
            public int LockTimeoutSeconds { get; set; } = 120;
            public int MaxBackoffSeconds { get; set; } = 300;
            public int PollIntervalSeconds { get; set; } = 1;
        }

        public sealed class EscrowTimerOptions
        {
            public int ScanIntervalSeconds { get; set; } = 300;
            public int CompletionTimeoutBatchSize { get; set; } = 200;
        }
    }

    public sealed class UiOptions
    {
        public ReadersOptions Readers { get; set; } = new();
        public SearchOptions Search { get; set; } = new();
        public PrefetchOptions Prefetch { get; set; } = new();

        public sealed class ReadersOptions
        {
            public int DirectoryPageSize { get; set; } = 12;
            public int FeaturedLimit { get; set; } = 4;
            public int DirectoryStaleMs { get; set; } = 30_000;
        }

        public sealed class SearchOptions
        {
            public int DebounceMs { get; set; } = 300;
        }

        public sealed class PrefetchOptions
        {
            public int ChatInboxStaleMs { get; set; } = 30_000;
        }
    }

    public sealed class MediaUploadOptions
    {
        public long MaxImageBytes { get; set; } = 10L * 1024 * 1024;
        public long MaxVoiceBytes { get; set; } = 5L * 1024 * 1024;
        public int MaxVoiceDurationMs { get; set; } = 600_000;
        public long ImageCompressionTargetBytes { get; set; } = 80L * 1024;
        public List<ImageCompressionStepOptions> ImageCompressionSteps { get; set; } =
        [
            new() { InitialQuality = 0.68, MaxSizeMb = 0.15, MaxWidthOrHeight = 1440 },
            new() { InitialQuality = 0.58, MaxSizeMb = 0.10, MaxWidthOrHeight = 1200 },
            new() { InitialQuality = 0.48, MaxSizeMb = 0.06, MaxWidthOrHeight = 960 },
            new() { InitialQuality = 0.38, MaxSizeMb = 0.03, MaxWidthOrHeight = 640 }
        ];
        public int RetryAttempts { get; set; } = 3;
        public int RetryDelayMs { get; set; } = 350;

        public sealed class ImageCompressionStepOptions
        {
            public double InitialQuality { get; set; }
            public double MaxSizeMb { get; set; }
            public int MaxWidthOrHeight { get; set; }
        }
    }

    public sealed class EconomyOptions
    {
        public long VndPerDiamond { get; set; } = 100;
    }

    public sealed class ReaderOptions
    {
        public int MinYearsOfExperience { get; set; } = 1;
        public long MinDiamondPerQuestion { get; set; } = 50;
    }

    public sealed class GamificationOptions
    {
        public string DefaultQuestType { get; set; } = "daily";
        public string DefaultLeaderboardTrack { get; set; } = "spent_gold_daily";
    }
}
