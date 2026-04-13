using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Services;

/// <summary>
/// Dịch vụ gắn/reconcile community asset theo nội dung markdown.
/// </summary>
public sealed class CommunityMediaAttachmentService : ICommunityMediaAttachmentService
{
    private readonly ICommunityMediaAssetRepository _assetRepository;
    private readonly IR2UploadService _r2UploadService;

    /// <summary>
    /// Khởi tạo dịch vụ attachment.
    /// </summary>
    public CommunityMediaAttachmentService(
        ICommunityMediaAssetRepository assetRepository,
        IR2UploadService r2UploadService)
    {
        _assetRepository = assetRepository;
        _r2UploadService = r2UploadService;
    }

    /// <inheritdoc />
    public async Task AttachForNewEntityAsync(
        Guid ownerUserId,
        string contextType,
        string? contextDraftId,
        string contextEntityId,
        string markdownContent,
        CancellationToken cancellationToken = default)
    {
        EnsureContextTypeValid(contextType);
        var objectKeys = ExtractOwnedCommunityObjectKeys(markdownContent);
        if (objectKeys.Count == 0)
        {
            return;
        }

        var nowUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(contextDraftId))
        {
            await _assetRepository.AttachDraftAssetsAsync(
                ownerUserId,
                contextType,
                contextDraftId.Trim(),
                contextEntityId,
                objectKeys,
                nowUtc,
                cancellationToken);
        }

        await _assetRepository.AttachByObjectKeysAsync(
            ownerUserId,
            contextType,
            contextEntityId,
            objectKeys,
            nowUtc,
            cancellationToken);

        await _assetRepository.ReconcileAttachedAssetsAsync(
            ownerUserId,
            contextType,
            contextEntityId,
            objectKeys,
            nowUtc,
            nowUtc.Add(MediaUploadConstants.CommunityOrphanTtl),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task ReconcileForExistingEntityAsync(
        Guid ownerUserId,
        string contextType,
        string contextEntityId,
        string markdownContent,
        CancellationToken cancellationToken = default)
    {
        EnsureContextTypeValid(contextType);
        var objectKeys = ExtractOwnedCommunityObjectKeys(markdownContent);
        var nowUtc = DateTime.UtcNow;

        if (objectKeys.Count > 0)
        {
            await _assetRepository.AttachByObjectKeysAsync(
                ownerUserId,
                contextType,
                contextEntityId,
                objectKeys,
                nowUtc,
                cancellationToken);
        }

        await _assetRepository.ReconcileAttachedAssetsAsync(
            ownerUserId,
            contextType,
            contextEntityId,
            objectKeys,
            nowUtc,
            nowUtc.Add(MediaUploadConstants.CommunityOrphanTtl),
            cancellationToken);
    }

    private IReadOnlyCollection<string> ExtractOwnedCommunityObjectKeys(string markdownContent)
    {
        var urls = MarkdownImageLinkExtractor.ExtractUrls(markdownContent);
        if (urls.Count == 0)
        {
            return Array.Empty<string>();
        }

        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var url in urls)
        {
            if (_r2UploadService.TryExtractObjectKey(url, out var key) == false)
            {
                continue;
            }

            if (key.StartsWith("community/", StringComparison.OrdinalIgnoreCase))
            {
                keys.Add(key);
            }
        }

        return keys.Count == 0 ? Array.Empty<string>() : keys.ToArray();
    }

    private static void EnsureContextTypeValid(string contextType)
    {
        if (!MediaUploadConstants.IsCommunityContextType(contextType))
        {
            throw new BadRequestException("Community context type không hợp lệ.");
        }
    }
}
