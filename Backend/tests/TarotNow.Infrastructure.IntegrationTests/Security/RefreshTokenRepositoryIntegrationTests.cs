using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.IntegrationTests.Outbox;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Repositories;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.IntegrationTests.Security;

[Collection("InfrastructurePostgres")]
public sealed class RefreshTokenRepositoryIntegrationTests
{
    private readonly InfrastructurePostgresFixture _fixture;

    public RefreshTokenRepositoryIntegrationTests(InfrastructurePostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByTokenAsync_ShouldIgnoreLegacyPlainTokenRows()
    {
        await ResetAuthTablesAsync();

        const string rawToken = "legacy-plain-refresh-token";
        var user = CreateUser("legacy");
        var sessionId = Guid.NewGuid();
        var seededTokenId = Guid.Empty;

        await using (var seedContext = _fixture.CreateDbContext())
        {
            seedContext.Users.Add(user);
            var refreshToken = new RefreshToken(
                user.Id,
                rawToken,
                DateTime.UtcNow.AddHours(1),
                "127.0.0.1",
                sessionId);

            seedContext.RefreshTokens.Add(refreshToken);
            await seedContext.SaveChangesAsync();
            seededTokenId = refreshToken.Id;

            // Giả lập dữ liệu legacy lưu plain token để xác nhận repo không còn query plain path.
            await seedContext.Database.ExecuteSqlInterpolatedAsync(
                $"""UPDATE refresh_tokens SET token = {rawToken} WHERE id = {seededTokenId};""");
        }

        await using var assertContext = _fixture.CreateDbContext();
        var repository = CreateRepository(assertContext, new InMemoryRefreshCacheService());

        var resolved = await repository.GetByTokenAsync(rawToken);

        Assert.Null(resolved);
    }

    [Fact]
    public async Task RotateAsync_ShouldReturnIdempotent_WhenSameRequestIsRetried()
    {
        await ResetAuthTablesAsync();

        var cacheService = new InMemoryRefreshCacheService();
        var user = CreateUser("idem");
        const string oldRawToken = "refresh-old-idem";
        const string newRawToken = "refresh-new-idem";
        var idempotencyKey = $"idem-{Guid.NewGuid():N}";

        await using var context = _fixture.CreateDbContext();
        var repository = CreateRepository(context, cacheService);
        await SeedUserAndTokenAsync(context, user, oldRawToken, DateTime.UtcNow.AddDays(7));

        var request = BuildRotateRequest(oldRawToken, newRawToken, idempotencyKey);
        var first = await repository.RotateAsync(request);
        var second = await repository.RotateAsync(request);

        Assert.Equal(RefreshRotateStatus.Success, first.Status);
        Assert.Equal(RefreshRotateStatus.Idempotent, second.Status);
        Assert.Equal(newRawToken, second.NewRawToken);

        var tokenCount = await context.RefreshTokens
            .AsNoTracking()
            .CountAsync(x => x.UserId == user.Id);
        Assert.Equal(2, tokenCount);
    }

    [Fact]
    public async Task RotateAsync_ShouldReturnIdempotentFromDatabase_WhenLockContentionOccurs()
    {
        await ResetAuthTablesAsync();

        var cacheService = new InMemoryRefreshCacheService();
        var user = CreateUser("lock");
        const string oldRawToken = "refresh-old-lock";
        const string newRawToken = "refresh-new-lock";
        var idempotencyKey = $"lock-{Guid.NewGuid():N}";

        await using var context = _fixture.CreateDbContext();
        var repository = CreateRepository(context, cacheService);
        await SeedUserAndTokenAsync(context, user, oldRawToken, DateTime.UtcNow.AddDays(7));

        var request = BuildRotateRequest(oldRawToken, newRawToken, idempotencyKey);
        var first = await repository.RotateAsync(request);
        Assert.Equal(RefreshRotateStatus.Success, first.Status);

        // Mô phỏng request concurrent không acquire được lock; nhánh xử lý phải trả idempotent từ DB state.
        cacheService.ForceAcquireFailure = true;
        var second = await repository.RotateAsync(request);

        Assert.Equal(RefreshRotateStatus.Idempotent, second.Status);
        Assert.Equal(newRawToken, second.NewRawToken);
    }

    [Fact]
    public async Task RotateAsync_ShouldRemainIdempotent_WhenCacheGetSetThrows()
    {
        await ResetAuthTablesAsync();

        var cacheService = new ThrowingReadWriteRefreshCacheService();
        var user = CreateUser("db-first");
        const string oldRawToken = "refresh-old-db-first";
        const string newRawToken = "refresh-new-db-first";
        var idempotencyKey = $"db-first-{Guid.NewGuid():N}";

        await using var context = _fixture.CreateDbContext();
        var repository = CreateRepository(context, cacheService);
        await SeedUserAndTokenAsync(context, user, oldRawToken, DateTime.UtcNow.AddDays(7));

        var request = BuildRotateRequest(oldRawToken, newRawToken, idempotencyKey);
        var first = await repository.RotateAsync(request);
        var second = await repository.RotateAsync(request);

        Assert.Equal(RefreshRotateStatus.Success, first.Status);
        Assert.Equal(RefreshRotateStatus.Idempotent, second.Status);
        Assert.Equal(newRawToken, second.NewRawToken);
    }

    [Fact]
    public async Task RotateAsync_ShouldReleaseLockWithCancellationTokenNone_WhenRequestIsCanceled()
    {
        await ResetAuthTablesAsync();

        var cacheService = new CancellationAwareReleaseLockCacheService();
        var user = CreateUser("cancel-release");
        const string oldRawToken = "refresh-old-cancel-release";
        const string newRawToken = "refresh-new-cancel-release";
        var idempotencyKey = $"cancel-release-{Guid.NewGuid():N}";

        await using var context = _fixture.CreateDbContext();
        var repository = CreateRepository(context, cacheService);
        await SeedUserAndTokenAsync(context, user, oldRawToken, DateTime.UtcNow.AddDays(7));

        var request = BuildRotateRequest(oldRawToken, newRawToken, idempotencyKey);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            repository.RotateAsync(request, cts.Token));

        Assert.True(cacheService.ReleaseCalled);
        Assert.False(cacheService.ReleaseCancellationTokenCanBeCanceled);
        Assert.False(cacheService.ReleaseCancellationRequested);
    }

