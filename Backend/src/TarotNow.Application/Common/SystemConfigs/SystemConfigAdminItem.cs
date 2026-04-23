namespace TarotNow.Application.Common.SystemConfigs;

/// <summary>
/// DTO quản trị cho một key system config.
/// </summary>
public sealed class SystemConfigAdminItem
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string ValueKind { get; init; } = "scalar";
    public string? Description { get; init; }
    public Guid? UpdatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public bool IsKnownKey { get; init; }
    public string Source { get; init; } = "db";
}
