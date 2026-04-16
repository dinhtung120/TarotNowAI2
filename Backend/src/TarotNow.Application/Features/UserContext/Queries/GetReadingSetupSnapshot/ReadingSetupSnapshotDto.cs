using System.Collections.Generic;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Features.Wallet.Queries;

namespace TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

public class ReadingSetupSnapshotDto
{
    public WalletBalanceDto Wallet { get; set; } = null!;

    public List<CardCatalogDto> CardsCatalog { get; set; } = [];

    public ReadingFreeDrawQuotaDto FreeDrawQuotas { get; set; } = new();
}

public sealed class ReadingFreeDrawQuotaDto
{
    public int Spread3 { get; set; }

    public int Spread5 { get; set; }

    public int Spread10 { get; set; }
}
