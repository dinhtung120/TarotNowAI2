/*
 * FILE: MfaVerifyCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler xác thực MFA (bước 2: verify TOTP code → bật MFA).
 *
 *   CÁC TEST CASE (5 scenarios):
 *   1. Handle_MfaAlreadyEnabled_ThrowsBadRequest: MFA đã bật → 400
 *   2. Handle_SecretEmpty_ThrowsBadRequest: chưa setup → 400 (cần setup trước)
 *   3. Handle_InvalidCode_ThrowsBadRequest: TOTP code sai → 400
 *   4. Handle_ValidCode_EnablesMfa: code đúng → MfaEnabled=true
 *   5. Handle_UserNotFound_ThrowsNotFoundException: userId sai → 404
 *
 *   FLOW: Setup (bước 1) → Verify (bước 2) → MfaEnabled=true
 *   → Bước 2 là bước cuối: user scan QR + nhập TOTP code → hệ thống confirm
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

/// <summary>
/// Test MFA verify: bước 2 kích hoạt, validate code, enable flag.
/// </summary>
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

    /* Helper: tạo User giả bằng reflection */
    private User CreateUser(bool mfaEnabled, string secretEncrypted)
    {
        var userType = typeof(User);
        var user = (User)Activator.CreateInstance(userType, nonPublic: true)!;
        userType.GetProperty("MfaEnabled")!.SetValue(user, mfaEnabled);
        userType.GetProperty("MfaSecretEncrypted")!.SetValue(user, secretEncrypted);
        return user;
    }

    /// <summary>MFA đã bật → BadRequest.</summary>
    [Fact]
    public async Task Handle_MfaAlreadyEnabled_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(true, "secret");
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("MFA đã được bật rồi", ex.Message);
    }

    /// <summary>Chưa setup (secret trống) → BadRequest.</summary>
    [Fact]
    public async Task Handle_SecretEmpty_ThrowsBadRequest()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        var user = CreateUser(false, null!);
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("vui lòng thực hiện bước setup", ex.Message.ToLower());
    }

    /// <summary>TOTP code sai → BadRequest.</summary>
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

    /// <summary>Code đúng → MfaEnabled=true.</summary>
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
        Assert.True(user.MfaEnabled); // MFA bật thành công!
        _mockUserRepo.Verify(x => x.UpdateAsync(user, default), Times.Once);
    }

    /// <summary>UserId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var command = new MfaVerifyCommand { UserId = Guid.NewGuid(), Code = "123" };
        _mockUserRepo.Setup(x => x.GetByIdAsync(command.UserId, default)).ReturnsAsync((User)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
