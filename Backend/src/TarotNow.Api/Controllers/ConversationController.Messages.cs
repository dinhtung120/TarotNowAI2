using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Chat.Queries.ListMessages;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Lấy danh sách tin nhắn của một hội thoại theo cursor pagination.
    /// Luồng xử lý: xác thực user, gửi query list messages, trả kết quả cho client.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="cursor">Con trỏ phân trang tùy chọn.</param>
    /// <param name="limit">Giới hạn số tin nhắn trả về.</param>
    /// <returns>Danh sách tin nhắn theo cursor.</returns>
    [HttpGet("{id}/messages")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> Messages(
        string id,
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 50)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn truy vấn message khi không xác định được người yêu cầu.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new ListMessagesQuery
        {
            ConversationId = id,
            RequesterId = userId,
            Cursor = cursor,
            Limit = limit
        });

        return Ok(result);
    }

    /// <summary>
    /// Gửi tin nhắn mới vào hội thoại.
    /// Luồng xử lý: xác thực sender, gửi command tạo message và xếp hàng moderation.
    /// </summary>
    /// <param name="id">Id hội thoại đích.</param>
    /// <param name="body">Payload tin nhắn gửi lên.</param>
    /// <returns>Tin nhắn vừa tạo.</returns>
    [HttpPost("{id}/messages")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> SendMessage(string id, [FromBody] ConversationSendMessageBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn gửi tin nhắn khi sender không hợp lệ để bảo toàn tính toàn vẹn dữ liệu.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new SendMessageCommand
        {
            ConversationId = id,
            SenderId = userId,
            // Chuẩn hóa type về text khi client gửi rỗng để tránh nhánh nghiệp vụ không xác định.
            Type = string.IsNullOrWhiteSpace(body.Type) ? "text" : body.Type.Trim(),
            Content = body.Content,
            ClientMessageId = body.ClientMessageId,
            MediaPayload = body.MediaPayload
        });

        return Ok(result);
    }
}
