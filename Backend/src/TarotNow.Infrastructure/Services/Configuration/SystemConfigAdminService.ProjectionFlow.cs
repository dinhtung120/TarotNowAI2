using TarotNow.Application.Common.SystemConfigs;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigAdminService
{
    private async Task RollbackConfigUpsertAsync(
        string normalizedKey,
        SystemConfigRollbackSnapshot? rollbackSnapshot,
        Guid updatedBy,
        CancellationToken cancellationToken)
    {
        if (rollbackSnapshot is null)
        {
            await _systemConfigRepository.DeleteByKeyAsync(normalizedKey, cancellationToken);
            return;
        }

        await _systemConfigRepository.UpsertAsync(
            normalizedKey,
            rollbackSnapshot.Value,
            rollbackSnapshot.ValueKind,
            rollbackSnapshot.Description,
            rollbackSnapshot.UpdatedBy ?? updatedBy,
            cancellationToken);
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

        var candidateSnapshot = reloaded.ToDictionary(
            item => item.Key,
            item => new SnapshotItem(
                item.Key,
                item.Value,
                item.ValueKind,
                item.Description,
                item.UpdatedBy,
                item.UpdatedAt),
            StringComparer.OrdinalIgnoreCase);
        await _projectionService.ProjectAsync(candidateSnapshot, cancellationToken);
        _snapshotStore.Replace(reloaded);
    }

    private sealed record SystemConfigRollbackSnapshot(
        string Value,
        string ValueKind,
        string? Description,
        Guid? UpdatedBy);
}
