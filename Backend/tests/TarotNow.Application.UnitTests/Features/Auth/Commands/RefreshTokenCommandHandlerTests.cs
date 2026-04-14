using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IAuthSessionRepository> _authSessionRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IJwtTokenSettings> _jwtTokenSettingsMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
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

        _handler = new RefreshTokenCommandHandler(
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
}

