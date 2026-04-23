using System.Text.Json;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Projection dữ liệu cấu hình từ system_configs xuống các read-model runtime.
/// </summary>
public sealed partial class SystemConfigProjectionService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ApplicationDbContext _dbContext;
    private readonly MongoDbContext _mongoDbContext;
    private readonly ILogger<SystemConfigProjectionService> _logger;

    public SystemConfigProjectionService(
        ApplicationDbContext dbContext,
        MongoDbContext mongoDbContext,
        ILogger<SystemConfigProjectionService> logger)
    {
        _dbContext = dbContext;
        _mongoDbContext = mongoDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Đồng bộ projection theo snapshot cấu hình đã load ở startup.
    /// </summary>
    public async Task ProjectAsync(IReadOnlyDictionary<string, SnapshotItem> configs, CancellationToken cancellationToken = default)
    {
        await ProjectGachaAsync(configs, cancellationToken);
        await ProjectGamificationAsync(configs, cancellationToken);
    }

    private static List<TItem> DeserializeList<TItem>(string json, string key)
    {
        try
        {
            return JsonSerializer.Deserialize<List<TItem>>(json, JsonOptions) ?? [];
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Invalid JSON for system config key '{key}'.", ex);
        }
    }

    private static string NormalizeRequired(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required value: {paramName}.");
        }

        return value.Trim();
    }

    private static string NormalizeRequiredLower(string? value, string paramName)
    {
        return NormalizeRequired(value, paramName).ToLowerInvariant();
    }
}
