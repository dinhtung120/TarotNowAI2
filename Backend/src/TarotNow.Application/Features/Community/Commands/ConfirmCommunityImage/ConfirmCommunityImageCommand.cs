using MediatR;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;

/// <summary>
/// Command xác nhận ảnh community đã upload thành công lên R2.
/// </summary>
public sealed class ConfirmCommunityImageCommand : IRequest<ConfirmCommunityImageResult>
{
    /// <summary>User xác nhận upload.</summary>
    public Guid UserId { get; set; }

    /// <summary>Context type post/comment.</summary>
    public string ContextType { get; set; } = string.Empty;

    /// <summary>Draft id trước khi tạo entity thật.</summary>
    public string ContextDraftId { get; set; } = string.Empty;

    /// <summary>Object key vừa upload.</summary>
    public string ObjectKey { get; set; } = string.Empty;

    /// <summary>Public URL vừa upload.</summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>Upload token one-time.</summary>
    public string UploadToken { get; set; } = string.Empty;
}

/// <summary>
/// Kết quả confirm community image.
/// </summary>
public sealed record ConfirmCommunityImageResult(string ObjectKey, string PublicUrl, string ContextType, string ContextDraftId);

/// <summary>
/// Handler xác nhận community image upload.
/// </summary>
public sealed class ConfirmCommunityImageCommandHandler : IRequestHandler<ConfirmCommunityImageCommand, ConfirmCommunityImageResult>
{
    private readonly IUploadSessionRepository _uploadSessionRepository;
    private readonly ICommunityMediaAssetRepository _communityMediaAssetRepository;
    private readonly IR2UploadService _r2UploadService;

    /// <summary>
    /// Khởi tạo handler confirm community image.
    /// </summary>
    public ConfirmCommunityImageCommandHandler(
        IUploadSessionRepository uploadSessionRepository,
        ICommunityMediaAssetRepository communityMediaAssetRepository,
        IR2UploadService r2UploadService)
    {
        _uploadSessionRepository = uploadSessionRepository;
        _communityMediaAssetRepository = communityMediaAssetRepository;
        _r2UploadService = r2UploadService;
    }

    /// <inheritdoc />
    public async Task<ConfirmCommunityImageResult> Handle(ConfirmCommunityImageCommand request, CancellationToken cancellationToken)
    {
        EnsureR2UploadEnabled();
        var contextType = NormalizeAndValidateContextType(request.ContextType);
        var session = await _uploadSessionRepository.GetByTokenAsync(request.UploadToken, cancellationToken)
            ?? throw new BadRequestException("Upload token không tồn tại.");

        ValidateUploadSession(session, request, contextType);
        var consumed = await _uploadSessionRepository.ConsumeAsync(request.UploadToken, DateTime.UtcNow, cancellationToken);
        if (!consumed)
        {
            throw new BadRequestException("Upload token đã được dùng hoặc đã hết hạn.");
        }

        var nowUtc = DateTime.UtcNow;
        await _communityMediaAssetRepository.UpsertUploadedAsync(
            new CommunityMediaAssetRecord
            {
                ObjectKey = request.ObjectKey,
                PublicUrl = request.PublicUrl,
                OwnerUserId = request.UserId,
                ContextType = contextType,
                ContextDraftId = request.ContextDraftId.Trim(),
                Status = MediaUploadConstants.AssetStatusUploaded,
                CreatedAtUtc = nowUtc,
                UpdatedAtUtc = nowUtc,
                ExpiresAtUtc = nowUtc.Add(MediaUploadConstants.CommunityOrphanTtl),
            },
            cancellationToken);

        return new ConfirmCommunityImageResult(request.ObjectKey, request.PublicUrl, contextType, request.ContextDraftId.Trim());
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

    private static void ValidateUploadSession(UploadSessionRecord session, ConfirmCommunityImageCommand request, string contextType)
    {
        if (session.OwnerUserId != request.UserId)
        {
            throw new BadRequestException("Upload token không thuộc user hiện tại.");
        }

        if (!string.Equals(session.Scope, MediaUploadConstants.ScopeCommunityImage, StringComparison.Ordinal))
        {
            throw new BadRequestException("Upload token scope không hợp lệ cho community image.");
        }

        if (session.ConsumedAtUtc.HasValue || session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new BadRequestException("Upload token đã hết hạn hoặc đã consume.");
        }

        if (!string.Equals(session.ObjectKey, request.ObjectKey, StringComparison.Ordinal))
        {
            throw new BadRequestException("Object key không khớp với upload token.");
        }

        if (!string.Equals(session.PublicUrl, request.PublicUrl, StringComparison.Ordinal))
        {
            throw new BadRequestException("Public URL không khớp với upload token.");
        }

        if (!string.Equals(session.ContextType, contextType, StringComparison.Ordinal))
        {
            throw new BadRequestException("ContextType không khớp với upload token.");
        }

        if (!string.Equals(session.ContextDraftId, request.ContextDraftId.Trim(), StringComparison.Ordinal))
        {
            throw new BadRequestException("ContextDraftId không khớp với upload token.");
        }
    }
}
