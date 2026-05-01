using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;

/// <summary>
/// Command sinh presigned URL upload media chat (image/voice).
/// </summary>
public sealed class PresignConversationMediaCommand : IRequest<PresignedUploadResult>
{
    /// <summary>Định danh conversation.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>User gửi yêu cầu presign.</summary>
    public Guid RequesterId { get; set; }

    /// <summary>Loại media cần upload: image|voice.</summary>
    public string MediaKind { get; set; } = string.Empty;

    /// <summary>Content type file media.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Kích thước media (byte).</summary>
    public long SizeBytes { get; set; }

    /// <summary>Thời lượng media (ms), bắt buộc cho voice.</summary>
    public int? DurationMs { get; set; }
}

/// <summary>
/// Handler presign conversation media.
/// </summary>
public sealed partial class PresignConversationMediaCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignConversationMediaCommandHandlerRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IR2UploadService _r2UploadService;
    private readonly IUploadSessionRepository _uploadSessionRepository;

    /// <summary>
    /// Khởi tạo handler presign media chat.
    /// </summary>
    public PresignConversationMediaCommandHandlerRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IR2UploadService r2UploadService,
        IUploadSessionRepository uploadSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _r2UploadService = r2UploadService;
        _uploadSessionRepository = uploadSessionRepository;
    }

    /// <inheritdoc />
    public async Task<PresignedUploadResult> Handle(PresignConversationMediaCommand request, CancellationToken cancellationToken)
    {
        EnsureR2UploadEnabled();
        var normalizedKind = NormalizeAndValidateMediaKind(request.MediaKind);
        var normalizedContentType = NormalizeContentType(request.ContentType);
        ValidateMediaPayload(normalizedKind, normalizedContentType, request.SizeBytes, request.DurationMs);

        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        EnsureRequesterOwnsConversation(conversation, request.RequesterId);
        EnsureConversationAllowsUpload(conversation);

        var nowUtc = DateTime.UtcNow;
        var expiresAtUtc = nowUtc.Add(MediaUploadConstants.PresignExpiry);
        var objectKey = BuildChatObjectKey(request.ConversationId, request.RequesterId, normalizedKind, normalizedContentType);
        var publicUrl = _r2UploadService.BuildPublicUrl(objectKey);
        var uploadToken = UploadTokenGenerator.Generate();

        var uploadUrl = await _r2UploadService.GeneratePresignedPutUrlAsync(
            objectKey,
            normalizedContentType,
            expiresAtUtc,
            cancellationToken);

        await _uploadSessionRepository.CreateAsync(
            new UploadSessionRecord
            {
                UploadToken = uploadToken,
                OwnerUserId = request.RequesterId,
                Scope = normalizedKind == "image" ? MediaUploadConstants.ScopeChatImage : MediaUploadConstants.ScopeChatVoice,
                ObjectKey = objectKey,
                PublicUrl = publicUrl,
                ContentType = normalizedContentType,
                SizeBytes = request.SizeBytes,
                ConversationId = request.ConversationId,
                CreatedAtUtc = nowUtc,
                ExpiresAtUtc = expiresAtUtc,
            },
            cancellationToken);

        return new PresignedUploadResult(uploadUrl, objectKey, publicUrl, uploadToken, expiresAtUtc);
    }

    protected override async Task HandleDomainEventAsync(
        PresignConversationMediaCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
