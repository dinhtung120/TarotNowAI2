using TarotNow.Infrastructure.Security;

namespace TarotNow.Infrastructure.IntegrationTests.Security;

/// <summary>
/// Đảm bảo thuật toán Argon2id mã hóa và xác thực mật khẩu chính xác.
/// Thuộc Phase 1.1 Auth Baseline -> Password Hashing.
/// </summary>
public class Argon2idPasswordHasherTests
{
    private readonly Argon2idPasswordHasher _hasher;

    public Argon2idPasswordHasherTests()
    {
        _hasher = new Argon2idPasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "SecurePassword123!";

        // Act
        var hash = _hasher.HashPassword(password);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        // Arrange
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(hash, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        // Arrange
        var password = "SecurePassword123!";
        var incorrectPassword = "WrongPassword456?";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(hash, incorrectPassword);

        // Assert
        Assert.False(result);
    }
}
