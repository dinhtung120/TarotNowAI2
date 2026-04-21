using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class RevokeTokenCommandHandlerTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IAuthSessionRepository> _authSessionRepositoryMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    private readonly RevokeTokenCommandHandler _handler;

    public RevokeTokenCommandHandlerTests()
    {
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _authSessionRepositoryMock = new Mock<IAuthSessionRepository>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        _domainEventPublisherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _handler = new RevokeTokenCommandHandler(
            _refreshTokenRepositoryMock.Object,
            _authSessionRepositoryMock.Object,
            _domainEventPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRevokeAllWithoutUserIdButValidToken_ShouldResolveUserAndRevokeAll()
    {
        const string rawToken = "refresh-token-1";
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var tokenEntity = BuildToken(userId, sessionId, rawToken, Guid.NewGuid());

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(rawToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenEntity);
        _authSessionRepositoryMock
            .Setup(x => x.GetActiveSessionIdsByUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([sessionId]);

        var result = await _handler.Handle(
            new RevokeTokenCommand
            {
                RevokeAll = true,
                Token = rawToken
            },
            CancellationToken.None);

        Assert.True(result);
        _refreshTokenRepositoryMock.Verify(x => x.RevokeAllByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _authSessionRepositoryMock.Verify(x => x.RevokeAllByUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<UserLoggedOutDomainEvent>(evt =>
                    evt.UserId == userId
                    && evt.RevokeAll
                    && evt.SessionIds.Contains(sessionId)
                    && evt.Reason == RefreshRevocationReasons.ManualRevoke),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRevokeAllIdentityMismatch_ShouldThrowUnauthorized()
    {
        const string rawToken = "refresh-token-2";
        var userIdFromClaim = Guid.NewGuid();
        var userIdFromToken = Guid.NewGuid();
        var tokenEntity = BuildToken(userIdFromToken, Guid.NewGuid(), rawToken, Guid.NewGuid());

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(rawToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenEntity);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(
            new RevokeTokenCommand
            {
                RevokeAll = true,
                UserId = userIdFromClaim,
                Token = rawToken
            },
            CancellationToken.None));

        Assert.Equal(AuthErrorCodes.Unauthorized, ex.ErrorCode);
        _refreshTokenRepositoryMock.Verify(
            x => x.RevokeAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSingleLogoutForLegacySessionlessToken_ShouldRevokeFamily()
    {
        const string rawToken = "legacy-refresh-token";
        var userId = Guid.NewGuid();
        var familyId = Guid.NewGuid();
        var tokenEntity = BuildToken(userId, Guid.Empty, rawToken, familyId);

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(rawToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenEntity);
        _refreshTokenRepositoryMock
            .Setup(x => x.UpdateAsync(tokenEntity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(
            new RevokeTokenCommand
            {
                RevokeAll = false,
                Token = rawToken
            },
            CancellationToken.None);

        Assert.True(result);
        _refreshTokenRepositoryMock.Verify(
            x => x.RevokeFamilyAsync(familyId, RefreshRevocationReasons.ManualRevoke, It.IsAny<CancellationToken>()),
            Times.Once);
        _refreshTokenRepositoryMock.Verify(
            x => x.RevokeSessionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _authSessionRepositoryMock.Verify(
            x => x.RevokeAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<UserLoggedOutDomainEvent>(evt =>
                    evt.UserId == userId
                    && evt.RevokeAll == false
                    && evt.SessionId == null),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static RefreshToken BuildToken(Guid userId, Guid sessionId, string rawToken, Guid familyId)
    {
        return new RefreshToken(
            userId: userId,
            token: rawToken,
            expiresAt: DateTime.UtcNow.AddDays(1),
            createdByIp: "127.0.0.1",
            sessionId: sessionId,
            familyId: familyId,
            parentTokenId: null,
            createdDeviceId: "device",
            createdUserAgentHash: "ua-hash");
    }
}
