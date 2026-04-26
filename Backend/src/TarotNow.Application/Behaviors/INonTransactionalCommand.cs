namespace TarotNow.Application.Behaviors;

/// <summary>
/// Marker cho command cần tự điều phối transaction cục bộ thay vì bọc toàn command pipeline.
/// </summary>
public interface INonTransactionalCommand
{
}
