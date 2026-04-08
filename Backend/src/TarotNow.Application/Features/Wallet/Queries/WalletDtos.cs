using System;

namespace TarotNow.Application.Features.Wallet.Queries;

// DTO số dư ví của người dùng.
public class WalletBalanceDto
{
    // Số dư vàng khả dụng.
    public long GoldBalance { get; set; }

    // Số dư kim cương khả dụng.
    public long DiamondBalance { get; set; }

    // Số kim cương đang bị đóng băng (escrow).
    public long FrozenDiamondBalance { get; set; }
}

// DTO một dòng giao dịch trong lịch sử ví.
public class WalletTransactionDto
{
    // Định danh giao dịch.
    public Guid Id { get; set; }

    // Loại tiền tệ của giao dịch.
    public string Currency { get; set; } = string.Empty;

    // Loại nghiệp vụ giao dịch.
    public string Type { get; set; } = string.Empty;

    // Số tiền thay đổi của giao dịch.
    public long Amount { get; set; }

    // Số dư trước giao dịch.
    public long BalanceBefore { get; set; }

    // Số dư sau giao dịch.
    public long BalanceAfter { get; set; }

    // Mô tả nghiệp vụ giao dịch (nếu có).
    public string? Description { get; set; }

    // Thời điểm tạo giao dịch.
    public DateTime CreatedAt { get; set; }
}
