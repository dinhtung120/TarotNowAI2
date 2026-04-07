

namespace TarotNow.Application.Interfaces;

public interface IRngService
{
        int[] ShuffleDeck(int deckSize = 78);

        GachaRngResult WeightedSelect(System.Collections.Generic.IEnumerable<WeightedItem> items, string? seedForAudit = null);
}

public class WeightedItem
{
    public System.Guid ItemId { get; set; }
    public int WeightBasisPoints { get; set; }
}

public class GachaRngResult
{
    public System.Guid SelectedItemId { get; set; }
    public string RngSeed { get; set; } = string.Empty;
}
