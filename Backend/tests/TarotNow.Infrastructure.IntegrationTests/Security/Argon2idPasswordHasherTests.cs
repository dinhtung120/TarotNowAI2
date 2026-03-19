/*
 * FILE: Argon2idPasswordHasherTests.cs
 * MỤC ĐÍCH: Integration test đánh giá thuật toán Argon2id (mã hóa và xác thực mật khẩu).
 *
 *   CÁC TEST CASE:
 *   1. HashPassword_ShouldReturnNonEmptyString:
 *      → Đảm bảo hàm hash hoạt động và không bao giờ rỗng.
 *   2. VerifyPassword_ShouldReturnTrue_ForCorrectPassword:
 *      → Hash xong xác thực lại bằng đúng mật khẩu trả về true.
 *   3. VerifyPassword_ShouldReturnFalse_ForIncorrectPassword:
 *      → Hash xong xác thực bằng mật khẩu sai trả về false.
 *
 *   BẢO MẬT: Argon2id là chuẩn mã hóa ưu việt (Phase 1.1 Auth Baseline), 
 *   chống brute-force và side-channel attacks so với bcrypt/PBKDF2.
 */

using TarotNow.Infrastructure.Security;
using Xunit; // Added Xunit explicitly based on [Fact] usage

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

    /// <summary>
    /// Đảm bảo hàm HashPassword tạo ra một chuỗi hash hợp lệ, 
    /// không rỗng và không bị lộ mk gốc.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash); // Không được lưu plain text
    }

    /// <summary>Hợp lệ hóa việc verify mật khẩu đúng.</summary>
    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, password);

        Assert.True(result);
    }

    /// <summary>Hợp lệ hóa việc từ chối khi mật khẩu sai.</summary>
    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        var password = "SecurePassword123!";
        var incorrectPassword = "WrongPassword456?";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, incorrectPassword);

        Assert.False(result); // Phải là false
    }
}
