using System.Security.Claims;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Features.Presence.Commands.PublishUserStatusChanged;

namespace TarotNow.Api.IntegrationTests;

public class PresenceHubTests
{
    [Fact]
    public async Task OnDisconnectedAsync_WhenUserStillHasAnotherConnection_ShouldNotPublishOffline()
    {
        var userId = Guid.NewGuid().ToString();
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Send(It.IsAny<PublishUserStatusChangedCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var tracker = new Mock<IUserPresenceTracker>();
        tracker.Setup(x => x.HasActiveConnection(userId)).Returns(true);
        var groups = new Mock<IGroupManager>();

        var hub = new PresenceHub(mediator.Object, tracker.Object, NullLogger<PresenceHub>.Instance)
        {
            Context = CreateContext("conn-a", userId),
            Groups = groups.Object
        };

        await hub.OnDisconnectedAsync(null);

        tracker.Verify(x => x.MarkDisconnected(userId, "conn-a"), Times.Once);
        groups.Verify(x => x.RemoveFromGroupAsync("conn-a", $"user:{userId}", It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(
            It.Is<PublishUserStatusChangedCommand>(request =>
                request.UserId == userId && request.Status == "offline"),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OnDisconnectedAsync_WhenLastConnectionClosed_ShouldPublishOffline()
    {
        var userId = Guid.NewGuid().ToString();
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Send(It.IsAny<PublishUserStatusChangedCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var tracker = new Mock<IUserPresenceTracker>();
        tracker.Setup(x => x.HasActiveConnection(userId)).Returns(false);
        var groups = new Mock<IGroupManager>();

        var hub = new PresenceHub(mediator.Object, tracker.Object, NullLogger<PresenceHub>.Instance)
        {
            Context = CreateContext("conn-b", userId),
            Groups = groups.Object
        };

        await hub.OnDisconnectedAsync(null);

        mediator.Verify(x => x.Send(
            It.Is<PublishUserStatusChangedCommand>(request =>
                request.UserId == userId && request.Status == "offline"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SubscribeUserStatusObservers_ShouldJoinNormalizedObserverGroups()
    {
        var userId = Guid.NewGuid().ToString();
        var mediator = new Mock<IMediator>();
        var tracker = new Mock<IUserPresenceTracker>();
        var groups = new Mock<IGroupManager>();
        var hub = new PresenceHub(mediator.Object, tracker.Object, NullLogger<PresenceHub>.Instance)
        {
            Context = CreateContext("conn-c", userId),
            Groups = groups.Object
        };

        await hub.SubscribeUserStatusObservers(new[] { " reader-1 ", "reader-1", "reader-2", "" });

        groups.Verify(
            x => x.AddToGroupAsync("conn-c", "presence:watch:user:reader-1", It.IsAny<CancellationToken>()),
            Times.Once);
        groups.Verify(
            x => x.AddToGroupAsync("conn-c", "presence:watch:user:reader-2", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UnsubscribeUserStatusObservers_ShouldLeaveNormalizedObserverGroups()
    {
        var userId = Guid.NewGuid().ToString();
        var mediator = new Mock<IMediator>();
        var tracker = new Mock<IUserPresenceTracker>();
        var groups = new Mock<IGroupManager>();
        var hub = new PresenceHub(mediator.Object, tracker.Object, NullLogger<PresenceHub>.Instance)
        {
            Context = CreateContext("conn-d", userId),
            Groups = groups.Object
        };

        await hub.UnsubscribeUserStatusObservers(new[] { " reader-1 ", "reader-1", "reader-2", "" });

        groups.Verify(
            x => x.RemoveFromGroupAsync("conn-d", "presence:watch:user:reader-1", It.IsAny<CancellationToken>()),
            Times.Once);
        groups.Verify(
            x => x.RemoveFromGroupAsync("conn-d", "presence:watch:user:reader-2", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SubscribeUserStatusObservers_ShouldCapBatchAt200Users()
    {
        var userId = Guid.NewGuid().ToString();
        var mediator = new Mock<IMediator>();
        var tracker = new Mock<IUserPresenceTracker>();
        var groups = new Mock<IGroupManager>();
        var hub = new PresenceHub(mediator.Object, tracker.Object, NullLogger<PresenceHub>.Instance)
        {
            Context = CreateContext("conn-e", userId),
            Groups = groups.Object
        };
        var inputUserIds = Enumerable.Range(1, 250).Select(index => $"reader-{index}").ToArray();

        await hub.SubscribeUserStatusObservers(inputUserIds);

        groups.Verify(
            x => x.AddToGroupAsync(
                "conn-e",
                It.Is<string>(groupName => groupName.StartsWith("presence:watch:user:", StringComparison.Ordinal)),
                It.IsAny<CancellationToken>()),
            Times.Exactly(200));
    }

    private static HubCallerContext CreateContext(string connectionId, string userId)
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, userId)], "test-auth"));
        var context = new Mock<HubCallerContext>();
        context.SetupGet(x => x.ConnectionId).Returns(connectionId);
        context.SetupGet(x => x.User).Returns(claimsPrincipal);
        return context.Object;
    }
}
