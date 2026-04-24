using System.Text.Encodings.Web;
using System.Text.Json;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Service quản trị system configs: list, upsert và đồng bộ runtime snapshot/projection.
/// </summary>
public sealed class SystemConfigAdminService : ISystemConfigAdminService
{
    private static readonly JsonSerializerOptions CanonicalJsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

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
        var managedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var items = new List<SystemConfigAdminItem>();

        foreach (var definition in SystemConfigRegistry.GetAll())
        {
            managedKeys.Add(definition.Key);
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

        foreach (var unknown in existingConfigs.Where(x => !managedKeys.Contains(x.Key)))
        {
            items.Add(ToAdminItem(unknown, isKnownKey: false, source: "db"));
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

        await RefreshSnapshotAndProjectionAsync(cancellationToken);
        return ToAdminItem(updated, isKnownKey: SystemConfigRegistry.TryGetDefinition(updated.Key, out _), source: "db");
    }

    private async Task RefreshSnapshotAndProjectionAsync(CancellationToken cancellationToken)
    {
        var reloaded = await _systemConfigRepository.GetAllAsync(cancellationToken);
        foreach (var item in reloaded)
        {
            if (!SystemConfigRegistry.TryGetDefinition(item.Key, out _))
            {
                continue;
            }

            var validation = SystemConfigRegistry.Validate(item.Key, item.Value, item.ValueKind);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException($"System config '{item.Key}' is invalid: {validation.Error}");
            }
        }

        _snapshotStore.Replace(reloaded);
        await _projectionService.ProjectAsync(_snapshotStore.Items, cancellationToken);
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

    private static string ResolveDescription(string? requestedDescription, string? existingDescription, string fallbackDescription)
    {
        if (!string.IsNullOrWhiteSpace(requestedDescription))
        {
            return requestedDescription.Trim();
        }

        if (!string.IsNullOrWhiteSpace(existingDescription))
        {
            return existingDescription.Trim();
        }

        return fallbackDescription;
    }

    private static string NormalizeValue(string value, string valueKind)
    {
        if (string.Equals(valueKind, "json", StringComparison.Ordinal))
        {
            return CanonicalizeJson(value);
        }

        return value.Trim();
    }

    private static string CanonicalizeJson(string rawValue)
    {
        using var document = JsonDocument.Parse(rawValue);
        return JsonSerializer.Serialize(document.RootElement, CanonicalJsonOptions);
    }
}
