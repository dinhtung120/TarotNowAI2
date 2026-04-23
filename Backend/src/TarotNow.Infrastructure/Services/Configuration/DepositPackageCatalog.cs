using Microsoft.Extensions.Options;
using System.Text.Json;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter cung cấp danh sách gói nạp từ cấu hình hệ thống.
public sealed class DepositPackageCatalog : IDepositPackageCatalog
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IOptions<DepositOptions> _options;
    private readonly SystemConfigSnapshotStore _snapshotStore;

    /// <summary>
    /// Khởi tạo catalog gói nạp từ options.
    /// </summary>
    public DepositPackageCatalog(
        IOptions<DepositOptions> options,
        SystemConfigSnapshotStore snapshotStore)
    {
        _options = options;
        _snapshotStore = snapshotStore;
    }

    /// <inheritdoc />
    public IReadOnlyList<DepositPackageDefinition> GetActivePackages()
    {
        return LoadPackages()
            .Where(package => package.IsActive)
            .ToArray();
    }

    /// <inheritdoc />
    public DepositPackageDefinition? FindByCode(string packageCode)
    {
        if (string.IsNullOrWhiteSpace(packageCode))
        {
            return null;
        }

        return LoadPackages().FirstOrDefault(package =>
            string.Equals(package.Code, packageCode.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private IReadOnlyList<DepositPackageDefinition> LoadPackages()
    {
        if (_snapshotStore.TryGetValue("deposit.packages", out var rawJson))
        {
            try
            {
                var fromConfig = JsonSerializer.Deserialize<List<DepositPackageOption>>(rawJson, JsonOptions);
                if (fromConfig is { Count: > 0 })
                {
                    return fromConfig
                        .Select(ToDefinition)
                        .Where(package => string.IsNullOrWhiteSpace(package.Code) == false)
                        .ToArray();
                }
            }
            catch
            {
                // Fail-safe fallback về options khi JSON không hợp lệ.
            }
        }

        return _options.Value.Packages
            .Select(ToDefinition)
            .Where(package => string.IsNullOrWhiteSpace(package.Code) == false)
            .ToArray();
    }

    private static DepositPackageDefinition ToDefinition(DepositPackageOption package)
    {
        return new DepositPackageDefinition(
            package.Code.Trim(),
            package.AmountVnd,
            package.BaseDiamond,
            package.IsActive);
    }
}
