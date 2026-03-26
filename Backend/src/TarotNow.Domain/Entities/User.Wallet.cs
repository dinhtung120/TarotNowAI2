namespace TarotNow.Domain.Entities;

public partial class User
{
    public long GoldBalance => Wallet.GoldBalance;

    public long DiamondBalance => Wallet.DiamondBalance;

    public long FrozenDiamondBalance => Wallet.FrozenDiamondBalance;

    public long TotalDiamondsPurchased => Wallet.TotalDiamondsPurchased;

    public void Credit(string currency, long amount, string type)
    {
        Wallet.Credit(currency, amount, type);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Debit(string currency, long amount)
    {
        Wallet.Debit(currency, amount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void FreezeDiamond(long amount)
    {
        Wallet.FreezeDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReleaseFrozenDiamond(long amount)
    {
        Wallet.ReleaseFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RefundFrozenDiamond(long amount)
    {
        Wallet.RefundFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConsumeFrozenDiamond(long amount)
    {
        Wallet.ConsumeFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
}
