using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Subscriber đồng bộ runtime system config snapshot giữa nhiều node qua Redis Pub/Sub.
/// </summary>
public sealed class SystemConfigRuntimeRefreshSubscriberService : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SystemConfigRuntimeRefreshSubscriberService> _logger;

    public SystemConfigRuntimeRefreshSubscriberService(
        IServiceProvider serviceProvider,
        ILogger<SystemConfigRuntimeRefreshSubscriberService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var multiplexer = _serviceProvider.GetService<IConnectionMultiplexer>();
        if (multiplexer is null)
        {
            _logger.LogInformation("System config refresh subscriber disabled because Redis multiplexer is unavailable.");
            return;
        }

        var subscriber = multiplexer.GetSubscriber();
        Action<RedisChannel, RedisValue> callback = (channel, value) =>
        {
            _ = HandleMessageSafeAsync(value.ToString(), CancellationToken.None);
        };

        await subscriber.SubscribeAsync(
            RedisChannel.Literal(SystemConfigAdminService.SystemConfigRefreshChannel),
            callback);
        _logger.LogInformation(
            "Subscribed system config refresh channel {Channel}.",
            SystemConfigAdminService.SystemConfigRefreshChannel);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            await subscriber.UnsubscribeAsync(
                RedisChannel.Literal(SystemConfigAdminService.SystemConfigRefreshChannel),
                callback);
        }
    }

    private async Task HandleMessageSafeAsync(string payloadJson, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return;
        }

        try
        {
            var envelope = JsonSerializer.Deserialize<RedisRealtimeEnvelope>(payloadJson, JsonOptions);
            if (envelope is null
                || !string.Equals(
                    envelope.EventName,
                    SystemConfigAdminService.SystemConfigRefreshEventName,
                    StringComparison.Ordinal))
            {
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var configAdminService = scope.ServiceProvider.GetRequiredService<ISystemConfigAdminService>();
            await configAdminService.RefreshRuntimeAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Failed to handle system config refresh message.");
        }
    }
}
