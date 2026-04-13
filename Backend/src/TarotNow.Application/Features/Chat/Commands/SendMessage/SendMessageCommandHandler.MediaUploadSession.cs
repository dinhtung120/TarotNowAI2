using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Validate và consume upload token one-time cho message media.
    /// Luồng xử lý: kiểm tra token/object key/session scope, consume token và chuẩn hóa content lưu DB.
    /// </summary>
    private async Task ValidateAndConsumeMediaUploadSessionAsync(
        SendMessageCommand request,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        if (IsMediaType(request.Type) == false)
        {
            return;
        }

        var payload = request.MediaPayload ?? throw new BadRequestException("Thiếu media payload.");
        if (string.IsNullOrWhiteSpace(payload.ObjectKey))
        {
            throw new BadRequestException("Media payload thiếu objectKey.");
        }

        if (string.IsNullOrWhiteSpace(payload.UploadToken))
        {
            throw new BadRequestException("Media payload thiếu uploadToken.");
        }

        var session = await _uploadSessionRepository.GetByTokenAsync(payload.UploadToken, cancellationToken)
            ?? throw new BadRequestException("Upload token không tồn tại.");

        ValidateMediaUploadSession(session, request, conversation, payload);
        var consumed = await _uploadSessionRepository.ConsumeAsync(payload.UploadToken, DateTime.UtcNow, cancellationToken);
        if (!consumed)
        {
            throw new BadRequestException("Upload token đã được dùng hoặc đã hết hạn.");
        }

        payload.UploadToken = null;
        payload.ProcessingStatus = "uploaded";
        request.Content = string.Equals(request.Type, "voice", StringComparison.OrdinalIgnoreCase)
            ? "[voice]"
            : "[image]";
    }

    private static void ValidateMediaUploadSession(
        UploadSessionRecord session,
        SendMessageCommand request,
        ConversationDto conversation,
        MediaPayloadDto payload)
    {
        if (session.OwnerUserId != request.SenderId)
        {
            throw new BadRequestException("Upload token không thuộc người gửi tin nhắn.");
        }

        var expectedScope = ResolveExpectedMediaScope(request.Type);
        if (!string.Equals(session.Scope, expectedScope, StringComparison.Ordinal))
        {
            throw new BadRequestException("Upload token scope không hợp lệ cho media type hiện tại.");
        }

        if (session.ConsumedAtUtc.HasValue || session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new BadRequestException("Upload token đã hết hạn hoặc đã consume.");
        }

        if (!string.Equals(session.ConversationId, conversation.Id, StringComparison.Ordinal))
        {
            throw new BadRequestException("Upload token không thuộc conversation hiện tại.");
        }

        if (!string.Equals(session.ObjectKey, payload.ObjectKey, StringComparison.Ordinal))
        {
            throw new BadRequestException("Object key không khớp với upload token.");
        }

        if (!string.Equals(session.PublicUrl, payload.Url, StringComparison.Ordinal))
        {
            throw new BadRequestException("Media URL không khớp với upload token.");
        }

        if (!string.Equals(session.ContentType, payload.MimeType, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Media mime type không khớp với upload token.");
        }
    }

    private static string ResolveExpectedMediaScope(string messageType)
    {
        return string.Equals(messageType, "voice", StringComparison.OrdinalIgnoreCase)
            ? MediaUploadConstants.ScopeChatVoice
            : MediaUploadConstants.ScopeChatImage;
    }
}
