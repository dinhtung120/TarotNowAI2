using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter cung cấp danh sách gói nạp từ cấu hình hệ thống.
public sealed class DepositPackageCatalog : IDepositPackageCatalog
{
    private readonly IReadOnlyList<DepositPackageDefinition> _packages;

    /// <summary>
    /// Khởi tạo catalog gói nạp từ options.
    /// </summary>
    public DepositPackageCatalog(IOptions<DepositOptions> options)
    {
        _packages = options.Value.Packages
            .Select(package => new DepositPackageDefinition(
                package.Code.Trim(),
                package.AmountVnd,
                package.BaseDiamond,
                package.IsActive))
            .Where(package => string.IsNullOrWhiteSpace(package.Code) == false)
            .ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyList<DepositPackageDefinition> GetActivePackages()
    {
        return _packages.Where(package => package.IsActive).ToArray();
    }

    /// <inheritdoc />
    public DepositPackageDefinition? FindByCode(string packageCode)
    {
        if (string.IsNullOrWhiteSpace(packageCode))
        {
            return null;
        }

        return _packages.FirstOrDefault(package =>
            string.Equals(package.Code, packageCode.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
