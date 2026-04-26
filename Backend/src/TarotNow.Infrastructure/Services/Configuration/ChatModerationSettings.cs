using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Adapter expose policy moderation từ options hạ tầng sang contract Application.
/// </summary>
public sealed class ChatModerationSettings : IChatModerationSettings
{
    private readonly IOptionsMonitor<ChatModerationOptions> _options;

    public ChatModerationSettings(IOptionsMonitor<ChatModerationOptions> options)
    {
        _options = options;
    }

    public bool Enabled => _options.CurrentValue.Enabled;

    public IReadOnlyCollection<string> Keywords => _options.CurrentValue.Keywords;
}
