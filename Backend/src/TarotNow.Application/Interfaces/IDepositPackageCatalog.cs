namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract cung cấp danh sách gói nạp preset cho hệ thống nạp tiền.
/// </summary>
public interface IDepositPackageCatalog
{
    /// <summary>
    /// Lấy toàn bộ gói nạp đang active.
    /// </summary>
    IReadOnlyList<DepositPackageDefinition> GetActivePackages();

    /// <summary>
    /// Tìm gói nạp theo mã gói.
    /// </summary>
    DepositPackageDefinition? FindByCode(string packageCode);
}

/// <summary>
/// Định nghĩa một gói nạp preset.
/// </summary>
public sealed record DepositPackageDefinition(
    string Code,
    long AmountVnd,
    long BaseDiamond,
    bool IsActive);
