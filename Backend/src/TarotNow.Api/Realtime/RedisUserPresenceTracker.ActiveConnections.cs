using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisUserPresenceTracker : IUserPresenceTracker
{
    /// <summary>
    /// Kiểm tra user còn connection realtime active hay không (không xét fallback heartbeat).
    /// </summary>
    public bool HasActiveConnection(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            return db.SetLength(GetConnectionsKey(userId)) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to check active connections. UserId={UserId}", userId);
            return false;
        }
    }
}
