namespace TarotNow.Application.Interfaces;

public sealed class MediaImageCompressionStep
{
    public double InitialQuality { get; set; }
    public double MaxSizeMb { get; set; }
    public int MaxWidthOrHeight { get; set; }
}

// Contract cấu hình hệ thống để tập trung các ngưỡng chi phí và hạn mức vận hành.
public interface ISystemConfigSettings
{
    // Chi phí Gold cho trải bài 3 lá.
    long Spread3GoldCost { get; }

    // Chi phí Diamond cho trải bài 3 lá.
    long Spread3DiamondCost { get; }

    // Chi phí Gold cho trải bài 5 lá.
    long Spread5GoldCost { get; }

    // Chi phí Diamond cho trải bài 5 lá.
    long Spread5DiamondCost { get; }

    // Chi phí Gold cho trải bài 10 lá.
    long Spread10GoldCost { get; }

    // Chi phí Diamond cho trải bài 10 lá.
    long Spread10DiamondCost { get; }

    // Hạn mức lượt AI mỗi ngày.
    int DailyAiQuota { get; }

    // Số request AI đồng thời tối đa.
    int InFlightAiCap { get; }

    // Khoảng thời gian chống spam request đọc bài (giây).
    int ReadingRateLimitSeconds { get; }

    // Thưởng Gold cho mỗi lần điểm danh ngày.
    long DailyCheckinGold { get; }

    // Cửa sổ giờ cho phép giữ streak trước khi reset.
    int StreakFreezeWindowHours { get; }

    // Chi phí Diamond cho một lần quay gacha.
    long GachaCostDiamond { get; }

    // Danh sách SLA giờ được phép cho conversation.
    IReadOnlyList<int> ChatAllowedSlaHours { get; }

    // SLA mặc định khi client không truyền giá trị.
    int ChatDefaultSlaHours { get; }

    // Số conversation active tối đa cho mỗi user.
    int ChatMaxActiveConversationsPerUser { get; }

    // Tỷ giá quy đổi VND theo 1 Diamond.
    long EconomyVndPerDiamond { get; }

    // Số năm kinh nghiệm tối thiểu khi đăng ký/cập nhật Reader.
    int ReaderMinYearsOfExperience { get; }

    // Mức Diamond/câu hỏi tối thiểu của Reader.
    long ReaderMinDiamondPerQuestion { get; }

    // Bậc giá follow-up trả phí theo thứ tự lượt hỏi.
    IReadOnlyList<int> FollowupPriceTiers { get; }

    // Số follow-up tối đa cho một phiên đọc bài.
    int FollowupMaxAllowed { get; }

    // Ngưỡng level để được 1 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdLow { get; }

    // Ngưỡng level để được 2 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdMid { get; }

    // Ngưỡng level để được 3 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdHigh { get; }

    // Ngưỡng tối thiểu Diamond cho một lệnh rút.
    long WithdrawalMinDiamond { get; }

    // Tỷ lệ phí rút (0..1).
    decimal WithdrawalFeeRate { get; }

    // Timeout online presence theo phút.
    int PresenceTimeoutMinutes { get; }

    // Chu kỳ quét timeout presence theo giây.
    int PresenceScanIntervalSeconds { get; }

    // Cửa sổ mở dispute theo giờ.
    int EscrowDisputeWindowHours { get; }

    // Độ dài tối thiểu của lý do mở dispute.
    int EscrowDisputeMinReasonLength { get; }

    // Deadline reader phản hồi theo giờ.
    int EscrowReaderResponseDueHours { get; }

    // Deadline tự động refund theo giờ.
    int EscrowAutoRefundHours { get; }

    // Split mặc định % dành cho Reader khi admin resolve dispute theo kiểu split.
    int AdminDisputeDefaultSplitPercentToReader { get; }

    // Lookback window (ngày) để xét policy freeze reader theo dispute.
    int AdminDisputeReaderFreezeLookbackDays { get; }

    // Ngưỡng số dispute gần đây để freeze reader.
    int AdminDisputeReaderFreezeThreshold { get; }

    // EXP cơ bản nhận trên mỗi lá bài khi reveal.
    decimal ProgressionReadingExpPerCard { get; }

    // Hệ số nhân EXP cho non-daily khi thanh toán bằng diamond.
    decimal ProgressionReadingDiamondMultiplierNonDaily { get; }

    // Gold thưởng khi dùng Lucky Star nhưng đã sở hữu title.
    long InventoryLuckyStarOwnedTitleGoldReward { get; }

    // Tuổi tối thiểu để đăng ký tài khoản.
    int LegalMinimumAge { get; }

    // Số Diamond mặc định đề xuất trong payment offer.
    long ChatPaymentOfferDefaultAmount { get; }

    // Độ dài tối đa ghi chú payment offer.
    int ChatPaymentOfferMaxNoteLength { get; }

    // Kích thước trang lịch sử chat.
    int ChatHistoryPageSize { get; }

    // Page size mặc định truy vấn conversation IDs theo participant.
    int ChatParticipantsDefaultPageSize { get; }

