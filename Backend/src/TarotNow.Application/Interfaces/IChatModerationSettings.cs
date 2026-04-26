namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract đọc policy moderation runtime mà không phụ thuộc hạ tầng cụ thể.
/// </summary>
public interface IChatModerationSettings
{
    bool Enabled { get; }

    IReadOnlyCollection<string> Keywords { get; }
}
