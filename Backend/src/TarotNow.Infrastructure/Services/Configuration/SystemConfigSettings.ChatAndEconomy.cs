namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Danh sách SLA giờ được phép cho conversation.
    public IReadOnlyList<int> ChatAllowedSlaHours
    {
        get
        {
            var fallback = NormalizeDistinctPositiveOrderedList(
                _options.Chat.AllowedSlaHours,
                minInclusive: 1,
                maxInclusive: 168);
            var fallbackWithDefault = fallback.Count > 0 ? fallback : new[] { 6, 12, 24 };
            var fromConfig = NormalizeDistinctPositiveOrderedList(
                ReadIntArray("chat.allowed_sla_hours"),
                minInclusive: 1,
                maxInclusive: 168);
            return fromConfig.Count > 0 ? fromConfig : fallbackWithDefault;
        }
    }

    // SLA mặc định cho conversation.
    public int ChatDefaultSlaHours
    {
        get
        {
            var allowed = ChatAllowedSlaHours;
            var configured = ResolvePositiveInt(
                ReadInt(["chat.default_sla_hours"], _options.Chat.DefaultSlaHours),
                fallback: 12);
            if (allowed.Contains(configured))
            {
                return configured;
            }

            return allowed.Contains(12) ? 12 : allowed[0];
        }
    }

    // Số conversation active tối đa mỗi user.
    public int ChatMaxActiveConversationsPerUser => ResolvePositiveInt(
        ReadInt(["chat.max_active_conversations_per_user"], _options.Chat.MaxActiveConversationsPerUser),
        fallback: 5);

    // Tỷ giá quy đổi 1 Diamond sang VND.
    public long EconomyVndPerDiamond => ResolvePositiveLong(
        ReadLong(["economy.vnd_per_diamond"], _options.Economy.VndPerDiamond),
        fallback: 100);
}
