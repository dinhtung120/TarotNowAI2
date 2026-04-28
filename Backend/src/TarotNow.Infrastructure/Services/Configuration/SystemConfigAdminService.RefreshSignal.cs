using Microsoft.Extensions.Logging;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigAdminService
{
    internal const string SystemConfigRefreshChannel = "runtime:system-configs";
    internal const string SystemConfigRefreshEventName = "system-config.changed";

    private async Task PublishRefreshSignalSafeAsync(
        SystemConfig updatedConfig,
        CancellationToken cancellationToken)
    {
        try
        {
            await _redisPublisher.PublishAsync(
                SystemConfigRefreshChannel,
                SystemConfigRefreshEventName,
                new
                {
                    key = updatedConfig.Key,
                    updatedAt = updatedConfig.UpdatedAt,
                    source = "admin_system_config_upsert"
                },
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "Failed to publish system config refresh event for key {Key}. Local runtime snapshot was refreshed successfully.",
                updatedConfig.Key);
        }
    }
}
