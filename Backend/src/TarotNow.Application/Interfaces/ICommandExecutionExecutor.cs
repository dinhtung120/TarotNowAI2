namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract cho executor chứa logic command legacy đã tách khỏi command handler event-only.
/// </summary>
public interface ICommandExecutionExecutor<TCommand, TResponse>
{
    /// <summary>
    /// Thực thi command và trả về response tương ứng.
    /// </summary>
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}
