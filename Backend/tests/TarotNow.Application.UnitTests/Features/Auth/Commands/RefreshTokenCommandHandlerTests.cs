using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class RefreshTokenCommandExecutorTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IAuthSessionRepository> _authSessionRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IJwtTokenSettings> _jwtTokenSettingsMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    private readonly RefreshTokenCommandExecutor _handler;

    public RefreshTokenCommandExecutorTests()
    {
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _authSessionRepositoryMock = new Mock<IAuthSessionRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _jwtTokenSettingsMock = new Mock<IJwtTokenSettings>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();

        _jwtTokenSettingsMock.SetupGet(x => x.RefreshTokenExpiryDays).Returns(30);
        _jwtTokenSettingsMock.SetupGet(x => x.AccessTokenExpiryMinutes).Returns(10);
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh-token");
        _domainEventPublisherMock
            .Setup(x => x.PublishAsync(It.IsAny<Domain.Events.IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _authSessionRepositoryMock
            .Setup(x => x.TouchAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _handler = new RefreshTokenCommandExecutor(
            _refreshTokenRepositoryMock.Object,
            _authSessionRepositoryMock.Object,
            _tokenServiceMock.Object,
            _jwtTokenSettingsMock.Object,
            _domainEventPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRotateLockedAfterRetries_ShouldThrowRateLimited()
    {
        var command = BuildCommand();
        var tokenEntity = new RefreshToken(
            userId: Guid.NewGuid(),
            token: command.Token,
            expiresAt: DateTime.UtcNow.AddDays(1),
            createdByIp: command.ClientIpAddress,
            sessionId: Guid.NewGuid(),
            familyId: Guid.NewGuid(),
            parentTokenId: null,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokenEntity);
        _refreshTokenRepositoryMock
            .Setup(x => x.RotateAsync(It.IsAny<RefreshRotateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RefreshRotateResult.Locked());

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Equal(AuthErrorCodes.RateLimited, ex.ErrorCode);
        _refreshTokenRepositoryMock.Verify(
            x => x.RotateAsync(It.IsAny<RefreshRotateRequest>(), It.IsAny<CancellationToken>()),
            Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_WhenLegacyTokenHasEmptySession_ShouldBindSessionBeforeRotate()
    {
        var command = BuildCommand();
        var legacyToken = new RefreshToken(
            userId: Guid.NewGuid(),
            token: command.Token,
            expiresAt: DateTime.UtcNow.AddDays(1),
            createdByIp: command.ClientIpAddress,
            sessionId: Guid.Empty,
            familyId: Guid.NewGuid(),
            parentTokenId: null,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);
        var upgradedSession = new AuthSession(
            legacyToken.UserId,
            command.DeviceId,
            command.UserAgentHash,
            "ip-hash");

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(legacyToken);
        _authSessionRepositoryMock
            .Setup(x => x.CreateAsync(
                legacyToken.UserId,
                command.DeviceId,
                command.UserAgentHash,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(upgradedSession);
        _refreshTokenRepositoryMock
            .Setup(x => x.UpdateAsync(legacyToken, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _refreshTokenRepositoryMock
            .Setup(x => x.RotateAsync(It.IsAny<RefreshRotateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RefreshRotateResult.Locked());

        _ = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Equal(upgradedSession.Id, legacyToken.SessionId);
        _refreshTokenRepositoryMock.Verify(x => x.UpdateAsync(legacyToken, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRotationSucceeds_ShouldTouchSessionAndReturnNewTokens()
    {
        var command = BuildCommand();
        var user = new User("active@example.com", "active-user", "hash", "Active User", new DateTime(2000, 1, 1), true);
        user.Activate();

        var sessionId = Guid.NewGuid();
        var currentToken = new RefreshToken(
            userId: user.Id,
            token: command.Token,
            expiresAt: DateTime.UtcNow.AddDays(1),
            createdByIp: command.ClientIpAddress,
            sessionId: sessionId,
            familyId: Guid.NewGuid(),
            parentTokenId: null,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);
        SetTokenUser(currentToken, user);

        var nextToken = new RefreshToken(
            userId: user.Id,
            token: "next-refresh",
            expiresAt: DateTime.UtcNow.AddDays(7),
            createdByIp: command.ClientIpAddress,
            sessionId: sessionId,
            familyId: currentToken.FamilyId,
            parentTokenId: currentToken.Id,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);
        SetTokenUser(nextToken, user);

        var activeSession = new AuthSession(user.Id, command.DeviceId, command.UserAgentHash, "ip-hash");
        var accessExpiresAt = DateTime.UtcNow.AddMinutes(10);
        var accessJti = "jti-refresh-1";
        const string accessToken = "access-token-1";
        const string rotatedRawRefresh = "rotated-refresh-token";

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currentToken);
        _refreshTokenRepositoryMock
            .Setup(x => x.RotateAsync(It.IsAny<RefreshRotateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RefreshRotateResult.Success(currentToken, nextToken, rotatedRawRefresh));
        _authSessionRepositoryMock
            .Setup(x => x.GetActiveAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeSession);
        _tokenServiceMock
            .Setup(x => x.GenerateAccessToken(user, sessionId, out accessExpiresAt, out accessJti))
            .Returns(accessToken);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(accessToken, result.Response.AccessToken);
        Assert.Equal(rotatedRawRefresh, result.NewRefreshToken);
        _authSessionRepositoryMock.Verify(
            x => x.TouchAsync(
                sessionId,
                ComputeHash(command.ClientIpAddress),
                command.UserAgentHash,
                It.IsAny<CancellationToken>()),
            Times.Once);
        _domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<TokenRefreshedDomainEvent>(evt =>
                    evt.UserId == user.Id
                    && evt.SessionId == sessionId
                    && evt.DeviceId == command.DeviceId
                    && evt.AccessTokenJti == accessJti
                    && evt.IpHash == ComputeHash(command.ClientIpAddress)
                    && evt.UserAgentHash == command.UserAgentHash),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static RefreshTokenCommand BuildCommand()
    {
        return new RefreshTokenCommand
        {
            Token = "legacy-refresh-token",
            ClientIpAddress = "127.0.0.1",
            DeviceId = "device-1",
            UserAgentHash = "ua-hash-1",
            IdempotencyKey = "idem-key-1"
        };
    }

    private static void SetTokenUser(RefreshToken token, User user)
    {
        var backingField = typeof(RefreshToken).GetField(
            "<User>k__BackingField",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        backingField?.SetValue(token, user);
    }

    private static string ComputeHash(string value)
    {
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
