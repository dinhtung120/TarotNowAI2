using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Service quản trị system configs: list, upsert và đồng bộ runtime snapshot/projection.
/// </summary>
public sealed partial class SystemConfigAdminService : ISystemConfigAdminService
{
    private readonly ISystemConfigRepository _systemConfigRepository;
    private readonly SystemConfigSnapshotStore _snapshotStore;
    private readonly SystemConfigProjectionService _projectionService;

    public SystemConfigAdminService(
        ISystemConfigRepository systemConfigRepository,
        SystemConfigSnapshotStore snapshotStore,
        SystemConfigProjectionService projectionService)
    {
        _systemConfigRepository = systemConfigRepository;
        _snapshotStore = snapshotStore;
        _projectionService = projectionService;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SystemConfigAdminItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        var existingConfigs = await _systemConfigRepository.GetAllAsync(cancellationToken);
        var existingByKey = existingConfigs.ToDictionary(x => x.Key, StringComparer.OrdinalIgnoreCase);
        var items = new List<SystemConfigAdminItem>();

        foreach (var definition in SystemConfigRegistry.GetAll())
        {
            if (existingByKey.TryGetValue(definition.Key, out var existing))
            {
                items.Add(ToAdminItem(existing, isKnownKey: true, source: "db"));
                continue;
            }

            items.Add(new SystemConfigAdminItem
            {
                Key = definition.Key,
                Value = definition.DefaultValue,
                ValueKind = definition.ValueKind.ToString().ToLowerInvariant(),
                Description = definition.Description,
                UpdatedBy = null,
                UpdatedAt = null,
                IsKnownKey = true,
                Source = "default"
            });
        }

        return items
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <inheritdoc />
    public async Task<SystemConfigAdminItem> UpsertAsync(
        string key,
        string value,
        string valueKind,
        string? description,
        Guid updatedBy,
        CancellationToken cancellationToken = default)
    {
        var normalizedKey = SystemConfigRegistry.NormalizeKey(key);
        var normalizedKind = SystemConfigRegistry.NormalizeValueKind(valueKind);
        var existing = await _systemConfigRepository.GetByKeyAsync(normalizedKey, cancellationToken);
        var rollbackSnapshot = existing is null
            ? null
            : new SystemConfigRollbackSnapshot(
                existing.Value,
                existing.ValueKind,
                existing.Description,
                existing.UpdatedBy);
        var fallbackDescription = SystemConfigRegistry.TryGetDefinition(normalizedKey, out var definition)
            ? definition.Description
            : string.Empty;
        var effectiveDescription = ResolveDescription(description, existing?.Description, fallbackDescription);
        var normalizedValue = NormalizeValue(value, normalizedKind);

        var updated = await _systemConfigRepository.UpsertAsync(
            normalizedKey,
            normalizedValue,
            normalizedKind,
            effectiveDescription,
            updatedBy,
            cancellationToken);

        try
        {
            await RefreshSnapshotAndProjectionAsync(cancellationToken);
        }
        catch (Exception projectionException)
        {
            await RollbackConfigUpsertAsync(
                normalizedKey,
                rollbackSnapshot,
                updatedBy,
                cancellationToken);
            await RefreshSnapshotAndProjectionAsync(cancellationToken);

            throw new InvalidOperationException(
                $"System config '{normalizedKey}' không thể apply projection runtime, đã rollback thay đổi DB.",
                projectionException);
        }

        return ToAdminItem(updated, isKnownKey: SystemConfigRegistry.TryGetDefinition(updated.Key, out _), source: "db");
    }

    private static SystemConfigAdminItem ToAdminItem(SystemConfig config, bool isKnownKey, string source)
    {
        return new SystemConfigAdminItem
        {
            Key = config.Key,
            Value = config.Value,
            ValueKind = config.ValueKind,
            Description = config.Description,
            UpdatedBy = config.UpdatedBy,
            UpdatedAt = config.UpdatedAt,
            IsKnownKey = isKnownKey,
            Source = source
        };
    }

}
