using MediatR;

namespace TarotNow.Application.Features.Deposit.Queries.ListDepositPackages;

// Query lấy danh sách gói nạp preset cho người dùng.
public class ListDepositPackagesQuery : IRequest<IEnumerable<DepositPackageDto>>
{
}

// DTO gói nạp trả về cho client.
public class DepositPackageDto
{
    // Mã gói nạp.
    public string Code { get; set; } = string.Empty;

    // Giá trị gói theo VND.
    public long AmountVnd { get; set; }

    // Diamond cơ bản của gói.
    public long BaseDiamondAmount { get; set; }

    // Gold khuyến mãi theo campaign hiện tại.
    public long BonusGoldAmount { get; set; }

    // Tổng Diamond user nhận khi nạp gói này.
    public long TotalDiamondAmount { get; set; }
}
