namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract lưu trạng thái hiệu ứng may mắn phát sinh từ inventory.
/// </summary>
public interface IInventoryLuckEffectRepository
{
    /// <summary>
    /// Áp hoặc gia hạn hiệu ứng may mắn cho người dùng.
    /// </summary>
    Task ApplyLuckAsync(
        Guid userId,
        int luckValue,
        string sourceItemCode,
        TimeSpan duration,
        CancellationToken cancellationToken = default);
}
