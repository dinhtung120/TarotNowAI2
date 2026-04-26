using System.Security.Claims;
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
