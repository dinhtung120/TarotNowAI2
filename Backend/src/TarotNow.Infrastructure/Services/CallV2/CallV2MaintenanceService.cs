using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2MaintenanceService : ICallV2MaintenanceService
{
    private readonly ICallSessionV2Repository _sessions;
    private readonly ICallRealtimePushService _realtimePush;
    private readonly CallV2Options _options;
    private readonly ILogger<CallV2MaintenanceService> _logger;

    public CallV2MaintenanceService(
        ICallSessionV2Repository sessions,
        ICallRealtimePushService realtimePush,
        IOptions<CallV2Options> options,
        ILogger<CallV2MaintenanceService> logger)
    {
        _sessions = sessions;
        _realtimePush = realtimePush;
        _options = options.Value;
        _logger = logger;
    }

    public async Task ProcessTimeoutsAsync(CancellationToken cancellationToken = default)
    {
        await ExpireRequestedCallsAsync(cancellationToken);
        await FailJoiningCallsAsync(cancellationToken);
    }

    private async Task ExpireRequestedCallsAsync(CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddSeconds(-Math.Max(5, _options.RingTimeoutSeconds));
        var stale = await _sessions.ListStaleByStatusAsync([CallSessionV2Statuses.Requested], cutoff, 100, cancellationToken);

        foreach (var session in stale)
        {
            await FinalizeAsync(session, CallSessionV2Statuses.Ended, "timeout_server", cancellationToken);
        }
    }

    private async Task FailJoiningCallsAsync(CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddSeconds(-Math.Max(5, _options.JoinTimeoutSeconds));
        var stale = await _sessions.ListStaleByStatusAsync(
            [CallSessionV2Statuses.Accepted, CallSessionV2Statuses.Joining],
            cutoff,
            100,
            cancellationToken);

        foreach (var session in stale)
        {
            await FinalizeAsync(session, CallSessionV2Statuses.Failed, "join_timeout", cancellationToken);
        }
    }
}
