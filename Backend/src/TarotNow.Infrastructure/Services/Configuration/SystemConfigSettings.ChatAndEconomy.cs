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
                fallback: Math.Max(1, _options.Chat.DefaultSlaHours));
            if (allowed.Contains(configured))
            {
                return configured;
            }

            return allowed.Contains(_options.Chat.DefaultSlaHours)
                ? _options.Chat.DefaultSlaHours
                : allowed[0];
        }
    }

    // Số conversation active tối đa mỗi user.
    public int ChatMaxActiveConversationsPerUser => ResolvePositiveInt(
        ReadInt(["chat.max_active_conversations_per_user"], _options.Chat.MaxActiveConversationsPerUser),
        fallback: Math.Max(1, _options.Chat.MaxActiveConversationsPerUser));

    // Amount mặc định cho payment offer.
    public long ChatPaymentOfferDefaultAmount => ResolvePositiveLong(
        ReadLong(["chat.payment_offer.default_amount"], _options.Chat.PaymentOffer.DefaultAmount),
        fallback: Math.Max(1, _options.Chat.PaymentOffer.DefaultAmount));

    // Độ dài tối đa note payment offer.
    public int ChatPaymentOfferMaxNoteLength => ResolvePositiveInt(
        ReadInt(["chat.payment_offer.max_note_length"], _options.Chat.PaymentOffer.MaxNoteLength),
        fallback: Math.Max(1, _options.Chat.PaymentOffer.MaxNoteLength));

    // Page size mặc định lịch sử chat.
    public int ChatHistoryPageSize => ClampInt(
        ReadInt(["chat.history.page_size"], _options.Chat.History.PageSize),
        min: 1,
        max: 200);

    // Page size mặc định query participants.
    public int ChatParticipantsDefaultPageSize => ClampInt(
        ReadInt(["chat.participants.default_page_size"], _options.Chat.Participants.DefaultPageSize),
        min: 1,
        max: 500);

    // Page size tối đa query participants.
    public int ChatParticipantsMaxPageSize => ClampInt(
        ReadInt(["chat.participants.max_page_size"], _options.Chat.Participants.MaxPageSize),
        min: 1,
        max: 1000);

    // Tỷ giá quy đổi 1 Diamond sang VND.
    public long EconomyVndPerDiamond => ResolvePositiveLong(
        ReadLong(["economy.vnd_per_diamond"], _options.Economy.VndPerDiamond),
        fallback: Math.Max(1, _options.Economy.VndPerDiamond));
}
