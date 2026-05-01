using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;

public sealed partial class PresignConversationMediaCommandHandlerRequestedDomainEventHandler
{
    private static readonly HashSet<string> AllowedImageMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/webp",
        "image/jpeg",
        "image/png",
        "image/avif",
        "image/gif"
    };

    private static readonly HashSet<string> AllowedVoiceMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "audio/webm",
        "audio/ogg",
        "audio/opus",
        "audio/mp4",
        "audio/m4a",
        "audio/x-m4a",
        "audio/mpeg",
        "audio/wav"
    };

    private static readonly IReadOnlyDictionary<string, string> VoiceExtensionByMime =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["audio/webm"] = ".webm",
        ["audio/ogg"] = ".ogg",
        ["audio/opus"] = ".opus",
        ["audio/mp4"] = ".m4a",
        ["audio/m4a"] = ".m4a",
        ["audio/x-m4a"] = ".m4a",
        ["audio/mpeg"] = ".mp3",
        ["audio/wav"] = ".wav"
    };

    private static readonly IReadOnlyDictionary<string, string> ImageExtensionByMime =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["image/webp"] = ".webp",
        ["image/jpeg"] = ".jpg",
        ["image/png"] = ".png",
        ["image/avif"] = ".avif",
        ["image/gif"] = ".gif"
    };

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
        var normalizedContentType = NormalizeContentType(contentType);

        if (mediaKind == "image")
        {
            if (AllowedImageMimeTypes.Contains(normalizedContentType) == false)
            {
                throw new BadRequestException("Chat image chỉ hỗ trợ webp/jpeg/png/avif/gif.");
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

    private static string NormalizeContentType(string contentType)
    {
        var normalized = contentType.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return normalized;
        }

        var semicolonIndex = normalized.IndexOf(';');
        if (semicolonIndex >= 0)
        {
            normalized = normalized[..semicolonIndex];
        }

        return normalized.Trim();
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
            var normalizedContentType = contentType.Trim().ToLowerInvariant();
            var extension = ImageExtensionByMime.TryGetValue(normalizedContentType, out var imageExt)
                ? imageExt
                : ".webp";
            return $"chat/{conversationId}/images/{requesterId:N}-{Guid.NewGuid():N}{extension}";
        }

        var ext = VoiceExtensionByMime.TryGetValue(contentType.Trim().ToLowerInvariant(), out var value)
            ? value
            : ".webm";
        return $"chat/{conversationId}/voices/{requesterId:N}-{Guid.NewGuid():N}{ext}";
    }
}
