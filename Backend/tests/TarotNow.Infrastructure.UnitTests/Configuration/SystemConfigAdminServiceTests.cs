using Microsoft.Extensions.Logging;
using Moq;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure.UnitTests.Configuration;

public sealed class SystemConfigAdminServiceTests
{
    [Fact]
    public async Task UpsertAsync_WhenProjectionFailsForNewKey_ShouldRollbackByDeletingKey()
    {
        var repository = new InMemorySystemConfigRepository();
        var snapshotStore = new SystemConfigSnapshotStore();
        var projectionService = BuildProjectionService();
        var service = new SystemConfigAdminService(
            repository,
            snapshotStore,
            projectionService,
            Mock.Of<IRedisPublisher>(),
            Mock.Of<ILogger<SystemConfigAdminService>>());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpsertAsync(
                key: "gacha.pools",
                value: BuildMinimalGachaPoolsJson(),
                valueKind: "json",
                description: "Projection should fail and rollback.",
                updatedBy: Guid.NewGuid(),
                cancellationToken: CancellationToken.None));

        Assert.Contains("đã rollback", exception.Message, StringComparison.OrdinalIgnoreCase);
        var persisted = await repository.GetByKeyAsync("gacha.pools", CancellationToken.None);
        Assert.Null(persisted);
    }

    [Fact]
    public async Task UpsertAsync_WhenProjectionFailsForExistingKey_ShouldRestorePreviousValue()
    {
        var repository = new InMemorySystemConfigRepository(
            new SystemConfig(
                key: "gacha.pools",
                value: "[]",
                valueKind: "json",
                description: "original",
                updatedBy: null));
        var snapshotStore = new SystemConfigSnapshotStore();
        var projectionService = BuildProjectionService();
        var service = new SystemConfigAdminService(
            repository,
            snapshotStore,
            projectionService,
            Mock.Of<IRedisPublisher>(),
            Mock.Of<ILogger<SystemConfigAdminService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpsertAsync(
                key: "gacha.pools",
                value: BuildMinimalGachaPoolsJson(),
                valueKind: "json",
                description: "updated",
                updatedBy: Guid.NewGuid(),
                cancellationToken: CancellationToken.None));

        var restored = await repository.GetByKeyAsync("gacha.pools", CancellationToken.None);
        Assert.NotNull(restored);
        Assert.Equal("[]", restored!.Value);
        Assert.Equal("original", restored.Description);
    }

    private static SystemConfigProjectionService BuildProjectionService()
    {
        // Truyền null context để ép projection gacha ném lỗi khi config gacha.pools có dữ liệu.
        return new SystemConfigProjectionService(
            dbContext: null!,
            mongoDbContext: null!,
            logger: Mock.Of<ILogger<SystemConfigProjectionService>>());
    }

    private static string BuildMinimalGachaPoolsJson()
    {
        return
            """
            [
              {
                "code": "test_pool",
                "poolType": "normal",
                "nameVi": "Test",
                "nameEn": "Test",
                "nameZh": "Test",
                "descriptionVi": "Test",
                "descriptionEn": "Test",
                "descriptionZh": "Test",
                "costCurrency": "diamond",
                "costAmount": 1,
                "oddsVersion": "v1",
                "isActive": true,
                "rewards": [
                  {
                    "rewardKind": "item",
                    "itemCode": "missing_item",
                    "rarity": "common",
                    "probabilityBasisPoints": 10000
                  }
                ]
              }
            ]
            """;
    }

    private sealed class InMemorySystemConfigRepository : ISystemConfigRepository
    {
        private readonly Dictionary<string, SystemConfig> _store;

        public InMemorySystemConfigRepository(params SystemConfig[] seeds)
        {
            _store = new Dictionary<string, SystemConfig>(StringComparer.OrdinalIgnoreCase);
            foreach (var seed in seeds)
            {
                _store[seed.Key] = seed;
            }
        }

        public Task<IReadOnlyList<SystemConfig>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var clone = _store.Values
                .Select(config => new SystemConfig(
                    config.Key,
                    config.Value,
                    config.ValueKind,
                    config.Description,
                    config.UpdatedBy))
                .ToArray();
            return Task.FromResult<IReadOnlyList<SystemConfig>>(clone);
        }

        public Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            _store.TryGetValue(key, out var config);
            return Task.FromResult(config);
        }

        public Task<SystemConfig> UpsertAsync(
            string key,
            string value,
            string valueKind,
            string? description,
            Guid? updatedBy,
            CancellationToken cancellationToken = default)
        {
            if (_store.TryGetValue(key, out var existing))
            {
                existing.Update(value, valueKind, description, updatedBy);
                return Task.FromResult(existing);
            }

            var created = new SystemConfig(key, value, valueKind, description, updatedBy);
            _store[key] = created;
            return Task.FromResult(created);
        }

        public Task DeleteByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }
    }
}
