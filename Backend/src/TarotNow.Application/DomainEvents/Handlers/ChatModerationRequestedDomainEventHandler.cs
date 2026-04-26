using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using Microsoft.Extensions.Logging;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler kiểm duyệt trực tiếp sau khi message được lưu thành công.
/// </summary>
public sealed class ChatModerationRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ChatModerationRequestedDomainEvent>
{
    private readonly IReportRepository _reportRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IChatModerationSettings _moderationSettings;
    private readonly ILogger<ChatModerationRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler moderation requested.
    /// </summary>
    public ChatModerationRequestedDomainEventHandler(
        IReportRepository reportRepository,
        IChatMessageRepository chatMessageRepository,
        IChatModerationSettings moderationSettings,
        ILogger<ChatModerationRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _reportRepository = reportRepository;
        _chatMessageRepository = chatMessageRepository;
        _moderationSettings = moderationSettings;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ChatModerationRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        if (_moderationSettings.Enabled == false)
        {
            // Fail để outbox retry lại khi moderation được bật lại, tránh drop payload.
            throw new InvalidOperationException("Chat moderation is disabled; event will be retried.");
        }

        if (!ShouldModerate(domainEvent.MessageType))
        {
            return;
        }

        var matchedKeyword = MatchKeyword(domainEvent.Content, _moderationSettings.Keywords);
        if (matchedKeyword is null)
        {
            return;
        }

        await _chatMessageRepository.UpdateFlagAsync(domainEvent.MessageId, true, cancellationToken);

        var payload = new ChatModerationPayload
        {
            MessageId = domainEvent.MessageId,
            ConversationId = domainEvent.ConversationId,
            SenderId = domainEvent.SenderId,
            Type = domainEvent.MessageType,
            Content = domainEvent.Content,
            CreatedAt = domainEvent.CreatedAtUtc
        };
        await _reportRepository.AddAsync(BuildAutoModerationReport(payload, matchedKeyword), cancellationToken);

        _logger.LogWarning(
            "Auto moderation flagged message. MessageId={MessageId}, Keyword={Keyword}",
            domainEvent.MessageId,
            matchedKeyword);
    }

    private static bool ShouldModerate(string type)
    {
        return string.Equals(type, "text", StringComparison.OrdinalIgnoreCase)
            || string.Equals(type, "system", StringComparison.OrdinalIgnoreCase);
    }

    private static string? MatchKeyword(string content, IReadOnlyCollection<string> keywords)
    {
        if (string.IsNullOrWhiteSpace(content) || keywords.Count == 0)
        {
            return null;
        }

        foreach (var keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                continue;
            }

            if (content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                return keyword;
            }
        }

        return null;
    }

    private static ReportDto BuildAutoModerationReport(ChatModerationPayload payload, string matchedKeyword)
    {
        return new ReportDto
        {
            ReporterId = Guid.Empty.ToString(),
            TargetType = "message",
            TargetId = payload.MessageId,
            ConversationRef = payload.ConversationId,
            Reason = $"Auto moderation flag: keyword '{matchedKeyword}' detected in chat message.",
            Status = "pending",
            CreatedAt = DateTime.UtcNow,
            AdminNote = $"sender={payload.SenderId}, created_at={payload.CreatedAt:O}"
        };
    }
}
