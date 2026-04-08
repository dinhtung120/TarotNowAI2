using System;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    // TTL cache quyền truy cập conversation để giảm truy vấn quyền lặp cho signaling.
    private static readonly TimeSpan ConversationAccessCacheDuration = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Chuyển tiếp SDP offer tới participant còn lại trong conversation.
    /// Luồng xử lý: kiểm tra quyền participant, broadcast offer tới group trừ caller.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh signaling.</param>
    /// <param name="sdpOffer">Payload SDP offer từ caller.</param>
    public async Task SendOffer(string conversationId, object sdpOffer)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            // Chặn relay signaling trái phép để bảo vệ conversation boundary.
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.offer", sdpOffer);
    }

    /// <summary>
    /// Chuyển tiếp SDP answer tới participant còn lại.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh signaling.</param>
    /// <param name="sdpAnswer">Payload SDP answer từ caller.</param>
    public async Task SendAnswer(string conversationId, object sdpAnswer)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            // Chặn relay answer khi user không có quyền trong conversation.
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.answer", sdpAnswer);
    }

    /// <summary>
    /// Chuyển tiếp ICE candidate tới participant còn lại.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh signaling.</param>
    /// <param name="candidate">Payload ICE candidate.</param>
    public async Task SendIceCandidate(string conversationId, object candidate)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            // Chặn relay ICE candidate trái phép để giữ an toàn kênh signaling.
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.ice-candidate", candidate);
    }

    /// <summary>
    /// Kiểm tra user hiện tại có quyền tham gia conversation hay không.
    /// Luồng xử lý: kiểm tra in-memory cache, kiểm tra distributed cache, fallback truy vấn nghiệp vụ.
    /// </summary>
    /// <param name="conversationId">Id conversation cần xác minh quyền.</param>
    /// <returns><c>true</c> nếu user được phép relay signaling; ngược lại <c>false</c>.</returns>
    private async Task<bool> IsConversationParticipantAsync(string conversationId)
    {
        if (TryGetUserGuid(out var userId) == false)
        {
            // Không parse được user id thì xem như không đủ quyền.
            return false;
        }

        var userIdText = userId.ToString();
        if (HasConversationAccessCached(userIdText, conversationId))
        {
            // Cache memory hit giúp giảm độ trễ cho signaling liên tục.
            return true;
        }

        var cacheKey = $"callhub:conv_access:{userIdText}:{conversationId}";
        var cachedAccess = await _cacheService.GetAsync<bool>(cacheKey);
        if (cachedAccess == true)
        {
            // Đồng bộ lại cache memory khi distributed cache đã có kết quả cho phép.
            RememberConversationAccess(userIdText, conversationId);
            return true;
        }

        var accessStatus = await _mediator.Send(new ValidateConversationAccessQuery
        {
            ConversationId = conversationId,
            RequesterId = userId
        });

        var isAllowed = accessStatus == ConversationAccessStatus.Allowed;
        if (isAllowed)
        {
            // Ghi cả memory + distributed cache để tối ưu các lần signaling tiếp theo.
            RememberConversationAccess(userIdText, conversationId);
            await _cacheService.SetAsync(cacheKey, true, ConversationAccessCacheDuration);
        }

        return isAllowed;
    }
}
