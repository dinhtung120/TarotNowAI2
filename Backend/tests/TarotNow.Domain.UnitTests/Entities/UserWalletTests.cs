using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.UnitTests.Entities;

public class UserWalletTests
{
    [Fact]
    public void Credit_DiamondDeposit_ShouldIncreaseBalanceAndPurchasedTotal()
    {
        var wallet = UserWallet.CreateDefault();

        wallet.Credit(CurrencyType.Diamond, 50, TransactionType.Deposit);

        Assert.Equal(50, wallet.DiamondBalance);
        Assert.Equal(50, wallet.TotalDiamondsPurchased);
    }

    [Fact]
    public void Credit_DiamondNonDeposit_ShouldNotIncreasePurchasedTotal()
    {
        var wallet = UserWallet.CreateDefault();

        wallet.Credit(CurrencyType.Diamond, 20, TransactionType.AdminTopup);

        Assert.Equal(20, wallet.DiamondBalance);
        Assert.Equal(0, wallet.TotalDiamondsPurchased);
    }

    [Fact]
    public void Debit_WhenInsufficientBalance_ShouldThrow()
    {
        var wallet = UserWallet.CreateDefault();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            wallet.Debit(CurrencyType.Gold, 1));

        Assert.Equal("Số dư Gold không đủ.", exception.Message);
    }

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

    [Fact]
    public void Credit_WithInvalidCurrency_ShouldThrowArgumentException()
    {
        var wallet = UserWallet.CreateDefault();

        Assert.Throws<ArgumentException>(() => wallet.Credit("token", 10, TransactionType.Deposit));
    }
}
