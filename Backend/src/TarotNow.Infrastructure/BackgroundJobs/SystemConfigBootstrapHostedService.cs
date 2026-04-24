using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Hosted service bootstrap system config snapshot và projection read-model.
/// </summary>
public sealed class SystemConfigBootstrapHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SystemConfigBootstrapHostedService> _logger;

    public SystemConfigBootstrapHostedService(
        IServiceProvider serviceProvider,
        ILogger<SystemConfigBootstrapHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Nạp snapshot + chạy projection khi host start.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<ISystemConfigRepository>();
        var snapshotStore = scope.ServiceProvider.GetRequiredService<SystemConfigSnapshotStore>();
        var projectionService = scope.ServiceProvider.GetRequiredService<SystemConfigProjectionService>();

        _logger.LogInformation("[SystemConfig] bootstrap started.");

        await EnsureKnownDefaultsAsync(repository, cancellationToken);
        var reloaded = await repository.GetAllAsync(cancellationToken);
        ValidateKnownConfigs(reloaded);

        snapshotStore.Replace(reloaded);
        await projectionService.ProjectAsync(snapshotStore.Items, cancellationToken);

        _logger.LogInformation("[SystemConfig] bootstrap completed with {Count} keys.", reloaded.Count);
    }

    private async Task EnsureKnownDefaultsAsync(ISystemConfigRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetAllAsync(cancellationToken);
        var existingKeys = existing
            .Select(x => x.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var definition in SystemConfigRegistry.GetAll())
        {
            if (existingKeys.Contains(definition.Key))
            {
                continue;
            }

            await repository.UpsertAsync(
                key: definition.Key,
                value: definition.DefaultValue,
                valueKind: definition.ValueKind.ToString().ToLowerInvariant(),
                description: definition.Description,
                updatedBy: null,
                cancellationToken: cancellationToken);
        }
    }

    private void ValidateKnownConfigs(IEnumerable<SystemConfig> configs)
    {
        var unknownKeys = new List<string>();
        foreach (var config in configs)
        {
            if (!SystemConfigRegistry.TryGetDefinition(config.Key, out _))
            {
                unknownKeys.Add(config.Key);
                continue;
            }

            var validation = SystemConfigRegistry.Validate(config.Key, config.Value, config.ValueKind);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(
                    $"System config '{config.Key}' is invalid: {validation.Error}");
            }
        }

        if (unknownKeys.Count == 0)
        {
            return;
        }

        _logger.LogWarning(
            "[SystemConfig] found {Count} unknown keys in DB; skip validation for these keys: {Keys}",
            unknownKeys.Count,
            string.Join(", ", unknownKeys.OrderBy(x => x, StringComparer.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// Service không giữ tài nguyên nền nên stop ngay.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
