

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

// Unit test cho handler thiết lập MFA ban đầu.
public class MfaSetupCommandHandlerTests
{
    // Mock user repo để điều khiển dữ liệu user MFA.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock MFA service để mô phỏng generate/encrypt/qr/backup codes.
    private readonly Mock<IMfaService> _mockMfaService;
    // Handler cần kiểm thử.
    private readonly MfaSetupCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho MfaSetupCommandHandler.
    /// Luồng dùng mock services để test logic setup MFA không phụ thuộc crypto thật.
    /// </summary>
    public MfaSetupCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _handler = new MfaSetupCommandHandler(_mockUserRepo.Object, _mockMfaService.Object);
    }

    /// <summary>
    /// Tạo user test với trạng thái MFA bật/tắt theo đầu vào.
    /// Luồng helper dùng reflection để khởi tạo nhanh entity trong test.
    /// </summary>
    private User CreateUser(bool mfaEnabled)
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, mfaEnabled);
        return user;
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả NotFoundException.
    /// Luồng này chặn thiết lập MFA trên tài khoản không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFound()
    {
        var command = new MfaSetupCommand { UserId = Guid.NewGuid() };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận user đã bật MFA không thể setup lại.
    /// Luồng này bảo vệ state machine thiết lập MFA một chiều.
    /// </summary>
    [Fact]
    public async Task Handle_MfaAlreadyEnabled_ThrowsBadRequest()
    {
        var command = new MfaSetupCommand { UserId = Guid.NewGuid() };
        var user = CreateUser(true);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA đã được bật", ex.Message);
    }

    /// <summary>
    /// Xác nhận request hợp lệ trả đầy đủ dữ liệu setup MFA.
    /// Luồng này kiểm tra QR URI, secret display, backup codes và dữ liệu encrypted lưu vào user.
    /// </summary>
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

        // Thực thi setup MFA và kiểm tra dữ liệu trả về.
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal("qr_uri", result.QrCodeUri);
        Assert.Equal("plain_secret", result.SecretDisplay);
        Assert.Equal(2, result.BackupCodes.Count);
        Assert.Equal("encrypted_secret", user.MfaSecretEncrypted);
        Assert.False(user.MfaEnabled);
        _mockUserRepo.Verify(x => x.UpdateAsync(user, default), Times.Once);
    }
}
