

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

public class MfaVerifyCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMfaService> _mockMfaService;
    private readonly MfaVerifyCommandHandler _handler;

    public MfaVerifyCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _handler = new MfaVerifyCommandHandler(_mockUserRepo.Object, _mockMfaService.Object);
    }

    
    private User CreateUser(bool mfaEnabled, string secretEncrypted)
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, mfaEnabled);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(user, secretEncrypted);
        return user;
    }

        [Fact]
    public async Task Handle_MfaAlreadyEnabled_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(true, "secret");
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA đã được bật rồi", ex.Message);
    }

        [Fact]
    public async Task Handle_SecretEmpty_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(false, null!);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("vui lòng thực hiện bước setup", ex.Message.ToLower());
    }

        [Fact]
    public async Task Handle_InvalidCode_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(false, "encrypted");
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret("encrypted")).Returns("plain");
        _mockMfaService.Setup(x => x.VerifyCode("plain", "123")).Returns(false);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không hợp lệ", ex.Message);
    }

        [Fact]
    public async Task Handle_ValidCode_EnablesMfa()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(false, "encrypted");
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret("encrypted")).Returns("plain");
        _mockMfaService.Setup(x => x.VerifyCode("plain", "123")).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.True(user.MfaEnabled); 
        _mockUserRepo.Verify(x => x.UpdateAsync(user, default), Times.Once);
    }

        [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
