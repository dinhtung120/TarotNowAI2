using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.UnitTests.Entities;

// Unit test cho hành vi ví người dùng ở tầng Domain.
public class UserWalletTests
{
    /// <summary>
    /// Xác nhận nạp diamond từ deposit tăng cả số dư và tổng đã mua.
    /// Luồng này kiểm tra business rule theo loại transaction Deposit.
    /// </summary>
    [Fact]
    public void Credit_DiamondDeposit_ShouldIncreaseBalanceAndPurchasedTotal()
    {
        var wallet = UserWallet.CreateDefault();

        wallet.Credit(CurrencyType.Diamond, 50, TransactionType.Deposit);

        Assert.Equal(50, wallet.DiamondBalance);
        Assert.Equal(50, wallet.TotalDiamondsPurchased);
    }

    /// <summary>
    /// Xác nhận nạp diamond không phải deposit không tăng tổng đã mua.
    /// Luồng này bảo vệ chỉ số total purchased chỉ tính cho giao dịch mua thật.
    /// </summary>
    [Fact]
    public void Credit_DiamondNonDeposit_ShouldNotIncreasePurchasedTotal()
    {
        var wallet = UserWallet.CreateDefault();

        wallet.Credit(CurrencyType.Diamond, 20, TransactionType.AdminTopup);

        Assert.Equal(20, wallet.DiamondBalance);
        Assert.Equal(0, wallet.TotalDiamondsPurchased);
    }

    /// <summary>
    /// Xác nhận debit khi không đủ số dư sẽ ném exception.
    /// Luồng này bảo vệ tính toàn vẹn số dư ví khỏi âm.
    /// </summary>
    [Fact]
    public void Debit_WhenInsufficientBalance_ShouldThrow()
    {
        var wallet = UserWallet.CreateDefault();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            wallet.Debit(CurrencyType.Gold, 1));

        Assert.Equal("Số dư Gold không đủ.", exception.Message);
    }

    /// <summary>
    /// Xác nhận freeze rồi refund khôi phục lại số dư diamond ban đầu.
    /// Luồng này kiểm tra nhánh hoàn tiền từ số dư frozen.
    /// </summary>
    [Fact]
    public void FreezeAndRefund_ShouldRestoreDiamondBalance()
    {
        var wallet = UserWallet.CreateDefault();
        wallet.Credit(CurrencyType.Diamond, 100, TransactionType.Deposit);

        wallet.FreezeDiamond(40);
        wallet.RefundFrozenDiamond(40);

        Assert.Equal(100, wallet.DiamondBalance);
        Assert.Equal(0, wallet.FrozenDiamondBalance);
    }

    /// <summary>
    /// Xác nhận release từ frozen tiêu thụ đúng phần frozen và giữ số dư khả dụng còn lại.
    /// Luồng này kiểm tra nhánh thanh toán thành công từ escrow.
    /// </summary>
    [Fact]
    public void FreezeAndRelease_ShouldConsumeFrozenBalance()
    {
        var wallet = UserWallet.CreateDefault();
        wallet.Credit(CurrencyType.Diamond, 100, TransactionType.Deposit);
        wallet.FreezeDiamond(70);

        wallet.ReleaseFrozenDiamond(30);

        Assert.Equal(30, wallet.DiamondBalance);
        Assert.Equal(40, wallet.FrozenDiamondBalance);
    }

    /// <summary>
    /// Xác nhận consume frozen chỉ giảm frozen balance theo amount.
    /// Luồng này dùng cho các khoản phí lấy trực tiếp từ phần đã khóa.
    /// </summary>
    [Fact]
    public void ConsumeFrozenDiamond_ShouldDecreaseFrozenOnly()
    {
        var wallet = UserWallet.CreateDefault();
        wallet.Credit(CurrencyType.Diamond, 25, TransactionType.Deposit);
        wallet.FreezeDiamond(20);

        wallet.ConsumeFrozenDiamond(15);

        Assert.Equal(5, wallet.DiamondBalance);
        Assert.Equal(5, wallet.FrozenDiamondBalance);
    }

    /// <summary>
    /// Xác nhận currency không hợp lệ bị từ chối khi credit.
    /// Luồng này bảo vệ whitelist đơn vị tiền tệ của hệ thống.
    /// </summary>
    [Fact]
    public void Credit_WithInvalidCurrency_ShouldThrowArgumentException()
    {
        var wallet = UserWallet.CreateDefault();

        Assert.Throws<ArgumentException>(() => wallet.Credit("token", 10, TransactionType.Deposit));
    }
}
