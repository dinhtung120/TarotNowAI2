

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

// Unit test cho handler xác minh mã MFA để bật MFA chính thức.
public class MfaVerifyCommandHandlerTests
{
    // Mock user repo để điều khiển trạng thái MFA của user.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock MFA service để mô phỏng decrypt và verify TOTP.
    private readonly Mock<IMfaService> _mockMfaService;
    // Handler cần kiểm thử.
    private readonly MfaVerifyCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho MfaVerifyCommandHandler.
    /// Luồng dùng mock services để cô lập logic verify MFA khỏi crypto thật.
    /// </summary>
    public MfaVerifyCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _handler = new MfaVerifyCommandHandler(_mockUserRepo.Object, _mockMfaService.Object);
    }

    /// <summary>
    /// Tạo user test với trạng thái MFA và secret encrypted tùy biến.
    /// Luồng helper này giúp giảm lặp setup giữa các test case verify.
    /// </summary>
    private User CreateUser(bool mfaEnabled, string secretEncrypted)
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, mfaEnabled);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(user, secretEncrypted);
        return user;
    }

    /// <summary>
    /// Xác nhận user đã bật MFA không thể verify lại.
    /// Luồng này bảo vệ state machine bật MFA chỉ diễn ra một lần.
    /// </summary>
    [Fact]
    public async Task Handle_MfaAlreadyEnabled_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(true, "secret");
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA đã được bật rồi", ex.Message);
    }

    /// <summary>
    /// Xác nhận secret trống sẽ bị từ chối và yêu cầu setup trước.
    /// Luồng này chặn verify MFA khi chưa hoàn tất bước setup.
    /// </summary>
    [Fact]
    public async Task Handle_SecretEmpty_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(false, null!);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("vui lòng thực hiện bước setup", ex.Message.ToLower());
    }

    /// <summary>
    /// Xác nhận mã MFA không hợp lệ sẽ bị từ chối.
    /// Luồng này đảm bảo handler không bật MFA khi verify thất bại.
    /// </summary>
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

    /// <summary>
    /// Xác nhận mã MFA hợp lệ sẽ bật cờ MfaEnabled và lưu user.
    /// Luồng này kiểm tra side-effect chính của bước verify cuối.
    /// </summary>
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

    /// <summary>
    /// Xác nhận user không tồn tại trả NotFoundException.
    /// Luồng này ngăn bật MFA cho định danh user không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
