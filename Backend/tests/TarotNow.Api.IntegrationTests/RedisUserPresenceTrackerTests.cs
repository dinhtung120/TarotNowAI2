using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StackExchange.Redis;
using TarotNow.Api.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.IntegrationTests;

public sealed class RedisUserPresenceTrackerTests
{
    [Fact]
    public void HasActiveConnection_WhenLastActivityIsStale_ShouldPruneConnectionSetAndReturnFalse()
    {
        var staleUnix = DateTimeOffset.UtcNow.AddMinutes(-20).ToUnixTimeSeconds();
        var database = new Mock<IDatabase>(MockBehavior.Strict);
        database
            .Setup(x => x.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(true);
        database
            .Setup(x => x.KeyTimeToLive(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(TimeSpan.FromMinutes(10));
        database
            .Setup(x => x.SetLength(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(2);
        database
            .Setup(x => x.SortedSetScore(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(staleUnix);
        database
            .Setup(x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(true);

        var tracker = CreateTracker(database.Object);
        var hasActiveConnection = tracker.HasActiveConnection("reader-3");

        Assert.False(hasActiveConnection);
        database.Verify(
            x => x.KeyDelete(
                It.Is<RedisKey>(key => key.ToString() == "presence:user:reader-3:connections"),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public void HasActiveConnection_WhenLastActivityIsFresh_ShouldKeepConnectionSet()
    {
        var freshUnix = DateTimeOffset.UtcNow.AddMinutes(-2).ToUnixTimeSeconds();
        var database = new Mock<IDatabase>(MockBehavior.Strict);
        database
            .Setup(x => x.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(true);
        database
            .Setup(x => x.KeyTimeToLive(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(TimeSpan.FromMinutes(10));
        database
            .Setup(x => x.SetLength(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(1);
        database
            .Setup(x => x.SortedSetScore(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(freshUnix);

        var tracker = CreateTracker(database.Object);
        var hasActiveConnection = tracker.HasActiveConnection("reader-4");

        Assert.True(hasActiveConnection);
        database.Verify(
            x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()),
            Times.Never);
    }

    [Fact]
    public void GetTimedOutUsers_WhenCandidateHasStaleConnectionSet_ShouldPruneAndReturnCandidate()
    {
        var staleUnix = DateTimeOffset.UtcNow.AddMinutes(-25).ToUnixTimeSeconds();
        var database = new Mock<IDatabase>(MockBehavior.Strict);
        database
            .Setup(x => x.SortedSetRangeByScore(
                It.IsAny<RedisKey>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<Exclude>(),
                It.IsAny<Order>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CommandFlags>()))
            .Returns([new RedisValue("reader-5")]);
        database
            .Setup(x => x.SetLength(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(1);
        database
            .Setup(x => x.SortedSetScore(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(staleUnix);
        database
            .Setup(x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(true);

        var tracker = CreateTracker(database.Object);
        var timedOutUsers = tracker.GetTimedOutUsers(TimeSpan.FromMinutes(15));

        Assert.Single(timedOutUsers);
        Assert.Equal("reader-5", timedOutUsers[0]);
        database.Verify(
            x => x.KeyDelete(
                It.Is<RedisKey>(key => key.ToString() == "presence:user:reader-5:connections"),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public void MarkDisconnected_WhenLastConnectionClosed_ShouldClearConnectionKeyAndLastActivity()
    {
        var database = new Mock<IDatabase>(MockBehavior.Strict);
        database
            .Setup(x => x.SetRemove(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(true);
        database
            .Setup(x => x.SetLength(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(0);
        database
            .Setup(x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(true);
        database
            .Setup(x => x.SortedSetRemove(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(true);

        var tracker = CreateTracker(database.Object);
        tracker.MarkDisconnected("reader-1", "conn-1");

        database.Verify(
            x => x.KeyDelete(
                It.Is<RedisKey>(key => key.ToString() == "presence:user:reader-1:connections"),
                It.IsAny<CommandFlags>()),
            Times.Once);
        database.Verify(
            x => x.SortedSetRemove(
                It.Is<RedisKey>(key => key.ToString() == "presence:last-activity"),
                It.Is<RedisValue>(value => value.ToString() == "reader-1"),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public void MarkDisconnected_WhenOtherConnectionsRemain_ShouldNotClearLastActivity()
    {
        var database = new Mock<IDatabase>(MockBehavior.Strict);
        database
            .Setup(x => x.SetRemove(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns(true);
        database
            .Setup(x => x.SetLength(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Returns(2);

        var tracker = CreateTracker(database.Object);
        tracker.MarkDisconnected("reader-2", "conn-2");

        database.Verify(
            x => x.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()),
            Times.Never);
        database.Verify(
            x => x.SortedSetRemove(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()),
            Times.Never);
    }

    private static RedisUserPresenceTracker CreateTracker(IDatabase database)
    {
        var settings = Mock.Of<ISystemConfigSettings>(x =>
            x.PresenceTimeoutMinutes == 15 &&
            x.PresenceScanIntervalSeconds == 60);
        var multiplexer = new Mock<IConnectionMultiplexer>(MockBehavior.Strict);
        multiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database);

        return new RedisUserPresenceTracker(
            multiplexer.Object,
            settings,
            NullLogger<RedisUserPresenceTracker>.Instance);
    }
}
