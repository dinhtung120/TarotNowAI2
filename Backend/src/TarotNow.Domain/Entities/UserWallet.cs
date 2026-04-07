

using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

public class UserWallet
{
        public long GoldBalance { get; private set; } = 0;

        public long DiamondBalance { get; private set; } = 0;

        public long FrozenDiamondBalance { get; private set; } = 0;

        public long TotalDiamondsPurchased { get; private set; } = 0;

        protected UserWallet() { }

        public static UserWallet CreateDefault() => new UserWallet();

    
    
    

        public void Credit(string currency, long amount, string type)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền cộng vào phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            GoldBalance += amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            DiamondBalance += amount;
            
            if (type == TransactionType.Deposit)
            {
                TotalDiamondsPurchased += amount;
            }
        }
        else
        {
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

        public void Debit(string currency, long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền trừ đi phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            if (GoldBalance < amount)
                throw new InvalidOperationException("Số dư Gold không đủ.");
            GoldBalance -= amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            if (DiamondBalance < amount)
                throw new InvalidOperationException("Số dư Diamond không đủ.");
            DiamondBalance -= amount;
        }
        else
        {
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

        public void FreezeDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền đóng băng phải lớn hơn 0.", nameof(amount));

        if (DiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond không đủ để đóng băng.");

        DiamondBalance -= amount;
        FrozenDiamondBalance += amount;
    }

        public void ReleaseFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền giải phóng phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để giải phóng.");

        FrozenDiamondBalance -= amount;
    }

        public void RefundFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền hoàn trả phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để hoàn trả.");

        FrozenDiamondBalance -= amount;
        DiamondBalance += amount;
    }

        public void ConsumeFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền tiêu thụ phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để tiêu thụ.");

        FrozenDiamondBalance -= amount;
    }
}
