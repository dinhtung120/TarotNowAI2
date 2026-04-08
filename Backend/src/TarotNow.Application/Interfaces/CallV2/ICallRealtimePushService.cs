using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICallRealtimePushService
{
    Task BroadcastIncomingAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default);

    Task BroadcastAcceptedAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default);

    Task BroadcastEndedAsync(CallSessionV2Dto session, string reason, CancellationToken ct = default);
}
