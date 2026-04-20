namespace TarotNow.Infrastructure.Options;

// Cấu hình tính năng nạp tiền.
public sealed class DepositOptions
{
    // Url callback khi người dùng quay về sau khi thanh toán.
    public string ReturnUrl { get; set; } = string.Empty;

    // Url callback khi người dùng hủy thanh toán.
    public string CancelUrl { get; set; } = string.Empty;

    // Thời lượng hiệu lực payment link (phút).
    public int LinkExpiryMinutes { get; set; } = 15;

    // Danh sách gói nạp preset.
    public List<DepositPackageOption> Packages { get; set; } =
    [
        new DepositPackageOption { Code = "topup_50k", AmountVnd = 50_000, BaseDiamond = 500, IsActive = true },
        new DepositPackageOption { Code = "topup_100k", AmountVnd = 100_000, BaseDiamond = 1_000, IsActive = true },
        new DepositPackageOption { Code = "topup_200k", AmountVnd = 200_000, BaseDiamond = 2_000, IsActive = true },
        new DepositPackageOption { Code = "topup_500k", AmountVnd = 500_000, BaseDiamond = 5_000, IsActive = true },
        new DepositPackageOption { Code = "topup_1m", AmountVnd = 1_000_000, BaseDiamond = 10_000, IsActive = true }
    ];
}

// Cấu hình từng gói nạp preset.
public sealed class DepositPackageOption
{
    // Mã gói nạp.
    public string Code { get; set; } = string.Empty;

    // Số tiền gói theo VND.
    public long AmountVnd { get; set; }

    // Diamond cơ bản của gói.
    public long BaseDiamond { get; set; }

    // Trạng thái active của gói.
    public bool IsActive { get; set; }
}
