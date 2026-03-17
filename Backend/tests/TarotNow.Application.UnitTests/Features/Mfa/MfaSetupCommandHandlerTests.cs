using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

public class MfaSetupCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMfaService> _mockMfaService;
    private readonly MfaSetupCommandHandler _handler;

    public MfaSetupCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _handler = new MfaSetupCommandHandler(_mockUserRepo.Object, _mockMfaService.Object);
    }

    private User CreateUser(bool mfaEnabled)
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, mfaEnabled);
        return user;
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFound()
    {
        var command = new MfaSetupCommand { UserId = Guid.NewGuid() };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_MfaAlreadyEnabled_ThrowsBadRequest()
    {
        var command = new MfaSetupCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(true);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA đã được bật", ex.Message);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSetupResult()
    {
        var command = new MfaSetupCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(false);
        user.GetType().GetProperty("Email")!.DeclaringType!.GetProperty("Email")!.SetValue(user, "test@test.com", null);

        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.GenerateSecretKey()).Returns("plain_secret");
        _mockMfaService.Setup(x => x.EncryptSecret("plain_secret")).Returns("encrypted_secret");
        _mockMfaService.Setup(x => x.GenerateQrCodeUri("plain_secret", "test@test.com")).Returns("qr_uri");
        _mockMfaService.Setup(x => x.GenerateBackupCodes(6)).Returns(new List<string> { "code1", "code2" });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal("qr_uri", result.QrCodeUri);
        Assert.Equal("plain_secret", result.SecretDisplay);
        Assert.Equal(2, result.BackupCodes.Count);

        Assert.Equal("encrypted_secret", user.MfaSecretEncrypted);
        Assert.False(user.MfaEnabled);

        _mockUserRepo.Verify(x => x.UpdateAsync(user, default), Times.Once);
    }
}
