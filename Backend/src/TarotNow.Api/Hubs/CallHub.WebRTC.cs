using System;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private static readonly TimeSpan ConversationAccessCacheDuration = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Relay WebRTC SDP Offer
    /// Caller (người gọi) tạo Offer và đẩy vào hub, Hub forward sang cho đối phương.
    /// 
    /// BẢO MẬT: Kiểm tra user có thuộc conversation hay không trước khi relay.
    /// Nếu không kiểm tra, bất kỳ user nào biết conversationId đều có thể inject
    /// SDP/ICE payload vào phòng chat của người khác.
    /// </summary>
    public async Task SendOffer(string conversationId, object sdpOffer)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.offer", sdpOffer);
    }

    /// <summary>
    /// Relay WebRTC SDP Answer
    /// Callee nhận được Offer, tính toán và xuất Answer đẩy về lại hub.
    /// </summary>
    public async Task SendAnswer(string conversationId, object sdpAnswer)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.answer", sdpAnswer);
    }

    /// <summary>
    /// Relay ICE Candidates
    /// Khi STUN server tìm thấy đường truyền hợp lệ (candidate), cả 2 bên P2P sẽ gửi chéo thông tin cho nhau.
    /// </summary>
    public async Task SendIceCandidate(string conversationId, object candidate)
    {
        if (!await IsConversationParticipantAsync(conversationId))
        {
            await SendClientErrorAsync("unauthorized_relay", "Bạn không có quyền gửi tín hiệu vào cuộc trò chuyện này.");
            return;
        }

        await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                     .SendAsync("webrtc.ice-candidate", candidate);
    }

    /// <summary>
    /// Helper kiểm tra user hiện tại có phải participant của conversation hay không.
    /// Dùng cho WebRTC relay để ngăn chặn injection payload từ user ngoài.
    /// </summary>
    private async Task<bool> IsConversationParticipantAsync(string conversationId)
    {
        if (TryGetUserGuid(out var userId) == false)
        {
            return false;
        }

        var userIdText = userId.ToString();
        if (HasConversationAccessCached(userIdText, conversationId))
        {
            return true;
        }

        var cacheKey = $"callhub:conv_access:{userIdText}:{conversationId}";
        var cachedAccess = await _cacheService.GetAsync<bool>(cacheKey);
        if (cachedAccess == true)
        {
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
            RememberConversationAccess(userIdText, conversationId);
            await _cacheService.SetAsync(cacheKey, true, ConversationAccessCacheDuration);
        }

        return isAllowed;
    }
}