    // Page size tối đa truy vấn conversation IDs theo participant.
    int ChatParticipantsMaxPageSize { get; }

    // Lịch reconnect SignalR theo milliseconds.
    IReadOnlyList<int> RealtimeReconnectScheduleMs { get; }

    // Timeout negotiation realtime theo milliseconds.
    int RealtimeNegotiationTimeoutMs { get; }

    // Cooldown retry presence khi negotiation thất bại.
    int RealtimePresenceNegotiationCooldownMs { get; }

    // Cooldown retry chat realtime khi unauthorized.
    int RealtimeChatUnauthorizedCooldownMs { get; }

    // Server timeout cho SignalR connection.
    int RealtimeServerTimeoutMs { get; }

    // Thời gian clear typing indicator (ms).
    int RealtimeChatTypingClearMs { get; }

    // Debounce invalidate query realtime (ms).
    int RealtimeChatInvalidateDebounceMs { get; }

    // Guard khoảng thời gian sau lần load initial trước khi refetch conversation (ms).
    int RealtimeChatInitialLoadGuardMs { get; }

    // Guard bỏ qua invalidate trong vài giây đầu app start (ms).
    int RealtimeChatAppStartGuardMs { get; }

    // Timeout mặc định client HTTP fetch (ms).
    int OperationalHttpClientTimeoutMs { get; }

    // Timeout mặc định server-side HTTP proxy (ms).
    int OperationalHttpServerTimeoutMs { get; }

    // Mức timeout tối thiểu cho HTTP helper (ms).
    int OperationalHttpMinTimeoutMs { get; }

    // Timeout fetch runtime policies (ms).
    int OperationalRuntimePoliciesTimeoutMs { get; }

    // Stale time runtime policies (ms).
    int OperationalRuntimePoliciesStaleMs { get; }

    // Redis connect timeout (ms).
    int OperationalRedisConnectTimeoutMs { get; }

    // Redis sync timeout (ms).
    int OperationalRedisSyncTimeoutMs { get; }

    // Redis connect retry count.
    int OperationalRedisConnectRetry { get; }

    // AI timeout (giây).
    int OperationalAiTimeoutSeconds { get; }

    // AI max retries.
    int OperationalAiMaxRetries { get; }

    // AI retry base delay cho streaming (ms).
    int OperationalAiStreamingRetryBaseDelayMs { get; }

    // AI streaming temperature.
    double OperationalAiStreamingTemperature { get; }

    // Lease lock quota reservation cho luồng AI stream (giây).
    int OperationalAiQuotaReservationLeaseSeconds { get; }

    // Phiên bản prompt AI dùng thống nhất cho request + telemetry.
    string OperationalAiPromptVersion { get; }

    // Outbox batch size.
    int OperationalOutboxBatchSize { get; }

    // Outbox parallel worker count cho một batch.
    int OperationalOutboxParallelism { get; }

    // Outbox max retry attempts.
    int OperationalOutboxMaxRetryAttempts { get; }

    // Outbox lock timeout (giây).
    int OperationalOutboxLockTimeoutSeconds { get; }

    // Outbox max backoff (giây).
    int OperationalOutboxMaxBackoffSeconds { get; }

    // Outbox poll interval (giây).
    int OperationalOutboxPollIntervalSeconds { get; }

    // Escrow timer scan interval (giây).
    int OperationalEscrowTimerScanIntervalSeconds { get; }

    // Escrow completion-timeout batch size mỗi vòng quét.
    int OperationalEscrowCompletionTimeoutBatchSize { get; }

    // Page size mặc định danh sách readers ở FE.
    int UiReadersDirectoryPageSize { get; }

    // Giới hạn readers featured ở FE.
    int UiReadersFeaturedLimit { get; }

    // Debounce search input FE (ms).
    int UiSearchDebounceMs { get; }

    // Stale time readers directory FE (ms).
    int UiReadersDirectoryStaleMs { get; }

    // Stale time prefetch inbox FE (ms).
    int UiPrefetchChatInboxStaleMs { get; }

    // Kích thước tối đa file ảnh upload (bytes).
    long MediaUploadMaxImageBytes { get; }

    // Kích thước tối đa file voice upload (bytes).
    long MediaUploadMaxVoiceBytes { get; }

    // Thời lượng tối đa file voice upload (ms).
    int MediaUploadMaxVoiceDurationMs { get; }

    // Mục tiêu kích thước ảnh sau nén (bytes).
    long MediaUploadImageCompressionTargetBytes { get; }

    // Danh sách bước nén ảnh theo thứ tự fallback.
    IReadOnlyList<MediaImageCompressionStep> MediaUploadImageCompressionSteps { get; }

    // Số lần retry upload mặc định.
    int MediaUploadRetryAttempts { get; }

    // Delay retry upload mặc định (ms).
    int MediaUploadRetryDelayMs { get; }

    // Quest type mặc định của gamification.
    string GamificationDefaultQuestType { get; }

    // Leaderboard track mặc định của gamification.
    string GamificationDefaultLeaderboardTrack { get; }
}
