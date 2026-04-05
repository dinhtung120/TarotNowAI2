using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using System.Security.Claims;

namespace TarotNow.Api.Hubs;

/// <summary>
/// Hub quản lý kết nối và WebRTC Signaling cho các cuộc gọi Voice/Video.
/// WebRTC P2P trao đổi thông tin (SDP, ICE Candidates) thông qua Hub này.
/// </summary>
[Authorize]
public partial class CallHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IConversationRepository _conversationRepository;
    private readonly ILogger<CallHub> _logger;
    private readonly ICacheService _cacheService;
    /* FIX #22: Inject ICallSessionRepository trực tiếp qua constructor.
     * Trước đây dùng Context.GetHttpContext()?.RequestServices.GetService<>() 
     * trong OnDisconnectedAsync → ObjectDisposedException vì DI scope đã bị dispose
     * khi WebSocket connection đóng. Constructor injection an toàn vì Hub instance
     * tồn tại suốt vòng đời của connection. */
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public CallHub(
        IMediator mediator,
        IConversationRepository conversationRepository,
        ILogger<CallHub> logger,
        ICacheService cacheService,
        ICallSessionRepository callSessionRepository,
        IHubContext<ChatHub> chatHubContext)
    {
        _mediator = mediator;
        _conversationRepository = conversationRepository;
        _logger = logger;
        _cacheService = cacheService;
        _callSessionRepository = callSessionRepository;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// Helper: Lấy userId từ JWT claim.
    /// </summary>
    protected string GetUserId()
    {
        return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    /// <summary>
    /// Helper: Lấy User Guid an toàn.
    /// </summary>
    protected bool TryGetUserGuid(out Guid userId)
    {
        return Guid.TryParse(GetUserId(), out userId);
    }

    /// <summary>
    /// Helper: Lấy tên group theo Conversation ID để broadcast trong group kín (User &amp; Reader).
    /// </summary>
    protected string ConversationGroup(string conversationId) => $"conversation:{conversationId}";

    /// <summary>
    /// Báo lỗi tới client cụ thể.
    /// </summary>
    protected async Task SendClientErrorAsync(string errorKey, string message)
    {
        await Clients.Caller.SendAsync("call.error", new { errorKey, message });
    }
}
