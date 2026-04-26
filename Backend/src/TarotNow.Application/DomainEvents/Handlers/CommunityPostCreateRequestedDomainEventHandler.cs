using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler tạo post community theo event request.
/// </summary>
public sealed class CommunityPostCreateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CommunityPostCreateRequestedDomainEvent>
{
    private readonly ICommunityPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler tạo post community.
    /// </summary>
    public CommunityPostCreateRequestedDomainEventHandler(
        ICommunityPostRepository postRepository,
        IUserRepository userRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        CommunityPostCreateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(domainEvent.AuthorId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User không tồn tại.");
        }

        var requiresMediaAttach = RequiresMediaAttach(domainEvent.ContextDraftId, domainEvent.Content);
        var createdPost = await CreatePostAsync(domainEvent, user, requiresMediaAttach, cancellationToken);

        HydrateResultPayload(domainEvent, createdPost);
        await PublishFollowupEventsAsync(domainEvent, createdPost, requiresMediaAttach, cancellationToken);
    }

    private Task<CommunityPostDto> CreatePostAsync(
        CommunityPostCreateRequestedDomainEvent domainEvent,
        TarotNow.Domain.Entities.User user,
        bool requiresMediaAttach,
        CancellationToken cancellationToken)
    {
        var mediaAttachStatus = requiresMediaAttach
            ? MediaUploadConstants.EntityMediaAttachStatusPending
            : MediaUploadConstants.EntityMediaAttachStatusNone;

        return _postRepository.CreateAsync(
            new CommunityPostDto
            {
                AuthorId = domainEvent.AuthorId.ToString(),
                AuthorDisplayName = user.DisplayName,
                AuthorAvatarUrl = user.AvatarUrl,
                Content = domainEvent.Content.Trim(),
                Visibility = domainEvent.Visibility,
                CreatedAt = DateTime.UtcNow,
                ReactionsCount = new Dictionary<string, int>(),
                TotalReactions = 0,
                CommentsCount = 0,
                IsDeleted = false,
                MediaAttachStatus = mediaAttachStatus,
                MediaAttachLastError = null
            },
            cancellationToken);
    }

    private static void HydrateResultPayload(
        CommunityPostCreateRequestedDomainEvent domainEvent,
        CommunityPostDto createdPost)
    {
        domainEvent.CreatedPostId = createdPost.Id;
        domainEvent.AuthorDisplayName = createdPost.AuthorDisplayName;
        domainEvent.AuthorAvatarUrl = createdPost.AuthorAvatarUrl;
        domainEvent.CreatedAtUtc = createdPost.CreatedAt;
        domainEvent.MediaAttachStatus = createdPost.MediaAttachStatus;
    }

    private async Task PublishFollowupEventsAsync(
        CommunityPostCreateRequestedDomainEvent domainEvent,
        CommunityPostDto createdPost,
        bool requiresMediaAttach,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new CommunityPostCreatedDomainEvent
            {
                AuthorId = domainEvent.AuthorId,
                PostId = createdPost.Id
            },
            cancellationToken);

        if (!requiresMediaAttach)
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(
            new CommunityMediaAttachRequestedDomainEvent
            {
                OwnerUserId = domainEvent.AuthorId,
                ContextType = MediaUploadConstants.ContextPost,
                ContextDraftId = domainEvent.ContextDraftId,
                ContextEntityId = createdPost.Id,
                MarkdownContent = createdPost.Content
            },
            cancellationToken);
    }

    private static bool RequiresMediaAttach(string? contextDraftId, string markdownContent)
    {
        if (!string.IsNullOrWhiteSpace(contextDraftId))
        {
            return true;
        }

        return MarkdownImageLinkExtractor.ExtractUrls(markdownContent).Count > 0;
    }
}
