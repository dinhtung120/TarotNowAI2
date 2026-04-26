using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler attach media community bất đồng bộ qua outbox.
/// </summary>
public sealed class CommunityMediaAttachRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CommunityMediaAttachRequestedDomainEvent>
{
    private const int LastErrorMaxLength = 500;

    private readonly ICommunityMediaAttachmentService _communityMediaAttachmentService;
    private readonly ICommunityPostRepository _postRepository;
    private readonly ICommunityCommentRepository _commentRepository;
    private readonly ILogger<CommunityMediaAttachRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler attach media community.
    /// </summary>
    public CommunityMediaAttachRequestedDomainEventHandler(
        ICommunityMediaAttachmentService communityMediaAttachmentService,
        ICommunityPostRepository postRepository,
        ICommunityCommentRepository commentRepository,
        ILogger<CommunityMediaAttachRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _communityMediaAttachmentService = communityMediaAttachmentService;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        CommunityMediaAttachRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _communityMediaAttachmentService.AttachForNewEntityAsync(
                domainEvent.OwnerUserId,
                domainEvent.ContextType,
                domainEvent.ContextDraftId,
                domainEvent.ContextEntityId,
                domainEvent.MarkdownContent,
                cancellationToken);

            await UpdateEntityStatusAsync(
                domainEvent.ContextType,
                domainEvent.ContextEntityId,
                MediaUploadConstants.EntityMediaAttachStatusCompleted,
                null,
                cancellationToken);
        }
        catch (Exception exception)
        {
            var safeMessage = TrimLastError(exception.Message);
            await UpdateEntityStatusAsync(
                domainEvent.ContextType,
                domainEvent.ContextEntityId,
                MediaUploadConstants.EntityMediaAttachStatusFailed,
                safeMessage,
                cancellationToken);

            _logger.LogWarning(
                exception,
                "Community media attach failed for ContextType={ContextType}, ContextEntityId={ContextEntityId}.",
                domainEvent.ContextType,
                domainEvent.ContextEntityId);
            throw;
        }
    }

    private async Task UpdateEntityStatusAsync(
        string contextType,
        string contextEntityId,
        string status,
        string? lastError,
        CancellationToken cancellationToken)
    {
        var normalizedType = MediaUploadConstants.NormalizeContextType(contextType);
        if (string.Equals(normalizedType, MediaUploadConstants.ContextPost, StringComparison.Ordinal))
        {
            await _postRepository.UpdateMediaAttachStatusAsync(contextEntityId, status, lastError, cancellationToken);
            return;
        }

        if (string.Equals(normalizedType, MediaUploadConstants.ContextComment, StringComparison.Ordinal))
        {
            await _commentRepository.UpdateMediaAttachStatusAsync(contextEntityId, status, lastError, cancellationToken);
            return;
        }

        throw new BadRequestException("Community context type không hợp lệ.");
    }

    private static string TrimLastError(string message)
    {
        var normalized = string.IsNullOrWhiteSpace(message) ? "community_media_attach_failed" : message.Trim();
        return normalized.Length <= LastErrorMaxLength ? normalized : normalized[..LastErrorMaxLength];
    }
}
