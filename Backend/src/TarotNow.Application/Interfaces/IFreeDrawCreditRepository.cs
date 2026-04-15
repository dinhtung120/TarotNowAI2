namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract quản lý credit free draw phát sinh từ inventory.
/// </summary>
public interface IFreeDrawCreditRepository
{
    /// <summary>
    /// Cộng thêm credit free draw cho người dùng.
    /// </summary>
    Task AddCreditsAsync(Guid userId, int creditCount, CancellationToken cancellationToken = default);
}
