namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract quản lý credit free draw phát sinh từ inventory.
/// </summary>
public interface IFreeDrawCreditRepository
{
    /// <summary>
    /// Cộng thêm credit free draw cho người dùng theo loại spread.
    /// </summary>
    Task AddCreditsAsync(
        Guid userId,
        int spreadCardCount,
        int creditCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tiêu thụ một credit free draw theo loại spread.
    /// </summary>
    Task<bool> TryConsumeAsync(Guid userId, int spreadCardCount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Truy vấn tổng quan quota free draw 3/5/10 lá.
    /// </summary>
    Task<FreeDrawCreditSummary> GetSummaryAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Tổng quan quota free draw theo spread.
/// </summary>
public sealed class FreeDrawCreditSummary
{
    /// <summary>
    /// Lượt miễn phí spread 3 lá.
    /// </summary>
    public int Spread3Count { get; init; }

    /// <summary>
    /// Lượt miễn phí spread 5 lá.
    /// </summary>
    public int Spread5Count { get; init; }

    /// <summary>
    /// Lượt miễn phí spread 10 lá.
    /// </summary>
    public int Spread10Count { get; init; }
}
