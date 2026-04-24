using System.Text.Json;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Bậc giá follow-up trả phí.
    public IReadOnlyList<int> FollowupPriceTiers
    {
        get
        {
            var fallback = _options.Followup.PriceTiers.Count == 0
                ? new[] { 1, 2, 4, 8, 16 }
                : _options.Followup.PriceTiers
                    .Where(x => x >= 0)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();
            var raw = ReadString("followup.price_tiers");
            if (string.IsNullOrWhiteSpace(raw))
            {
                return fallback;
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<int[]>(raw, JsonOptions) ?? [];
                var normalized = parsed
                    .Where(x => x >= 0)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();
                return normalized.Length > 0 ? normalized : fallback;
            }
            catch
            {
                return fallback;
            }
        }
    }

    // Số follow-up tối đa trong một phiên.
    public int FollowupMaxAllowed => ResolvePositiveInt(
        ReadInt(["followup.max_allowed"], _options.Followup.MaxAllowed),
        fallback: Math.Max(1, _options.Followup.MaxAllowed));

    // Ngưỡng level 1 lượt free.
    public int FollowupFreeSlotThresholdLow => ResolvePositiveInt(
        ReadInt(["followup.free_slots.threshold_low"], _options.Followup.FreeSlotThresholdLow),
        fallback: Math.Max(1, _options.Followup.FreeSlotThresholdLow));

    // Ngưỡng level 2 lượt free.
    public int FollowupFreeSlotThresholdMid => ResolvePositiveInt(
        ReadInt(["followup.free_slots.threshold_mid"], _options.Followup.FreeSlotThresholdMid),
        fallback: Math.Max(1, _options.Followup.FreeSlotThresholdMid));

    // Ngưỡng level 3 lượt free.
    public int FollowupFreeSlotThresholdHigh => ResolvePositiveInt(
        ReadInt(["followup.free_slots.threshold_high"], _options.Followup.FreeSlotThresholdHigh),
        fallback: Math.Max(1, _options.Followup.FreeSlotThresholdHigh));

    // Rút tối thiểu.
    public long WithdrawalMinDiamond => ResolveNonNegativeLong(
        ReadLong(["withdrawal.min_diamond"], _options.Withdrawal.MinDiamond),
        fallback: Math.Max(0, _options.Withdrawal.MinDiamond));

    // Tỷ lệ phí rút.
    public decimal WithdrawalFeeRate
    {
        get
        {
            var directRate = ReadDecimal("withdrawal.fee_rate");
            if (directRate.HasValue)
            {
                return ClampDecimal(directRate.Value, 0m, 1m);
            }

            return ClampDecimal(_options.Withdrawal.FeeRate, 0m, 1m);
        }
    }

    // Presence timeout online.
    public int PresenceTimeoutMinutes => ResolvePositiveInt(
        ReadInt(["presence.timeout_minutes"], _options.Presence.TimeoutMinutes),
        fallback: Math.Max(1, _options.Presence.TimeoutMinutes));

    // Presence scan interval.
    public int PresenceScanIntervalSeconds => ResolvePositiveInt(
        ReadInt(["presence.scan_interval_seconds"], _options.Presence.ScanIntervalSeconds),
        fallback: Math.Max(5, _options.Presence.ScanIntervalSeconds));

    // Escrow dispute window.
    public int EscrowDisputeWindowHours => ResolvePositiveInt(
        ReadInt(["escrow.dispute_window_hours"], _options.Escrow.DisputeWindowHours),
        fallback: Math.Max(1, _options.Escrow.DisputeWindowHours));

    // Escrow dispute reason min length.
    public int EscrowDisputeMinReasonLength => ResolvePositiveInt(
        ReadInt(["escrow.dispute.min_reason_length"], _options.Escrow.DisputeMinReasonLength),
        fallback: Math.Max(1, _options.Escrow.DisputeMinReasonLength));

    // Escrow response due.
    public int EscrowReaderResponseDueHours => ResolvePositiveInt(
        ReadInt(["escrow.reader_response_due_hours"], _options.Escrow.ReaderResponseDueHours),
        fallback: Math.Max(1, _options.Escrow.ReaderResponseDueHours));

    // Escrow auto refund.
    public int EscrowAutoRefundHours => ResolvePositiveInt(
        ReadInt(["escrow.auto_refund_hours"], _options.Escrow.AutoRefundHours),
        fallback: Math.Max(1, _options.Escrow.AutoRefundHours));

    // Split mặc định cho Reader khi admin resolve dispute.
    public int AdminDisputeDefaultSplitPercentToReader => ClampInt(
        ReadInt(["admin.dispute.default_split_percent_to_reader"], _options.AdminDispute.DefaultSplitPercentToReader),
        min: 1,
        max: 99);

    // Lookback window ngày cho policy freeze reader.
    public int AdminDisputeReaderFreezeLookbackDays => ClampInt(
        ReadInt(["admin.dispute.reader_freeze.lookback_days"], _options.AdminDispute.ReaderFreezeLookbackDays),
        min: 1,
        max: 365);

    // Ngưỡng dispute gần đây để freeze reader.
    public int AdminDisputeReaderFreezeThreshold => ClampInt(
        ReadInt(["admin.dispute.reader_freeze.threshold"], _options.AdminDispute.ReaderFreezeThreshold),
        min: 1,
        max: 100);
}
