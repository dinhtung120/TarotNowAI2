

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Mfa.Commands.MfaChallenge;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Mfa;

// Unit test cho handler challenge MFA (TOTP + backup code).
public class MfaChallengeCommandHandlerTests
{
    // Mock user repo để điều khiển dữ liệu MFA user.
    private readonly Mock<IUserRepository> _mockUserRepo;
    // Mock MFA service để mô phỏng decrypt/verify code.
    private readonly Mock<IMfaService> _mockMfaService;
    // Handler cần kiểm thử.
    private readonly MfaChallengeCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho MfaChallengeCommandHandler.
    /// Luồng này cô lập kiểm thử challenge MFA khỏi triển khai crypto thật.
    /// </summary>
    public MfaChallengeCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMfaService = new Mock<IMfaService>();
        _handler = new MfaChallengeCommandHandler(_mockUserRepo.Object, _mockMfaService.Object);
    }

    /// <summary>
    /// Xác nhận TOTP hợp lệ trả true và không consume backup codes.
    /// Luồng này đảm bảo ưu tiên xác thực TOTP trước backup code.
    /// </summary>
    [Fact]
    public async Task Handle_TotpValid_ReturnsTrueWithoutUpdatingBackupCodes()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        var originalBackupCodes = JsonSerializer.Serialize(new List<string> { HashBackupCode("12345678") });
        user.MfaEnabled = true;
        user.MfaSecretEncrypted = "encrypted-secret";
        user.MfaBackupCodesHashJson = originalBackupCodes;

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret("encrypted-secret")).Returns("plain-secret");
        _mockMfaService.Setup(x => x.VerifyCode("plain-secret", "654321")).Returns(true);

        // TOTP pass thì không đụng vào danh sách backup code đã lưu.
        var result = await _handler.Handle(new MfaChallengeCommand { UserId = userId, Code = "654321" }, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(originalBackupCodes, user.MfaBackupCodesHashJson);
        _mockUserRepo.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Xác nhận backup code hợp lệ sẽ bị consume và cập nhật user.
    /// Luồng này kiểm tra behavior one-time-use của backup code.
    /// </summary>
    [Fact]
    public async Task Handle_BackupCodeValid_ConsumesBackupCodeAndUpdatesUser()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        user.MfaEnabled = true;
        user.MfaSecretEncrypted = "encrypted-secret";
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(new List<string> { HashBackupCode("12345678") });

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret("encrypted-secret")).Returns("plain-secret");
        _mockMfaService.Setup(x => x.VerifyCode("plain-secret", "12345678")).Returns(false);

        var result = await _handler.Handle(new MfaChallengeCommand { UserId = userId, Code = "12345678" }, CancellationToken.None);

        Assert.True(result);
        var remaining = JsonSerializer.Deserialize<List<string>>(user.MfaBackupCodesHashJson ?? "[]");
        Assert.NotNull(remaining);
        Assert.Empty(remaining);
        _mockUserRepo.Verify(x => x.UpdateAsync(user, default), Times.Once);
    }

    /// <summary>
    /// Xác nhận backup code không hợp lệ sẽ ném BadRequestException.
    /// Luồng này bảo vệ challenge MFA khỏi mã dự phòng giả mạo.
    /// </summary>
    [Fact]
    public async Task Handle_BackupCodeInvalid_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
        user.MfaEnabled = true;
        user.MfaSecretEncrypted = "encrypted-secret";
        user.MfaBackupCodesHashJson = JsonSerializer.Serialize(new List<string> { HashBackupCode("12345678") });

        _mockUserRepo.Setup(x => x.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _mockMfaService.Setup(x => x.DecryptSecret("encrypted-secret")).Returns("plain-secret");
        _mockMfaService.Setup(x => x.VerifyCode("plain-secret", "00000000")).Returns(false);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _handler.Handle(new MfaChallengeCommand { UserId = userId, Code = "00000000" }, CancellationToken.None));
    }

    /// <summary>
    /// Tạo user test đã active với định danh cố định.
    /// Luồng helper này giảm lặp setup cho các kịch bản MFA challenge.
    /// </summary>
    private static User CreateUser(Guid userId)
    {
        var user = new User(email: $"user-{userId:N}@mail.test", username: $"user_{userId:N}",
            passwordHash: "hash", displayName: "Test User",
            dateOfBirth: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        user.Activate();
        return user;
    }

    /// <summary>
    /// Băm backup code bằng SHA256 để mô phỏng dữ liệu lưu trữ thực tế.
    /// Luồng helper giúp assert consume backup code dựa trên hash thay vì plain text.
    /// </summary>
    private static string HashBackupCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
