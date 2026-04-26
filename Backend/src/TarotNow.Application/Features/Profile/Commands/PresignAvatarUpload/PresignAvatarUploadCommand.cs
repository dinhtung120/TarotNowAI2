using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;

/// <summary>
/// Command sinh presigned URL cho upload avatar trực tiếp lên R2.
/// </summary>
public sealed class PresignAvatarUploadCommand : IRequest<PresignedUploadResult>
{
    /// <summary>Định danh user upload avatar.</summary>
    public Guid UserId { get; set; }

    /// <summary>Content type client dự kiến upload.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Kích thước file đã nén phía FE (byte).</summary>
    public long SizeBytes { get; set; }
}

/// <summary>
/// Handler presign avatar upload.
/// </summary>
public sealed class PresignAvatarUploadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignAvatarUploadCommandHandlerRequestedDomainEvent>
{
    private readonly IR2UploadService _r2UploadService;
    private readonly IUploadSessionRepository _uploadSessionRepository;

    /// <summary>
    /// Khởi tạo handler presign avatar.
    /// </summary>
    public PresignAvatarUploadCommandHandlerRequestedDomainEventHandler(
        IR2UploadService r2UploadService,
        IUploadSessionRepository uploadSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _r2UploadService = r2UploadService;
        _uploadSessionRepository = uploadSessionRepository;
    }

    /// <inheritdoc />
    public async Task<PresignedUploadResult> Handle(PresignAvatarUploadCommand request, CancellationToken cancellationToken)
    {
        EnsureR2UploadEnabled();
        EnsureImagePayload(request.ContentType, request.SizeBytes);

        var nowUtc = DateTime.UtcNow;
        var expiresAtUtc = nowUtc.Add(MediaUploadConstants.PresignExpiry);
        var objectKey = BuildAvatarObjectKey(request.UserId);
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
                Scope = MediaUploadConstants.ScopeAvatar,
                ObjectKey = objectKey,
                PublicUrl = publicUrl,
                ContentType = MediaUploadConstants.RequiredImageMimeType,
                SizeBytes = request.SizeBytes,
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

    private static void EnsureImagePayload(string contentType, long sizeBytes)
    {
        if (!string.Equals(contentType, MediaUploadConstants.RequiredImageMimeType, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Avatar chỉ hỗ trợ content-type image/webp.");
        }

        if (sizeBytes <= 0 || sizeBytes > MediaUploadConstants.MaxImageUploadBytes)
        {
            throw new BadRequestException("Kích thước avatar không hợp lệ (tối đa 10MB).");
        }
    }

    private static string BuildAvatarObjectKey(Guid userId)
    {
        return $"avatars/{userId:N}-{Guid.NewGuid():N}.webp";
    }

    protected override async Task HandleDomainEventAsync(
        PresignAvatarUploadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
