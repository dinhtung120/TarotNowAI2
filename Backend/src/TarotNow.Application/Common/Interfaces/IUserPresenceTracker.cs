using System;
using System.Collections.Generic;

namespace TarotNow.Application.Common.Interfaces;

public interface IUserPresenceTracker
{
    void MarkConnected(string userId, string connectionId);

    void MarkDisconnected(string userId, string connectionId);

    bool IsOnline(string userId);
    
    
    void RecordHeartbeat(string userId);
    
    DateTime? GetLastActivity(string userId);
    
    IReadOnlyList<string> GetTimedOutUsers(TimeSpan timeout);
    
    void RemoveUser(string userId);
}
