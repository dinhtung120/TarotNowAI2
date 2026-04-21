namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Danh mục ngân hàng nội địa hỗ trợ payout theo chuẩn VietQR/NAPAS.
/// </summary>
public static class VietnamBankCatalog
{
    private static readonly Dictionary<string, string> BankNameByBin = new(StringComparer.Ordinal)
    {
        ["970436"] = "Vietcombank",
        ["970407"] = "Techcombank",
        ["970418"] = "BIDV",
        ["970405"] = "Agribank",
        ["970422"] = "MB Bank",
        ["970416"] = "ACB",
        ["970432"] = "VPBank",
        ["970423"] = "TPBank",
        ["970415"] = "VietinBank",
        ["970403"] = "Sacombank",
        ["970431"] = "Eximbank",
        ["970448"] = "OCB",
        ["970441"] = "VIB",
        ["970437"] = "HDBank",
        ["970454"] = "VietCapitalBank",
        ["970446"] = "BaoVietBank",
        ["970452"] = "KienlongBank",
        ["970400"] = "Saigonbank",
        ["970408"] = "PVcomBank",
        ["970443"] = "SHB",
        ["970429"] = "SCB"
    };

    /// <summary>
    /// Lấy danh sách ngân hàng hỗ trợ theo mã BIN.
    /// </summary>
    public static IReadOnlyCollection<VietnamBankOption> GetAll()
    {
        return BankNameByBin
            .Select(item => new VietnamBankOption(item.Key, item.Value))
            .OrderBy(item => item.Name, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Kiểm tra cặp mã BIN và tên ngân hàng có hợp lệ hay không.
    /// </summary>
    public static bool IsValidPair(string? bankBin, string? bankName)
    {
        var normalizedBin = bankBin?.Trim();
        var normalizedName = bankName?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedBin) || string.IsNullOrWhiteSpace(normalizedName))
        {
            return false;
        }

        if (!BankNameByBin.TryGetValue(normalizedBin, out var expectedName))
        {
            return false;
        }

        return string.Equals(expectedName, normalizedName, StringComparison.Ordinal);
    }
}

/// <summary>
/// Thông tin một ngân hàng trong danh mục payout.
/// </summary>
public sealed record VietnamBankOption(string Bin, string Name);
