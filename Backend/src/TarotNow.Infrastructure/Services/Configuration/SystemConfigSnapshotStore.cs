using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Snapshot in-memory cho system configs tại thời điểm startup.
/// </summary>
public sealed class SystemConfigSnapshotStore
{
    private IReadOnlyDictionary<string, SnapshotItem> _items =
        new Dictionary<string, SnapshotItem>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Cờ cho biết snapshot đã được nạp lần đầu hay chưa.
    /// </summary>
    public bool IsLoaded { get; private set; }

    /// <summary>
    /// Lấy toàn bộ snapshot hiện tại.
    /// </summary>
    public IReadOnlyDictionary<string, SnapshotItem> Items => _items;

    /// <summary>
    /// Nạp lại snapshot từ danh sách cấu hình DB.
    /// </summary>
    public void Replace(IEnumerable<SystemConfig> configs)
    {
        var next = new Dictionary<string, SnapshotItem>(StringComparer.OrdinalIgnoreCase);
        foreach (var config in configs)
        {
            next[config.Key] = new SnapshotItem(
                config.Key,
                config.Value,
                config.ValueKind,
                config.Description,
                config.UpdatedBy,
                config.UpdatedAt);
        }

        _items = next;
        IsLoaded = true;
    }

    /// <summary>
    /// Thử lấy item theo key.
    /// </summary>
    public bool TryGet(string key, out SnapshotItem item)
    {
        return _items.TryGetValue(key, out item!);
    }

    /// <summary>
    /// Thử lấy giá trị text theo key.
    /// </summary>
    public bool TryGetValue(string key, out string value)
    {
        if (_items.TryGetValue(key, out var item))
        {
            value = item.Value;
            return true;
        }

        value = string.Empty;
        return false;
    }
}

/// <summary>
/// Bản ghi snapshot cho một key cấu hình.
/// </summary>
public sealed record SnapshotItem(
    string Key,
    string Value,
    string ValueKind,
    string? Description,
    Guid? UpdatedBy,
    DateTime UpdatedAt);
