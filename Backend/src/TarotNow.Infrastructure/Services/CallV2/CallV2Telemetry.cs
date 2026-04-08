using System.Diagnostics.Metrics;

namespace TarotNow.Infrastructure.Services.CallV2;

internal static class CallV2Telemetry
{
    private static readonly Meter Meter = new("TarotNow.CallV2", "1.0.0");

    private static readonly Counter<long> JoinSuccessCounter =
        Meter.CreateCounter<long>("call_v2_join_success_total");

    private static readonly Counter<long> ReconnectTokenCounter =
        Meter.CreateCounter<long>("call_v2_reconnect_token_total");

    private static readonly Counter<long> DropCounter =
        Meter.CreateCounter<long>("call_v2_drop_total");

    public static void RecordJoinSuccess(string callType)
    {
        JoinSuccessCounter.Add(1, new KeyValuePair<string, object?>("type", callType));
    }

    public static void RecordReconnectTokenIssued()
    {
        ReconnectTokenCounter.Add(1);
    }

    public static void RecordCallDrop(string status, string reason)
    {
        DropCounter.Add(
            1,
            new KeyValuePair<string, object?>("status", status),
            new KeyValuePair<string, object?>("reason", reason));
    }
}
