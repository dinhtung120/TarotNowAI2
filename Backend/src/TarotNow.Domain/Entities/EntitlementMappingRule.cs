

using System;

namespace TarotNow.Domain.Entities;

public class EntitlementMappingRule
{
    public Guid Id { get; private set; }
    
        public string SourceKey { get; private set; } = string.Empty;

        public string TargetKey { get; private set; } = string.Empty;

        public decimal Ratio { get; private set; }

        public bool IsEnabled { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected EntitlementMappingRule() { }

    public EntitlementMappingRule(
        string sourceKey, 
        string targetKey, 
        decimal ratio, 
        bool isEnabled = false)
    {
        Id = Guid.NewGuid();
        SourceKey = sourceKey;
        TargetKey = targetKey;
        Ratio = ratio;
        IsEnabled = isEnabled;
        CreatedAt = DateTime.UtcNow;
    }

        public void UpdateRatioOrStatus(decimal ratio, bool isEnabled)
    {
        Ratio = ratio;
        IsEnabled = isEnabled;
        UpdatedAt = DateTime.UtcNow;
    }
}