    private static RefreshRotateRequest BuildRotateRequest(string rawToken, string newRawToken, string idempotencyKey)
    {
        return new RefreshRotateRequest
        {
            RawToken = rawToken,
            NewRawToken = newRawToken,
            NewExpiresAtUtc = DateTime.UtcNow.AddDays(14),
            IdempotencyKey = idempotencyKey,
            IpAddress = "127.0.0.1",
            DeviceId = "integration-device",
            UserAgentHash = "integration-ua-hash"
        };
    }

    private static RefreshTokenRepository CreateRepository(ApplicationDbContext dbContext, ICacheService cacheService)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new AuthSecurityOptions
        {
            RequireRedisForRefreshConsistency = false,
            RefreshLockSeconds = 30,
            RefreshIdempotencyWindowSeconds = 120
        });

        return new RefreshTokenRepository(
            dbContext,
            cacheService,
            options,
            new CacheBackendState(usesRedis: false),
            NullLogger<RefreshTokenRepository>.Instance);
    }

    private static User CreateUser(string suffix)
    {
        var now = DateTime.UtcNow;
        return new User(
            email: $"refresh-{suffix}-{Guid.NewGuid():N}@example.test",
            username: $"refresh_{suffix}_{Guid.NewGuid():N}"[..30],
            passwordHash: "hashed-password",
            displayName: "Refresh Test User",
            dateOfBirth: now.AddYears(-25),
            hasConsented: true);
    }

    private static async Task SeedUserAndTokenAsync(
        ApplicationDbContext context,
        User user,
        string rawToken,
        DateTime expiresAtUtc)
    {
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken(
            user.Id,
            rawToken,
            expiresAtUtc,
            "127.0.0.1",
            sessionId: Guid.NewGuid(),
            createdDeviceId: "integration-device",
            createdUserAgentHash: "integration-ua-hash"));
        await context.SaveChangesAsync();
    }

    private async Task ResetAuthTablesAsync()
    {
        await using var context = _fixture.CreateDbContext();
        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE refresh_tokens, auth_sessions, users RESTART IDENTITY CASCADE;");
    }

    private sealed class InMemoryRefreshCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, CacheValue> _values = new(StringComparer.Ordinal);
        private readonly ConcurrentDictionary<string, string> _locks = new(StringComparer.Ordinal);
        private readonly ConcurrentDictionary<string, HashSet<string>> _sets = new(StringComparer.Ordinal);
        private readonly object _setLock = new();

        public bool ForceAcquireFailure { get; set; }

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (_values.TryGetValue(key, out var cacheValue) == false)
            {
                return Task.FromResult<T?>(default);
            }

            if (cacheValue.ExpiresAtUtc.HasValue && cacheValue.ExpiresAtUtc.Value <= DateTime.UtcNow)
            {
                _values.TryRemove(key, out _);
                return Task.FromResult<T?>(default);
            }

            if (cacheValue.Value is T typed)
            {
                return Task.FromResult<T?>(typed);
            }

            return Task.FromResult((T?)cacheValue.Value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var expiresAtUtc = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : (DateTime?)null;
            _values[key] = new CacheValue(value, expiresAtUtc);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _values.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1L);
        }

        public Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            lock (_setLock)
            {
                if (_sets.TryGetValue(key, out var set) == false)
                {
                    set = new HashSet<string>(StringComparer.Ordinal);
                    _sets[key] = set;
                }

                set.Add(member);
            }

            return Task.CompletedTask;
        }

        public Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default)
        {
            lock (_setLock)
            {
                if (_sets.TryGetValue(key, out var set))
                {
                    set.Remove(member);
                }
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
        {
            lock (_setLock)
            {
                if (_sets.TryGetValue(key, out var set))
                {
                    return Task.FromResult<IReadOnlyCollection<string>>(set.ToArray());
                }
            }

            return Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>());
        }

        public Task<bool> AcquireLockAsync(
            string key,
            string owner,
            TimeSpan leaseTime,
            CancellationToken cancellationToken = default)
        {
            if (ForceAcquireFailure)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_locks.TryAdd(key, owner));
        }

        public Task<bool> ReleaseLockAsync(string key, string owner, CancellationToken cancellationToken = default)
        {
            if (_locks.TryGetValue(key, out var currentOwner) == false
                || string.Equals(currentOwner, owner, StringComparison.Ordinal) == false)
            {
                return Task.FromResult(false);
            }

            _locks.TryRemove(key, out _);
            return Task.FromResult(true);
        }

        private readonly record struct CacheValue(object? Value, DateTime? ExpiresAtUtc);
    }

    private sealed class ThrowingReadWriteRefreshCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, string> _locks = new(StringComparer.Ordinal);

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("GetAsync must not be called for refresh idempotency.");
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("SetAsync must not be called for refresh idempotency.");
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1L);
        }

        public Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>());
        }

        public Task<bool> AcquireLockAsync(
            string key,
            string owner,
            TimeSpan leaseTime,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_locks.TryAdd(key, owner));
        }

        public Task<bool> ReleaseLockAsync(string key, string owner, CancellationToken cancellationToken = default)
        {
            if (_locks.TryGetValue(key, out var currentOwner) == false
                || string.Equals(currentOwner, owner, StringComparison.Ordinal) == false)
            {
                return Task.FromResult(false);
            }

            _locks.TryRemove(key, out _);
            return Task.FromResult(true);
        }
    }

    private sealed class CancellationAwareReleaseLockCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, string> _locks = new(StringComparer.Ordinal);

        public bool ReleaseCalled { get; private set; }

        public bool ReleaseCancellationTokenCanBeCanceled { get; private set; }

        public bool ReleaseCancellationRequested { get; private set; }

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1L);
        }

        public Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>());
        }

        public Task<bool> AcquireLockAsync(
            string key,
            string owner,
            TimeSpan leaseTime,
            CancellationToken cancellationToken = default)
        {
            _locks[key] = owner;
            return Task.FromResult(true);
        }

        public Task<bool> ReleaseLockAsync(string key, string owner, CancellationToken cancellationToken = default)
        {
            ReleaseCalled = true;
            ReleaseCancellationTokenCanBeCanceled = cancellationToken.CanBeCanceled;
            ReleaseCancellationRequested = cancellationToken.IsCancellationRequested;
            _locks.TryRemove(key, out _);
            return Task.FromResult(true);
        }
    }
}
