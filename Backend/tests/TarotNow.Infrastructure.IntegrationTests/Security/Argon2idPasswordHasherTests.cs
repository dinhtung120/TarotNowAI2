

using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Security;
using Xunit; 

namespace TarotNow.Infrastructure.IntegrationTests.Security;

public class Argon2idPasswordHasherTests
{
    private readonly Argon2idPasswordHasher _hasher;

    public Argon2idPasswordHasherTests()
    {
        _hasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 19456, iterations: 2, parallelism: 1));
    }

        [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash); 
    }

        [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, password);

        Assert.True(result);
    }

        [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        var password = "SecurePassword123!";
        var incorrectPassword = "WrongPassword456?";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(hash, incorrectPassword);

        Assert.False(result); 
    }

    [Fact]
    public void HashPassword_ShouldUseConfiguredArgon2Policy()
    {
        var password = "SecurePassword123!";
        var hash = _hasher.HashPassword(password);

        Assert.Contains("m=19456", hash);
        Assert.Contains("t=2", hash);
        Assert.Contains("p=1", hash);
    }

    [Fact]
    public void NeedsRehash_ShouldReturnTrue_WhenPolicyChanges()
    {
        var legacyHasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 19456, iterations: 2, parallelism: 1));
        var strongerHasher = new Argon2idPasswordHasher(CreateOptions(memoryKb: 46080, iterations: 2, parallelism: 1));
        var hash = legacyHasher.HashPassword("SecurePassword123!");

        Assert.False(legacyHasher.NeedsRehash(hash));
        Assert.True(strongerHasher.NeedsRehash(hash));
    }

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
