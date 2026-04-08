

using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Security;
using Xunit; 

namespace TarotNow.Infrastructure.IntegrationTests.Security;

// Integration test cho Argon2idPasswordHasher.
public class Argon2idPasswordHasherTests
{
    // Hasher thật với policy cố định để kiểm tra hành vi băm/xác minh.
    private readonly Argon2idPasswordHasher _hasher;

    /// <summary>
    /// Khởi tạo fixture hasher với policy mặc định.
    /// Luồng này đảm bảo các test chạy trên cùng cấu hình Argon2id.
    /// </summary>
    public Argon2idPasswordHasherTests()
    {
        _hasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 19456, iterations: 2, parallelism: 1));
    }

    /// <summary>
    /// Xác nhận HashPassword trả về chuỗi hash hợp lệ khác plain password.
    /// Luồng này kiểm tra đầu ra băm không rỗng và không lộ mật khẩu gốc.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash);
    }

    /// <summary>
    /// Xác nhận VerifyPassword trả true cho mật khẩu đúng.
    /// Luồng này kiểm tra khả năng xác minh round-trip hash.
    /// </summary>
    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, password);

        Assert.True(result);
    }

    /// <summary>
    /// Xác nhận VerifyPassword trả false cho mật khẩu sai.
    /// Luồng này bảo vệ nhánh xác thực thất bại.
    /// </summary>
    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        var password = "SecurePassword123!";
        var incorrectPassword = "WrongPassword456?";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, incorrectPassword);

        Assert.False(result);
    }

    /// <summary>
    /// Xác nhận hash chứa policy Argon2 đã cấu hình.
    /// Luồng này đảm bảo tham số memory/time/parallelism được áp dụng đúng.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldUseConfiguredArgon2Policy()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        Assert.Contains("m=19456", hash);
        Assert.Contains("t=2", hash);
        Assert.Contains("p=1", hash);
    }

    /// <summary>
    /// Xác nhận NeedsRehash nhận diện đúng khi policy được nâng cấp.
    /// Luồng này kiểm tra backward compatibility giữa hash cũ và policy mới.
    /// </summary>
    [Fact]
    public void NeedsRehash_ShouldReturnTrue_WhenPolicyChanges()
    {
        var legacyHasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 19456, iterations: 2, parallelism: 1));
        var strongerHasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 46080, iterations: 2, parallelism: 1));
        var hash = legacyHasher.HashPassword("SecurePassword123!");

        Assert.False(legacyHasher.NeedsRehash(hash));
        Assert.True(strongerHasher.NeedsRehash(hash));
    }

    /// <summary>
    /// Tạo options Argon2 theo tham số đầu vào.
    /// Luồng helper này chuẩn hóa setup cấu hình cho các test policy khác nhau.
    /// </summary>
    private static IOptions<Argon2Options> CreateOptions(int memoryKb, int iterations, int parallelism)
    {
        var options = new Argon2Options
        {
            MemoryKB = memoryKb,
            Iterations = iterations,
            Parallelism = parallelism
        };

        return Microsoft.Extensions.Options.Options.Create(options);
    }
}
