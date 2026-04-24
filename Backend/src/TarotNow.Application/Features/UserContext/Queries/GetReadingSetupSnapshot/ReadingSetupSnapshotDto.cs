using System.Collections.Generic;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Features.Wallet.Queries;

namespace TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

public class ReadingSetupSnapshotDto
{
    public WalletBalanceDto Wallet { get; set; } = null!;

    public List<CardCatalogDto> CardsCatalog { get; set; } = [];

    public ReadingFreeDrawQuotaDto FreeDrawQuotas { get; set; } = new();

    public ReadingPricingDto Pricing { get; set; } = new();
}

public sealed class ReadingFreeDrawQuotaDto
{
    public int Spread3 { get; set; }

    public int Spread5 { get; set; }

    public int Spread10 { get; set; }
}

public sealed class ReadingPricingDto
{
    public long Spread3GoldCost { get; set; }
    public long Spread3DiamondCost { get; set; }
    
    public long Spread5GoldCost { get; set; }
    public long Spread5DiamondCost { get; set; }
    
    public long Spread10GoldCost { get; set; }
    public long Spread10DiamondCost { get; set; }
}
