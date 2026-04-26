using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Community.Commands.PresignCommunityImage;

/// <summary>
/// Command sinh presigned URL upload ảnh community (post/comment).
/// </summary>
public sealed class PresignCommunityImageCommand : IRequest<PresignedUploadResult>
{
    /// <summary>User gửi yêu cầu upload.</summary>
    public Guid UserId { get; set; }

    /// <summary>Context type post/comment.</summary>
    public string ContextType { get; set; } = string.Empty;

    /// <summary>Draft id do FE cấp để map trước khi tạo entity thật.</summary>
    public string ContextDraftId { get; set; } = string.Empty;

    /// <summary>Content type file nén FE.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Kích thước file nén FE (byte).</summary>
    public long SizeBytes { get; set; }
}

/// <summary>
/// Handler presign ảnh community.
/// </summary>
public sealed class PresignCommunityImageCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignCommunityImageCommandHandlerRequestedDomainEvent>
{
    private readonly IR2UploadService _r2UploadService;
    private readonly IUploadSessionRepository _uploadSessionRepository;

    /// <summary>
    /// Khởi tạo handler presign ảnh community.
    /// </summary>
    public PresignCommunityImageCommandHandlerRequestedDomainEventHandler(
        IR2UploadService r2UploadService,
        IUploadSessionRepository uploadSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _r2UploadService = r2UploadService;
        _uploadSessionRepository = uploadSessionRepository;
    }

    /// <inheritdoc />
    public async Task<PresignedUploadResult> Handle(PresignCommunityImageCommand request, CancellationToken cancellationToken)
    {
        EnsureR2UploadEnabled();
        var contextType = NormalizeAndValidateContextType(request.ContextType);
        EnsureImagePayload(request.ContentType, request.SizeBytes);

        var nowUtc = DateTime.UtcNow;
        var expiresAtUtc = nowUtc.Add(MediaUploadConstants.PresignExpiry);
        var objectKey = BuildCommunityObjectKey(contextType, request.UserId);
        var publicUrl = _r2UploadService.BuildPublicUrl(objectKey);
        var uploadToken = UploadTokenGenerator.Generate();

        var uploadUrl = await _r2UploadService.GeneratePresignedPutUrlAsync(
            objectKey,
            MediaUploadConstants.RequiredImageMimeType,
            expiresAtUtc,
            cancellationToken);

        await _uploadSessionRepository.CreateAsync(
            new UploadSessionRecord
            {
                UploadToken = uploadToken,
                OwnerUserId = request.UserId,
                Scope = MediaUploadConstants.ScopeCommunityImage,
                ObjectKey = objectKey,
                PublicUrl = publicUrl,
                ContentType = MediaUploadConstants.RequiredImageMimeType,
                SizeBytes = request.SizeBytes,
                ContextType = contextType,
                ContextDraftId = request.ContextDraftId.Trim(),
                CreatedAtUtc = nowUtc,
                ExpiresAtUtc = expiresAtUtc,
            },
            cancellationToken);

        return new PresignedUploadResult(uploadUrl, objectKey, publicUrl, uploadToken, expiresAtUtc);
    }

    private void EnsureR2UploadEnabled()
    {
        if (_r2UploadService.IsEnabled == false)
        {
            throw new BadRequestException("R2 upload chưa được cấu hình. Vui lòng kiểm tra ObjectStorage:R2.");
        }
    }

    private static string NormalizeAndValidateContextType(string contextType)
    {
        var normalized = MediaUploadConstants.NormalizeContextType(contextType);
        if (MediaUploadConstants.IsCommunityContextType(normalized) == false)
        {
            throw new BadRequestException("Community context type không hợp lệ. Chỉ nhận post/comment.");
        }

        return normalized;
    }

    private static void EnsureImagePayload(string contentType, long sizeBytes)
    {
        if (!string.Equals(contentType, MediaUploadConstants.RequiredImageMimeType, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Community image chỉ hỗ trợ content-type image/webp.");
        }

        if (sizeBytes <= 0 || sizeBytes > MediaUploadConstants.MaxImageUploadBytes)
        {
            throw new BadRequestException("Kích thước ảnh community không hợp lệ (tối đa 10MB).");
        }
    }

    private static string BuildCommunityObjectKey(string contextType, Guid userId)
    {
        return $"community/{contextType}/{userId:N}-{Guid.NewGuid():N}.webp";
    }

    protected override async Task HandleDomainEventAsync(
        PresignCommunityImageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
