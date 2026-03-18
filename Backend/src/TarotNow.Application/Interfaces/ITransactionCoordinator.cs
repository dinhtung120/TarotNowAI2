namespace TarotNow.Application.Interfaces;

/// <summary>
/// Điều phối transaction cấp ứng dụng để gom nhiều thao tác repository
/// vào cùng một transaction boundary.
/// </summary>
public interface ITransactionCoordinator
{
    Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
}
