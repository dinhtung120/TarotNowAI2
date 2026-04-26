using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

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
public sealed class PresignConversationMediaCommandExecutor : ICommandExecutionExecutor<PresignConversationMediaCommand, PresignedUploadResult>
{
    private static readonly HashSet<string> AllowedVoiceMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "audio/webm",
        "audio/ogg",
        "audio/opus",
        "audio/mp4",
        "audio/mpeg",
        "audio/wav"
    };

    private static readonly IReadOnlyDictionary<string, string> VoiceExtensionByMime = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["audio/webm"] = ".webm",
        ["audio/ogg"] = ".ogg",
        ["audio/opus"] = ".opus",
        ["audio/mp4"] = ".m4a",
        ["audio/mpeg"] = ".mp3",
        ["audio/wav"] = ".wav"
    };

    private readonly IConversationRepository _conversationRepository;
    private readonly IR2UploadService _r2UploadService;
    private readonly IUploadSessionRepository _uploadSessionRepository;

    /// <summary>
    /// Khởi tạo handler presign media chat.
    /// </summary>
    public PresignConversationMediaCommandExecutor(
        IConversationRepository conversationRepository,
        IR2UploadService r2UploadService,
        IUploadSessionRepository uploadSessionRepository)
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
        ValidateMediaPayload(normalizedKind, request.ContentType, request.SizeBytes, request.DurationMs);

        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        EnsureRequesterOwnsConversation(conversation, request.RequesterId);
        EnsureConversationAllowsUpload(conversation);

        var nowUtc = DateTime.UtcNow;
        var expiresAtUtc = nowUtc.Add(MediaUploadConstants.PresignExpiry);
        var objectKey = BuildChatObjectKey(request.ConversationId, request.RequesterId, normalizedKind, request.ContentType);
        var publicUrl = _r2UploadService.BuildPublicUrl(objectKey);
        var uploadToken = UploadTokenGenerator.Generate();

        var uploadUrl = await _r2UploadService.GeneratePresignedPutUrlAsync(
            objectKey,
            request.ContentType.Trim().ToLowerInvariant(),
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
                ContentType = request.ContentType.Trim().ToLowerInvariant(),
                SizeBytes = request.SizeBytes,
                ConversationId = request.ConversationId,
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

    private static string NormalizeAndValidateMediaKind(string mediaKind)
    {
        var normalized = mediaKind.Trim().ToLowerInvariant();
        if (!MediaUploadConstants.IsChatMediaKind(normalized))
        {
            throw new BadRequestException("MediaKind không hợp lệ. Chỉ nhận image|voice.");
        }

        return normalized;
    }

    private static void ValidateMediaPayload(string mediaKind, string contentType, long sizeBytes, int? durationMs)
    {
        var normalizedContentType = contentType.Trim().ToLowerInvariant();
        if (mediaKind == "image")
        {
            if (!string.Equals(normalizedContentType, MediaUploadConstants.RequiredImageMimeType, StringComparison.Ordinal))
            {
                throw new BadRequestException("Chat image chỉ hỗ trợ content-type image/webp.");
            }

            if (sizeBytes <= 0 || sizeBytes > MediaUploadConstants.MaxImageUploadBytes)
            {
                throw new BadRequestException("Kích thước chat image không hợp lệ (tối đa 10MB).");
            }

            return;
        }

        if (AllowedVoiceMimeTypes.Contains(normalizedContentType) == false)
        {
            throw new BadRequestException("Voice mime type chưa được hỗ trợ.");
        }

        if (sizeBytes <= 0 || sizeBytes > MediaUploadConstants.MaxVoiceUploadBytes)
        {
            throw new BadRequestException("Kích thước voice không hợp lệ (tối đa 5MB).");
        }

        if (durationMs is <= 0 or > 600_000)
        {
            throw new BadRequestException("Thời lượng voice phải trong khoảng 1ms đến 600000ms.");
        }
    }

    private static void EnsureRequesterOwnsConversation(ConversationDto conversation, Guid requesterId)
    {
        var requester = requesterId.ToString();
        if (!string.Equals(conversation.UserId, requester, StringComparison.Ordinal)
            && !string.Equals(conversation.ReaderId, requester, StringComparison.Ordinal))
        {
            throw new ForbiddenException("Bạn không thuộc cuộc trò chuyện này.");
        }
    }

    private static void EnsureConversationAllowsUpload(ConversationDto conversation)
    {
        if (ConversationStatus.IsTerminal(conversation.Status) || conversation.Status == ConversationStatus.Disputed)
        {
            throw new BadRequestException($"Không thể upload media ở trạng thái conversation '{conversation.Status}'.");
        }
    }

    private static string BuildChatObjectKey(string conversationId, Guid requesterId, string mediaKind, string contentType)
    {
        if (mediaKind == "image")
        {
            return $"chat/{conversationId}/images/{requesterId:N}-{Guid.NewGuid():N}.webp";
        }

        var ext = VoiceExtensionByMime.TryGetValue(contentType.Trim().ToLowerInvariant(), out var value)
            ? value
            : ".webm";
        return $"chat/{conversationId}/voices/{requesterId:N}-{Guid.NewGuid():N}{ext}";
    }
}
