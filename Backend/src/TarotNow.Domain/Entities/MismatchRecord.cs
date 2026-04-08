
namespace TarotNow.Domain.Entities;

// DTO miền dữ liệu sai lệch số dư giữa bảng user wallet và sổ cái giao dịch.
public class MismatchRecord
{
    // Người dùng có sai lệch.
    public Guid UserId { get; set; }

    // Số dư Gold ghi nhận ở bảng user.
    public long UserGoldBalance { get; set; }

    // Số dư Gold tính từ ledger.
    public long LedgerGoldBalance { get; set; }

    // Số dư Diamond ghi nhận ở bảng user.
    public long UserDiamondBalance { get; set; }

    // Số dư Diamond tính từ ledger.
    public long LedgerDiamondBalance { get; set; }
}
