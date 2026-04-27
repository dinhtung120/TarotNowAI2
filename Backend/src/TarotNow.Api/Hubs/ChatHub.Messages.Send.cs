using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Gửi tin nhắn realtime vào conversation.
    /// Luồng xử lý: xác thực sender, gọi core send-message, trả lỗi realtime khi không đủ quyền.
    /// </summary>
    /// <param name="conversationId">Id conversation đích.</param>
    /// <param name="content">Nội dung tin nhắn thô.</param>
    /// <param name="type">Loại tin nhắn (text/image/voice/payment_offer...).</param>
    public async Task SendMessage(string conversationId, string content, string type = "text")
    {
        if (!TryGetUserGuid(out var userGuid))
        {
            // Chặn gửi tin nhắn khi không parse được user id.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await SendMessageCoreAsync(conversationId, content, type, userGuid);
    }

    /// <summary>
    /// Thực thi nghiệp vụ gửi message và các side effects realtime.
    /// Luồng xử lý: dựng command, gắn payload đặc biệt, gửi message và queue moderation.
    /// </summary>
    /// <param name="conversationId">Id conversation đích.</param>
    /// <param name="content">Nội dung message.</param>
    /// <param name="type">Loại message.</param>
    /// <param name="userGuid">Sender id.</param>
    private async Task SendMessageCoreAsync(
        string conversationId,
        string content,
        string type,
        Guid userGuid)
    {
        try
        {
            var command = BuildSendMessageCommand(conversationId, content, type, userGuid);
            // Gắn payload media/payment khi type đặc biệt để handler xử lý đúng rule.
            TryAttachSpecialPayload(command, content);

            await _mediator.SendWithConnectionCancellation(Context, command, Context.ConnectionAborted);
        }
        catch (BadRequestException ex)
        {
            // Trả lỗi input/business trực tiếp về caller.
            await SendClientErrorAsync(ex.Message);
        }
        catch (NotFoundException ex)
        {
            // Trả lỗi not found khi conversation không tồn tại hoặc không còn truy cập.
            await SendClientErrorAsync(ex.Message);
        }
        catch (Exception ex)
        {
            // Lỗi hạ tầng/ngoại lệ không mong đợi được log và trả thông điệp fallback an toàn.
            _logger.LogError(
                ex,
                "[ChatHub] SendMessage failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to send message. Please try again.");
        }
    }

    /// <summary>
    /// Dựng command gửi message cơ bản từ dữ liệu hub.
    /// </summary>
    /// <param name="conversationId">Id conversation đích.</param>
    /// <param name="content">Nội dung message.</param>
    /// <param name="type">Loại message.</param>
    /// <param name="userGuid">Sender id.</param>
    /// <returns>Command gửi message đã map các trường cơ bản.</returns>
    private static SendMessageCommand BuildSendMessageCommand(
        string conversationId,
        string content,
        string type,
        Guid userGuid)
    {
        return new SendMessageCommand
        {
            ConversationId = conversationId,
            SenderId = userGuid,
            Type = type,
            Content = content
        };
    }
}
