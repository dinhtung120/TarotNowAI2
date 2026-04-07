using System;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private static readonly TimeSpan ConversationAccessCacheDuration = TimeSpan.FromMinutes(2);

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
