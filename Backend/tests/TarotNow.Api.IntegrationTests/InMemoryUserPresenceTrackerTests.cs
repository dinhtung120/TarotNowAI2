using Moq;
using TarotNow.Api.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.IntegrationTests;

public sealed class InMemoryUserPresenceTrackerTests
{
    [Fact]
    public void MarkDisconnected_WhenLastConnectionClosed_ShouldClearOnlineStateImmediately()
    {
        var settings = Mock.Of<ISystemConfigSettings>(x => x.PresenceTimeoutMinutes == 15);
        var tracker = new InMemoryUserPresenceTracker(settings);
        const string userId = "reader-1";
        const string connectionId = "conn-1";

        tracker.MarkConnected(userId, connectionId);
        Assert.True(tracker.IsOnline(userId));

        tracker.MarkDisconnected(userId, connectionId);

        Assert.False(tracker.HasActiveConnection(userId));
        Assert.False(tracker.IsOnline(userId));
        Assert.Null(tracker.GetLastActivity(userId));
    }

    [Fact]
    public void MarkDisconnected_WhenOtherConnectionsRemain_ShouldKeepUserOnline()
    {
        var settings = Mock.Of<ISystemConfigSettings>(x => x.PresenceTimeoutMinutes == 15);
        var tracker = new InMemoryUserPresenceTracker(settings);
        const string userId = "reader-2";

        tracker.MarkConnected(userId, "conn-a");
        tracker.MarkConnected(userId, "conn-b");

        tracker.MarkDisconnected(userId, "conn-a");

        Assert.True(tracker.HasActiveConnection(userId));
        Assert.True(tracker.IsOnline(userId));
    }
}
