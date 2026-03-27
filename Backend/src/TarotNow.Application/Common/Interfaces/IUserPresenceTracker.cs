namespace TarotNow.Application.Common.Interfaces;

public interface IUserPresenceTracker
{
    void MarkConnected(string userId, string connectionId);

    void MarkDisconnected(string userId, string connectionId);

    bool IsOnline(string userId);
}
