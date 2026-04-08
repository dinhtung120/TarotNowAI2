namespace TarotNow.Domain.Entities;

// Phần hành vi ví của User: bọc các thao tác UserWallet và đồng bộ timestamp cập nhật user.
public partial class User
{
    // Số dư Gold hiện tại lấy từ wallet.
    public long GoldBalance => Wallet.GoldBalance;

    // Số dư Diamond khả dụng hiện tại.
    public long DiamondBalance => Wallet.DiamondBalance;

    // Số Diamond đang bị đóng băng.
    public long FrozenDiamondBalance => Wallet.FrozenDiamondBalance;

    // Tổng Diamond đã mua qua nạp tiền.
    public long TotalDiamondsPurchased => Wallet.TotalDiamondsPurchased;

    /// <summary>
    /// Cộng tiền vào ví người dùng theo loại tiền và loại giao dịch.
    /// Luồng xử lý: ủy quyền xử lý cho Wallet rồi cập nhật UpdatedAt của user.
    /// </summary>
    public void Credit(string currency, long amount, string type)
    {
        Wallet.Credit(currency, amount, type);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Trừ tiền khỏi ví người dùng theo loại tiền.
    /// Luồng xử lý: gọi Wallet.Debit để kiểm tra số dư và cập nhật UpdatedAt.
    /// </summary>
    public void Debit(string currency, long amount)
    {
        Wallet.Debit(currency, amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đóng băng Diamond để giữ tiền chờ chốt nghiệp vụ.
    /// Luồng xử lý: chuyển Diamond khả dụng sang FrozenDiamondBalance và cập nhật timestamp.
    /// </summary>
    public void FreezeDiamond(long amount)
    {
        Wallet.FreezeDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Giải phóng Diamond đóng băng khi cần release cho bên nhận.
    /// Luồng xử lý: trừ FrozenDiamondBalance theo amount và cập nhật thời gian.
    /// </summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        Wallet.ReleaseFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Hoàn Diamond đã đóng băng về số dư khả dụng.
    /// Luồng xử lý: gọi refund trên wallet và cập nhật UpdatedAt của user.
    /// </summary>
    public void RefundFrozenDiamond(long amount)
    {
        Wallet.RefundFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Tiêu thụ Diamond đóng băng khi giao dịch được xác nhận thành công.
    /// Luồng xử lý: giảm FrozenDiamondBalance theo amount và cập nhật timestamp.
    /// </summary>
    public void ConsumeFrozenDiamond(long amount)
    {
        Wallet.ConsumeFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
}
