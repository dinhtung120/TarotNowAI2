using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Api.Extensions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Conversations)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
[EnableRateLimiting("auth-session")]
// API điều phối toàn bộ luồng hội thoại user-reader.
// Luồng chính: cung cấp helper xác thực user và broadcast cập nhật conversation qua SignalR.
public partial class ConversationController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly IConversationRepository ConversationRepository;
    protected readonly IHubContext<ChatHub> ChatHubContext;

    /// <summary>
    /// Khởi tạo controller hội thoại.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query hội thoại.</param>
    /// <param name="conversationRepository">Repository đọc thông tin conversation để broadcast.</param>
    /// <param name="chatHubContext">Hub context gửi sự kiện realtime cho client.</param>
    public ConversationController(
        IMediator mediator,
        IConversationRepository conversationRepository,
        IHubContext<ChatHub> chatHubContext)
    {
        Mediator = mediator;
        ConversationRepository = conversationRepository;
        ChatHubContext = chatHubContext;
    }

    /// <summary>
    /// Thử lấy user id từ context xác thực hiện tại.
    /// </summary>
    /// <param name="userId">User id đầu ra nếu lấy thành công.</param>
    /// <returns><c>true</c> nếu lấy được user id; ngược lại <c>false</c>.</returns>
    protected bool TryGetUserId(out Guid userId)
    {
        return User.TryGetUserId(out userId);
    }

    /// <summary>
    /// Tạo tên group SignalR theo conversation id.
    /// </summary>
    /// <param name="conversationId">Id hội thoại.</param>
    /// <returns>Tên group conversation dùng cho broadcast realtime.</returns>
    protected static string ConversationGroup(string conversationId) => $"conversation:{conversationId}";

    /// <summary>
    /// Tạo tên group SignalR theo user id.
    /// </summary>
    /// <param name="userId">Id người dùng.</param>
    /// <returns>Tên group user dùng cho push sự kiện cá nhân.</returns>
    protected static string UserGroup(string userId) => $"user:{userId}";

    /// <summary>
    /// Broadcast sự kiện cập nhật hội thoại tới hai phía tham gia.
    /// Luồng xử lý: validate id, tải conversation, gửi event SignalR theo group user.
    /// </summary>
    /// <param name="conversationId">Id hội thoại cần broadcast.</param>
    /// <param name="type">Loại cập nhật để client xử lý UI.</param>
    protected async Task TryBroadcastConversationUpdatedAsync(string conversationId, string type)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            // Edge case id rỗng: bỏ qua broadcast để tránh gửi sự kiện không định danh.
            return;
        }

        try
        {
            var conversation = await ConversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
            {
                // Conversation không tồn tại thì không có đối tượng nhận broadcast hợp lệ.
                return;
            }

            // Gửi đồng thời cho cả user và reader để UI hai phía đồng bộ trạng thái thời gian thực.
            await ChatHubContext.Clients.Groups(
                UserGroup(conversation.UserId),
                UserGroup(conversation.ReaderId)).SendAsync("conversation.updated", new
            {
                conversationId,
                type,
                at = DateTime.UtcNow
            });
        }
        catch
        {
            // Broadcast thất bại không được làm hỏng luồng API chính; bỏ qua để giữ tính chịu lỗi.
        }
    }
}
