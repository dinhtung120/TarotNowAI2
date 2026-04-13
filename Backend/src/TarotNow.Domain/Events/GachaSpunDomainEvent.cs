namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi người dùng quay gacha thành công.
/// </summary>
public sealed class GachaSpunDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng thực hiện quay.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã banner được quay.
    /// </summary>
    public string BannerCode { get; init; } = string.Empty;

    /// <summary>
    /// Số lượt quay trong request.
    /// </summary>
    public int SpinCount { get; init; }

    /// <summary>
    /// Cờ cho biết batch có trigger pity.
    /// </summary>
    public bool WasPityTriggered { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
