using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TarotNow.Api.Constants;
using TarotNow.Application.Interfaces;
using System.Security.Claims;

namespace TarotNow.Api.Hubs;

[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
// SignalR hub điều phối cuộc gọi realtime.
// Luồng chính: xác thực người dùng, chuyển tiếp signaling và đồng bộ trạng thái cuộc gọi qua chat hub.
public partial class CallHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<CallHub> _logger;
    private readonly ICacheService _cacheService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    /// <summary>
    /// Khởi tạo call hub.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query nghiệp vụ call.</param>
    /// <param name="logger">Logger phục vụ quan sát lỗi realtime.</param>
    /// <param name="cacheService">Cache dùng cho rate-limit và access cache.</param>
    /// <param name="chatHubContext">Hub context gửi message/cập nhật conversation.</param>
    public CallHub(
        IMediator mediator,
        ILogger<CallHub> logger,
        ICacheService cacheService,
        IHubContext<ChatHub> chatHubContext)
    {
        _mediator = mediator;
        _logger = logger;
        _cacheService = cacheService;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// Lấy user id dạng chuỗi từ claim hiện tại của kết nối hub.
    /// </summary>
    /// <returns>User id string; trả rỗng nếu không tồn tại claim.</returns>
    protected string GetUserId()
    {
        return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    /// <summary>
    /// Thử parse user id claim thành Guid.
    /// </summary>
    /// <param name="userId">User id đầu ra nếu parse thành công.</param>
    /// <returns><c>true</c> nếu parse thành công; ngược lại <c>false</c>.</returns>
    protected bool TryGetUserGuid(out Guid userId)
    {
        return Guid.TryParse(GetUserId(), out userId);
    }

    /// <summary>
    /// Tạo tên group SignalR cho một conversation.
    /// </summary>
    /// <param name="conversationId">Id hội thoại.</param>
    /// <returns>Tên group conversation dùng cho broadcast.</returns>
    protected string ConversationGroup(string conversationId) => $"conversation:{conversationId}";

    /// <summary>
    /// Gửi sự kiện lỗi realtime về caller hiện tại.
    /// </summary>
    /// <param name="errorKey">Mã lỗi ngắn để client xử lý.</param>
    /// <param name="message">Thông điệp lỗi hiển thị cho người dùng.</param>
    protected async Task SendClientErrorAsync(string errorKey, string message)
    {
        await Clients.Caller.SendAsync("call.error", new { errorKey, message });
    }
}
