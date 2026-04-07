

using System;
using System.Collections.Generic;
using System.Linq;

namespace TarotNow.Domain.Entities;

public class GachaBanner
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string NameVi { get; private set; } = string.Empty;
    public string NameEn { get; private set; } = string.Empty;
    public string? DescriptionVi { get; private set; }
    public string? DescriptionEn { get; private set; }
    
    
    public long CostDiamond { get; private set; } = 5;
    
    
    public string OddsVersion { get; private set; } = string.Empty;
    
    
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    
    
    public bool PityEnabled { get; private set; } = true;
    public int HardPityCount { get; private set; } = 90;
    
    public bool IsActive { get; private set; } = true;
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    
    private readonly List<GachaBannerItem> _items = new();
    public IReadOnlyCollection<GachaBannerItem> Items => _items.AsReadOnly();

    protected GachaBanner() { } 

    public GachaBanner(string code, string nameVi, string nameEn, long costDiamond, string oddsVersion, DateTime effectiveFrom, DateTime? effectiveTo = null, bool pityEnabled = true, int hardPityCount = 90)
    {
        Id = Guid.NewGuid();
        Code = code;
        NameVi = nameVi;
        NameEn = nameEn;
        CostDiamond = costDiamond;
        OddsVersion = oddsVersion;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        PityEnabled = pityEnabled;
        HardPityCount = hardPityCount;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

        public bool IsCurrentlyActive()
    {
        if (!IsActive) return false;
        var now = DateTime.UtcNow;
        if (now < EffectiveFrom) return false;
        if (EffectiveTo.HasValue && now > EffectiveTo.Value) return false;
        return true;
    }

        public bool ValidateOddsSum(IEnumerable<GachaBannerItem> itemsToValidate)
    {
        return itemsToValidate.Sum(x => x.WeightBasisPoints) == 10000;
    }
}
