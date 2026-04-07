

using System;

namespace TarotNow.Domain.Entities;

public class SubscriptionPlan
{
    public Guid Id { get; private set; }
    
        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public long PriceDiamond { get; private set; }

        public int DurationDays { get; private set; }

        public string EntitlementsJson { get; private set; } = "[]";

        public bool IsActive { get; private set; }

        public int DisplayOrder { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected SubscriptionPlan() { } 

    public SubscriptionPlan(
        string name, 
        string? description, 
        long priceDiamond, 
        int durationDays, 
        string entitlementsJson, 
        int displayOrder)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        PriceDiamond = priceDiamond;
        DurationDays = durationDays;
        EntitlementsJson = entitlementsJson;
        DisplayOrder = displayOrder;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

        public void Update(SubscriptionPlanUpdateDetails details)
    {
        Name = details.Name;
        Description = details.Description;
        PriceDiamond = details.PriceDiamond;
        DurationDays = details.DurationDays;
        EntitlementsJson = details.EntitlementsJson;
        DisplayOrder = details.DisplayOrder;
        IsActive = details.IsActive;
        UpdatedAt = DateTime.UtcNow;
    }
}

public sealed record SubscriptionPlanUpdateDetails(
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    string EntitlementsJson,
    int DisplayOrder,
    bool IsActive);
