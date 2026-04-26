using Npgsql;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    private static RedisBootstrapSettings? TryLoadRedisBootstrapSettings(string postgreSqlConnectionString)
    {
        try
        {
            var data = LoadRedisBootstrapRawValues(postgreSqlConnectionString);
            return BuildRedisBootstrapSettings(data);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(
                "[RedisBootstrap] Unable to load bootstrap tuning values from PostgreSQL. " +
                $"Using static fallback values. Reason={ex.GetType().Name}: {ex.Message}");
            return null;
        }
    }

    private static IReadOnlyDictionary<string, string> LoadRedisBootstrapRawValues(string postgreSqlConnectionString)
    {
        using var connection = new NpgsqlConnection(postgreSqlConnectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            """
            SELECT key, value
            FROM system_configs
            WHERE key IN (
                'operational.redis.connect_timeout_ms',
                'operational.redis.sync_timeout_ms',
                'operational.redis.connect_retry'
            );
            """,
            connection);

        using var reader = command.ExecuteReader();
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        while (reader.Read())
        {
            data[reader.GetString(0)] = reader.GetString(1);
        }

        return data;
    }

    private static RedisBootstrapSettings BuildRedisBootstrapSettings(IReadOnlyDictionary<string, string> data)
    {
        var fallback = new SystemConfigOptions().Operational.Redis;
        return new RedisBootstrapSettings
        {
            ConnectTimeoutMs = ParseIntOrFallback(
                data,
                "operational.redis.connect_timeout_ms",
                fallback.ConnectTimeoutMs,
                100,
                60_000),
            SyncTimeoutMs = ParseIntOrFallback(
                data,
                "operational.redis.sync_timeout_ms",
                fallback.SyncTimeoutMs,
                100,
                60_000),
            ConnectRetry = ParseIntOrFallback(
                data,
                "operational.redis.connect_retry",
                fallback.ConnectRetry,
                0,
                20)
        };
    }

    private static int ParseIntOrFallback(
        IReadOnlyDictionary<string, string> values,
        string key,
        int fallback,
        int min,
        int max)
    {
        if (!values.TryGetValue(key, out var raw))
        {
            return Math.Clamp(fallback, min, max);
        }

        return int.TryParse(raw, out var parsed)
            ? Math.Clamp(parsed, min, max)
            : Math.Clamp(fallback, min, max);
    }

    private sealed class RedisBootstrapSettings
    {
        public int ConnectTimeoutMs { get; init; }
        public int SyncTimeoutMs { get; init; }
        public int ConnectRetry { get; init; }
    }
}
